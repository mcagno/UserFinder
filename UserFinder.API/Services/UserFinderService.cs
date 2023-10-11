using UserFinder.API.Messages;
using UserFinder.Library;

namespace UserFinder.API.Services;

public class UserFinderService
{
    private readonly IUserRepository _userRepository;
    
    private readonly IMessageSender _messageSender;

    public UserFinderService(IUserRepository userRepository, IMessageSender messageSender)
    {
        _userRepository = userRepository;
        _messageSender = messageSender;
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
            await _messageSender.Send(userInsertedMessage);
        }

    }
}