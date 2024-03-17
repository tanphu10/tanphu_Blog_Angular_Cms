using System.Security.Claims;

namespace TPBlog.Api.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpriedToken(string token);
    }
}
