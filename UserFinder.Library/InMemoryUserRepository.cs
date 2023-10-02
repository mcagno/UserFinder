namespace UserFinder.Library;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new()
    {
        new User
        {
            Id = 1,
            FirstName = "Matteo",
            LastName = "Rossi"
        },
        new User
        {
            Id = 2,
            FirstName = "Andrea",
            LastName = "Bianchi"
        }

    };

    public async Task<IEnumerable<User>> GetUsersAsync(string searchString)
    {
        return _users.Where(x => x.FirstName.Contains(searchString) || x.LastName.Contains(searchString));
    }

    public async Task InsertUserAsync(User user)
    {   
        _users.Add(user);
    }
}