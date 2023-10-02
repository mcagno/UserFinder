using System.Text.Json;
using MassTransit;
using NServiceBus;
using UserFinder.API.Messages;
using UserFinder.API.Services;
using UserFinder.Library;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddScoped<UserFinderService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "policyName",
        policyBuilder =>
        {
            policyBuilder
                .WithOrigins("*") // specifying the allowed origin
                .WithMethods("*") // defining the allowed HTTP method
                .AllowAnyHeader(); // allowing any header to be sent
        });
});
if (builder.Configuration["EventBusSettings:BrokerType"] == "MassTransit")
{
    ConfigureMassTransit(builder);
} else if (builder.Configuration["EventBusSettings:BrokerType"] == "NServiceBus")
{
    ConfigureNServiceBus(builder);
} 



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("policyName");

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureNServiceBus(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Host.UseNServiceBus(context =>
    {
        switch (webApplicationBuilder.Configuration["EventBusSettings:Type"])
        {
            case "RabbitMQ":
                
                var endpointConfiguration = new EndpointConfiguration("user-exchange");
                endpointConfiguration.SendOnly();
                endpointConfiguration.UseSerialization<SystemJsonSerializer>();
                var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                transport.UseDirectRoutingTopology(QueueType.Quorum, exchangeNameConvention: () => "user-exchange");
                transport.ConnectionString(webApplicationBuilder.Configuration["EventBusSettings:HostAddress"]);
                //transport.Routing().RouteToEndpoint(typeof(UserInsertedMessage),"user-exchange");
                
                return endpointConfiguration;
            default:
                throw new ConfigurationException("EventBusSettings type not recognised");
        }
    });
}

void ConfigureMassTransit(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddMassTransit(config =>
    {
        switch (webApplicationBuilder.Configuration["EventBusSettings:Type"])
        {
            case "RabbitMQ":
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.ClearSerialization();
                    cfg.UseRawJsonSerializer(RawSerializerOptions.AnyMessageType);
                    cfg.Host(webApplicationBuilder.Configuration["EventBusSettings:HostAddress"]);
                    cfg.Message<UserInsertedMessage>(x => x.SetEntityName("user-exchange"));
                });
                break;
            default:
                throw new ConfigurationException("EventBusSettings type not recognised");
        }
    });
}