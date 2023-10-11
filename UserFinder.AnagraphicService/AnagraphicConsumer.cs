using System.Diagnostics;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace UserFinder.AnagraphicService;

public class AnagraphicConsumer : IConsumer<AnagraphicMessage>
{
    private readonly ILogger<AnagraphicConsumer> _logger;

    public AnagraphicConsumer(ILogger<AnagraphicConsumer> logger) => _logger = logger;
    
    public Task Consume(ConsumeContext<AnagraphicMessage> context)
    {
        _logger.LogInformation($"PID {Process.GetCurrentProcess().Id}: User {context.Message.FirstName} {context.Message.LastName} received for anagraphic");
        return Task.CompletedTask;
    }
}

public record AnagraphicMessage
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}