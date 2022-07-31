using Microsoft.IdentityModel.Tokens;
using MudWasm.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MudWasm.Server.Authentication
{
    public class JwtAuthManager
    {
        private readonly IConfiguration _configuration;
        private const int JWT_LIFETIME_MINS = 20;

        private UserService _userService;

        public JwtAuthManager(UserService userService, IConfiguration configuration)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public UserSession? GenerateJwtToken(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _userService.GetUser(username, password);
            if (user is null)
                return null;
            
            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_LIFETIME_MINS);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserSession
            {
                Token = tokenAsString,
                ExpireTime = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
            };
        }
    }
}
