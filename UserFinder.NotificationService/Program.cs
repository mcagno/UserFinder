using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UserFinder.NotificationService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(x => x.AddConsole());
                    
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        x.AddConsumer<NotificationConsumer>();
                        
                        var queueType = hostContext.Configuration["EventBusSettings:Type"];
                        if (queueType == "AmazonSQS")
                        {

                            x.UsingAmazonSqs((context, cfg) =>
                            {
                                cfg.ClearSerialization();
                                cfg.UseRawJsonSerializer(RawSerializerOptions.AnyMessageType);
                                cfg.Host("us-east-1", configurator => {});    
                            
                                var queueName = hostContext.Configuration["EventBusSettings:NotificationQueueName"];

                                if (queueName != null)
                                    cfg.ReceiveEndpoint(queueName,
                                        e =>
                                        {
                                            e.ConfigureConsumer<NotificationConsumer>(context);
                                            e.ConfigureConsumeTopology = false;
                                            var exchangeName = hostContext.Configuration["EventBusSettings:UserExchangeName"];
                                            e.Subscribe(exchangeName, s =>
                                            {
                                                // set topic attributes
                                                /*s.TopicAttributes["DisplayName"] = "Public Event Topic";
                                            s.TopicSubscriptionAttributes["some-subscription-attribute"] = "some-attribute-value";*/
                                                s.TopicTags.Add("environment", "development");
                                            });
                                        });
                                cfg.ConfigureEndpoints(context);
                            });
                        } else if (queueType == "RabbitMQ")
                        {
                            x.UsingRabbitMq((context, cfg) =>
                            {
                                cfg.ClearSerialization();
                                cfg.UseRawJsonSerializer(RawSerializerOptions.AnyMessageType);
                                cfg.Host("localhost", "/", h =>
                                {
                                    h.Username("guest");
                                    h.Password("guest");
                                });
                                var queueName = hostContext.Configuration["EventBusSettings:NotificationQueueName"];

                                if (queueName != null)
                                    cfg.ReceiveEndpoint(queueName,
                                        e =>
                                        {
                                            e.ConfigureConsumer<NotificationConsumer>(context);
                                            e.Bind(hostContext.Configuration["EventBusSettings:UserExchangeName"]);
                                        });
                                cfg.ConfigureEndpoints(context);
                            });
                        }
                        
                        

                    });
                });
    }
    
}
