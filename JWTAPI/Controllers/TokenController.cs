using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWTAPI.Data;
using JWTAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly JWTDbContext context;
        private readonly IConfiguration config;

        public TokenController(JWTDbContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        public async Task<User> GetUser(string userName, string password)
        {
            return await context.Users.FirstOrDefaultAsync(i => i.UserName == userName && i.Password == password);
        }

        [HttpPost]
        public async Task<IActionResult> Post(User userData)
        {
            if (userData != null && userData.UserName != null && userData.Password != null)
            {
                var user = await GetUser(userData.UserName, userData.Password);
                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, config["JWT:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(nameof(user.Id), user.Id.ToString()),
                        new Claim(nameof(user.FirstName), user.FirstName),
                        new Claim(nameof(user.LastName), user.LastName),
                        new Claim(nameof(user.UserName), user.UserName),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(config["JWT:Issuer"], config["JWT:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
