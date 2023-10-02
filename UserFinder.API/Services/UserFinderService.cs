using MassTransit;
using NServiceBus;
using UserFinder.API.Messages;
using UserFinder.Library;

namespace UserFinder.API.Services;

public class UserFinderService
{
    private readonly IUserRepository _userRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMessageSession _session;

    public UserFinderService(IUserRepository userRepository, IMessageSession session)
    {
        _userRepository = userRepository;
        //_publishEndpoint = publishEndpoint;
        _session = session;
    }

    public async Task<IEnumerable<User>> FindUsersAsync(string searchString)
    {
        var users = await _userRepository.GetUsersAsync(searchString);
        //Notify other services
        return users;
    }

    public async Task InsertUser(User user)
    {
        await _userRepository.InsertUserAsync(user);
        
        for (int i = 0; i < 10; i++)
        {
            var userInsertedMessage = new UserInsertedMessage(user.FirstName, user.LastName,
                user.Email + $" ({i})");
            await _session.Publish(userInsertedMessage);
        }

    }
}