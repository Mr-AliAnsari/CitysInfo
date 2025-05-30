using CitysInfo.Domain_Models.User;
using CitysInfo.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Dtos.General;

namespace CitysInfo.Controllers
{
    public class UsersController : MyBaseController
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var loginUser = await _userManager.FindByNameAsync(loginRequestDto.UserName!);
            if (loginUser == null)
            {
                return NotFound();
            }

            return Ok();
        }

        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            var registerUser = new User()
            {
                UserName = registerRequestDto.UserName!,
                Email = registerRequestDto.Email,
            };

            var result =

        }
    }
}
