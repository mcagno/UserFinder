using NRedisStack.Search;
using NRedisStack.Search.Literals.Enums;

namespace UserFinder.Library;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;


public class RedisUserRepository: IUserRepository
{
    private readonly IDatabase _db = ConnectionMultiplexer.Connect("localhost").GetDatabase();


    public async Task<IEnumerable<User>> GetUsersAsync(string searchString)
    {
        
        var jsonCommands = _db.JSON();
        var ft = _db.FT();


        var schema = new Schema()
            .AddTextField(new FieldName("$.firstName", "firstName"))
            .AddTextField(new FieldName("$.lastName", "lastName"));
            

        ft.Create(
            "idx:users",
            new FTCreateParams().On(IndexDataType.JSON).Prefix("user:"),
            schema);

        SearchResult searchResult = ft.Search("idx:users", new Query(searchString));
        //searchResult.Documents.ToList<User>();
        throw new NotImplementedException();
    }

    public async Task InsertUserAsync(User user)
    {
        RedisKey key = new RedisKey($"user:{user.Id}");
        var jsonCommands = _db.JSON();
        RedisValue s = new RedisValue();
        jsonCommands.Set(key, "$", user);
        throw new NotImplementedException();
    }
}