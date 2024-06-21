using CookBookBase;
using CookBookBase.Helpers.DataHelpers;
using CookBookBase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;

namespace CookBookBase.Helpers
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
        [Route("api/Login")]
        public IActionResult PostLoginDetails(User _userData)
        {
            if (_userData != null)
            {
                var resultLoginCheck = _context.Users
                    .Where(e => e.Nick == _userData.Nick)
                    .FirstOrDefault();
                if (resultLoginCheck == null)
                {
                    return BadRequest("User not found");
                }
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


                    var response = new TokenResponse() { jwttoken = ActiveToken, refreshtoken = GenerateRefreshToken(_userData.Nick), Id = resultLoginCheck.Id };

                    return Ok(response);

                }
            }
            else
            {
                return BadRequest("No Data Posted");
            }
        }

        [NonAction]
        public string GenerateRefreshToken(string userName)
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

        [HttpPost("api/RefreshToken")]
        public IActionResult RefToken([FromBody] TokenResponse tokenResponse)
        {
            var UserName = RefreshToken(tokenResponse);
            if (!UserName.isSuccess)
            {
                return BadRequest(UserName.exeption);
            }
            var AccessToken = GenerateToken(UserName.value);
            var RefToken = GenerateRefreshToken(UserName.value);

            var response = new TokenResponse() { jwttoken = AccessToken, refreshtoken = RefToken };

            return Ok(response);
        }

        private static DecodedToken DecodeJwt(JwtSecurityToken token)
        {
            var keyId = token.Header.Kid;
            var audience = token.Audiences.ToList();

            var claims = token.Claims.Select(claim => (claim.Type, claim.Value)).ToList();

            return new DecodedToken(
            keyId, token.Issuer, audience, claims, token.ValidTo, token.SignatureAlgorithm, token.RawData, token.Subject, token.ValidFrom, token.EncodedHeader, token.EncodedPayload)
            ;
        }

        [NonAction]
        public string GenerateToken(string userName)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, userName),
                    };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));

            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCred);

            var ActiveToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return ActiveToken;

        }


        [NonAction]
        public ValidValue<ClaimsPrincipal> GetPrincipal(string token)
        {
            var result = new ValidValue<ClaimsPrincipal>();
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            try
            {
                var principal = tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = _configuration.GetSection("Jwt:Audience").Value,
                        ValidIssuer = _configuration.GetSection("Jwt:Issuer").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value))
                    }, out validatedToken);
                var jwtToken = validatedToken as JwtSecurityToken;
                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature))
                {
                    result.exeption = "Invalid token passed!";
                    result.isSuccess = false;
                }
                result.value = principal;
                result.isSuccess = true;
            }
            catch (Exception ex)
            {
                result.exeption = ex.Message;
                result.isSuccess = false;
            }
            return result;
        }

        [NonAction]
        public ValidValue<ClaimsPrincipal> GetRefreshPrincipal(string token)
        {
            var result = new ValidValue<ClaimsPrincipal>();
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            try
            {
                var principal = tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = _configuration.GetSection("Jwt:Audience").Value,
                        ValidIssuer = _configuration.GetSection("Jwt:Issuer").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtR:Key").Value))
                    }, out validatedToken);
                var jwtToken = validatedToken as JwtSecurityToken;
                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature))
                {
                    result.exeption = "Invalid token passed!";
                    result.isSuccess = false;
                }
                result.value = principal;
                result.isSuccess = true;
            }
            catch (Exception ex)
            {
                result.exeption = ex.Message;
                result.isSuccess = false;
            }
            return result;
        }

        [NonAction]
        public ValidValue<string> RefreshToken(TokenResponse response)
        {
            var result = new ValidValue<string>();
            var AccessPrincipal = GetPrincipal(response.jwttoken);
            var RefreshPrincipal = GetRefreshPrincipal(response.refreshtoken);
            if (AccessPrincipal.isSuccess)
            {
                var RefreshUserName = RefreshPrincipal.value.Claims.First().Value;
                var AccessUserName = AccessPrincipal.value.Claims.First().Value;
                if (RefreshUserName != AccessUserName)
                {
                    result.exeption = "Tokens are not valid";
                    result.isSuccess = false;
                }
                else
                {
                    result.value = AccessUserName;
                    result.isSuccess = true;
                }

            }
            else
            {
                result.exeption = "Tokens are not valid";
                result.isSuccess = false;
            }

            return result;
        }

    }
}
