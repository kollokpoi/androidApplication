using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DiplomApi.Data.Interfaces;
using DiplomApi.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Configuration;
using Azure.Core;
using System;
using DiplomApi.Data.Classes;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiplomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        
        [HttpGet]
        public List<string> GetId()
        {

            return new List<string>(){
                "fgh", "jhg"
            };
        }

        [Authorize]
        [HttpGet("getUserInfo")]
        public User GetUser(Guid id)
        {
            return Program.context.Users.FirstOrDefault(x => x.Id == id)
                ;
        }
        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            ClaimsIdentity identity = null;
            var person = _userService.GetUserByUsernameAndPassword(username, password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                   new Claim(ClaimTypes.NameIdentifier, person.Id.ToString()),
                    new Claim(ClaimTypes.Name, username)
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");
                identity = claimsIdentity;
            }
            if (identity == null)
            {
                return Unauthorized(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            string userData = JsonConvert.SerializeObject(person);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name,
                user = userData
            };

            return Ok(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {


            return null;
        }
    }
}
