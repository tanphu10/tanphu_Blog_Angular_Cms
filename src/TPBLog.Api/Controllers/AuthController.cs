using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TPBlog.Api.Services;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Models.Auth;
using TPBlog.Data.SeedWorks.Contants;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _siginManager;
        private readonly ITokenService _tokenService;
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
            , ITokenService tokenService)
        {
            _userManager = userManager;
            _siginManager = signInManager;
            _tokenService = tokenService;
        }
        [HttpPost]
        public async Task<ActionResult<AuthenticatedResult>> Login([FromBody] LoginRequest request)
        {
            //authentication
            if (request == null)
            {
                return BadRequest("Invalid request");
            }
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null || user.IsActive == false || user.LockoutEnabled)
            {
                return Unauthorized();
            }

            var result = await _siginManager.PasswordSignInAsync(request.UserName, request.Password, false, true);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            //Authorization
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(UserClaims.Id,user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(UserClaims.FirstName,user.FirstName),
                new Claim(UserClaims.Roles,string.Join(",",roles)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            var accessToken = _tokenService.GenerateAccessToken(claims)
                ;
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTOkenExpiryTime = DateTime.Now.AddDays(30);
            await _userManager.UpdateAsync(user);
            return Ok(new AuthenticatedResult()
            {
                Token = accessToken,
                RefreshToken = refreshToken,
            });

        }
    }
}
