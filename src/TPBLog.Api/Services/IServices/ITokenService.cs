using System.Security.Claims;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Api.Services.IServices
{
    public interface ITokenService: IScopedService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpriedToken(string token);
    }
}
