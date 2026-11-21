using Burgers.Api.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Burgers.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config ;

        private readonly UserManager<IdentityUser> _user;

        public AuthController(IConfiguration config, UserManager<IdentityUser> user)
        {
            _user = user;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> VerificationAsync(LoginDto Login)
        {
            var user = await _user.FindByEmailAsync(Login.Email);

            if (user == null || !await _user.CheckPasswordAsync(user, Login.Password))
            {
                return Unauthorized("Invalid Email Or Password");
            }
            var claims = new[]
                 {
                     new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                     new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                 }; 

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiryMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expires });
        }

    }
}

