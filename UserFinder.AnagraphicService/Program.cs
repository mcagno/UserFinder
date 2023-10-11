using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UserFinder.AnagraphicService
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

                        x.AddConsumer<AnagraphicConsumer>();
                        
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ClearSerialization();
                            cfg.UseRawJsonSerializer(RawSerializerOptions.AnyMessageType);
                            cfg.Host("localhost", "/", h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });
                            var queueName = hostContext.Configuration["EventBusSettings:AnagraphicQueueName"];

                            if (queueName != null)
                                cfg.ReceiveEndpoint(queueName,
                                    e =>
                                    {
                                        e.ConfigureConsumer<AnagraphicConsumer>(context);
                                        e.Bind(hostContext.Configuration["EventBusSettings:UserExchangeName"]);
                                    });
                            cfg.ConfigureEndpoints(context);
                        });

                    });
                });
    }
    
}
