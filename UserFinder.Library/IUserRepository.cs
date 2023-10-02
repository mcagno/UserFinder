namespace UserFinder.Library;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync(string searchString);
    Task InsertUserAsync(User user);
}