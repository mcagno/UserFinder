using MassTransit;

namespace UserFinder.API.Services;

public interface IMessageSender
{
    Task Send<T>(T message);
}

class NServiceBusMessageSender : IMessageSender
{
    private readonly IMessageSession _messageSession;

    public NServiceBusMessageSender(IMessageSession messageSession)
    {
        _messageSession = messageSession;
    }

    public async Task Send<T>(T message)
    {
        if (message != null) 
            await _messageSession.Publish(message);
    }
}

class MassTransitMessageSender : IMessageSender
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitMessageSender(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Send<T>(T message)
    {
        if (message != null) 
            await _publishEndpoint.Publish(message);
    }
}

