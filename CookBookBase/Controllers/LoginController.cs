using CookBookBase;
using CookBookBase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationAndAuthorization.Controllers
{

    [ApiController]
    public class LoginController : ControllerBase
    {
        CookBookDbContext _context = new CookBookDbContext();

        IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        [HttpPost]
        [Route("PostLoginDetails")]
        public IActionResult PostLoginDetails(User _userData)
        {
            if (_userData != null)
            {
                var resultLoginCheck = _context.Users
                    .Where(e => e.Nick == _userData.Nick && e.Hashpassword == _userData.Hashpassword)
                    .FirstOrDefault();
                if (resultLoginCheck == null)
                {
                    return BadRequest("Invalid Credentials");
                }
                else
                {

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email,_userData.Nick),
                    };

                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));

                    var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

                    var securityToken = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        issuer: _configuration.GetSection("Jwt:Issuer").Value,
                        audience: _configuration.GetSection("Jwt:Audience").Value,
                        signingCredentials: signingCred);

                    var ActiveToken = new JwtSecurityTokenHandler().WriteToken(securityToken);



                    return Ok(ActiveToken);
                }
            }
            else
            {
                return BadRequest("No Data Posted");
            }
        }



    }
}
