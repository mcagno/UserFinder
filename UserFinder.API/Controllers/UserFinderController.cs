using Microsoft.AspNetCore.Mvc;
using UserFinder.API.Services;
using UserFinder.Library;

namespace UserFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFinderController : ControllerBase
    {
        private readonly UserFinderService _userFinderService;

        public UserFinderController(UserFinderService userFinderService)
        {
            _userFinderService = userFinderService;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers(string searchString)
        {
            return await _userFinderService.FindUsersAsync(searchString);
        }

        [HttpPost]
        public async void InsertUser(User user)
        {
            await _userFinderService.InsertUser(user);
        }
    }
}
