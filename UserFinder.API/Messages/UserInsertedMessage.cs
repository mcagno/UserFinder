using NServiceBus;

namespace UserFinder.API.Messages;
public record UserInsertedMessage(string FirstName, string LastName, string Email) : IEvent;