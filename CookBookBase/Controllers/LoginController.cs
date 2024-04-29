using CookBookBase;
using CookBookBase.Controllers;
using CookBookBase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;

namespace AuthenticationAndAuthorization.Controllers
{

    [ApiController]
    public class LoginController : ControllerBase
    {
        CookBookDbContext _context = new CookBookDbContext();
        PasswordHasher passwordHasher = new PasswordHasher();
        IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        [HttpPost]
        [Route("Login")]
        public IActionResult PostLoginDetails(User _userData)
        {
            if (_userData != null)
            {
                var resultLoginCheck = _context.Users   
                    .Where(e => e.Nick == _userData.Nick)
                    .FirstOrDefault();
               var result = passwordHasher.VerifyPassword(resultLoginCheck.Hashpassword, _userData.Hashpassword);
                if (!result)
                {
                    return BadRequest("Password is incorrect");
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


                    var response = new TokenResponse() { jwttoken = ActiveToken, refreshtoken = GenerateRefreshToken(_userData.Nick)};

                    return Ok(response);
                   
                }
            }
            else
            {
                return BadRequest("No Data Posted");
            }
        }

        private string GenerateRefreshToken(string userName)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, userName),
                    };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtR:Key").Value));

            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCred);

            var ActiveToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return ActiveToken;

        }

        [HttpPost("RefToken")]
        public async Task<IActionResult> RefToken([FromBody] TokenResponse tokenResponse)
        {


            return Ok();
        }

        private static DecodedToken DecodeJwt(JwtSecurityToken token)
        {
            var keyId = token.Header.Kid;
            var audience = token.Audiences.ToList();
            
            var claims = token.Claims.Select(claim=>(claim.Type,claim.Value)).ToList();

            return new DecodedToken(
            keyId, token.Issuer, audience, claims, token.ValidTo, token.SignatureAlgorithm,token.RawData,token.Subject,token.ValidFrom,token.EncodedHeader,token.EncodedPayload)
            ;
        }

    }
}
