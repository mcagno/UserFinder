using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace UserFinder.NotificationService;

public class NotificationConsumer : IConsumer<NotificationMessage>
{
    private readonly ILogger<NotificationConsumer> _logger;

    public NotificationConsumer(ILogger<NotificationConsumer> logger) => _logger = logger;
    
    public Task Consume(ConsumeContext<NotificationMessage> context)
    {
        _logger.LogInformation($"PID {Process.GetCurrentProcess().Id}: User with email {context.Message.Email} received for notification");
        return Task.CompletedTask;
    }
}