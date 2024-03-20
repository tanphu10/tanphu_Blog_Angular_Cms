using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TPBlog.Core.ConfigureOptions;

namespace TPBlog.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenSettings _jwtTokenSetting;
        public TokenService(IOptions<JwtTokenSettings> jwtTokenSetting)
        {
            _jwtTokenSetting = jwtTokenSetting.Value;
        }
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenSetting.Key));
            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                //lấy được là nhờ configuration bên program.cs
                issuer: _jwtTokenSetting.Issuer,
                audience: _jwtTokenSetting.Issuer,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwtTokenSetting.ExpireInHours),
                signingCredentials: signInCredentials
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        //get user principle from expired token

        public ClaimsPrincipal GetPrincipalFromExpriedToken(string token)
        {

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false
            };
            var tokenHanlder = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHanlder.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
