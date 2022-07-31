using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MudWasm.Server.Authentication;
using MudWasm.Shared;

namespace MudWasm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        private IConfiguration _configuration;

        public UserController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<UserSession> Login([FromBody] LoginRequest loginRequest)
        {
            var jwtManager = new JwtAuthManager(_userService, _configuration);
            var session = jwtManager.GenerateJwtToken(loginRequest.Username, loginRequest.Password);

            return session is not null ? session : Unauthorized();
        }
    }
}
