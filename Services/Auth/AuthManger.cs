using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetEnv;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace project_API.Services.Auth;
    public class AuthManger
    {
        public async Task<Token> AuthorizeEmployee(User employee)
        {
            List<Claim> claims = new()
                        {
                        new Claim(ClaimTypes.NameIdentifier,employee.Id.ToString()),
                        new Claim(ClaimTypes.MobilePhone,employee.PhoneNumber.ToString()),
                        new Claim(ClaimTypes.Email,employee.Email),
                        new Claim(ClaimTypes.Name,employee.Name),
                        new Claim(ClaimTypes.Role, employee.Role ? "Admin" : "User")
                        };
            var token = await CreateJwt(claims);
            token.UserId = employee.Id;


            return token;
        }
        public static Task<Token> CreateJwt(List<Claim> claims )
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.GetString("JWT_SECRET"))),
                SecurityAlgorithms.HmacSha256);
            var jwtExpireTime = DateTime.UtcNow.AddDays(30);
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
  
            var securityToken = new JwtSecurityToken(
                audience: Env.GetString("JWT_AUDIENCE"),
                issuer: Env.GetString("JWT_ISSUER"),
                claims: claims,
                expires: jwtExpireTime,
                signingCredentials: signingCredentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(securityToken);

            Token dbToken = new Token { Value = jwt, ExpiresOn = jwtExpireTime  };
           
            return Task.FromResult(dbToken);
        }

        /*
        public async Task RemoveJWTAsync()
        {
            var token = _contextAccessor!.HttpContext!.Request.Headers.Authorization.FirstOrDefault();
            if (token != null)
            {
                token = token.Remove(0, 6);
                token = token.Replace(" ", "");
                var dbToken = _db.Tokens.SingleOrDefault(T => T.Value == token);
                if (dbToken is not null)
                {
                    _db.Remove(token);
                    _db.SaveChanges();
                }
            }
        }
        */
    }

