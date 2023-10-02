// See https://aka.ms/new-console-template for more information


using UserFinder.Library;

var repository = new InMemoryUserRepository();
while (true)
{
    Console.Write("Insert your search or ! to terminate: ");
    var searchString = Console.ReadLine() ?? string.Empty;
    if (searchString == "!")
        return;
    
    var users = (await repository.GetUsersAsync(searchString)).ToList();
    if (users.Any())
    {
        foreach (var user in users)
        {
            Console.WriteLine($"First name: {user.FirstName} Last name: {user.LastName}");
        }
    }
    else
    {
        Console.WriteLine("No users found");
    }
    
}