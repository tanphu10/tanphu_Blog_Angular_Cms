using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using TPBlog.Api.Extensions;
using TPBlog.Api.Services.IServices;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Models.Auth;
using TPBlog.Core.Models.system;
using TPBlog.Core.SeedWorks.Contants;
using TPBlog.Data;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _siginManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly TPBlogContext _context;
        private readonly IPermissionService _permissionService;
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager
            , ITokenService tokenService, TPBlogContext context, IPermissionService permissionService)
        {
            _userManager = userManager;
            _siginManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<AuthenticatedResult>> Login([FromBody] LoginRequest request)
        {
            //authentication
            if (request == null)
            {
                return BadRequest("Invalid request");
            }
            //ở đây thì nếu giả sử như UserName nó có thể trùng dữ liệu rồi thì như thế nào nó đã check khi tạo ra rồi
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
            var permission = this.GetPermissionByUserIdAsync(user.Id.ToString()).Result;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(UserClaims.Id,user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(UserClaims.FirstName,user.FirstName),
                new Claim(UserClaims.Roles,string.Join(";",roles)),
                new Claim(UserClaims.Permissions,JsonSerializer.Serialize(permission)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(UserClaims.Avatar,user.Avatar),
            };
            var accessToken = _tokenService.GenerateAccessToken(claims);
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
        private async Task<List<string>> GetPermissionByUserIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var permissions = new List<string>();
            //là  tạo ra các type value display của RoleCliamsDto
            var allPermissions = new List<RoleClaimsDto>();
            if (roles.Contains(Roles.Admin))
            {
                // Lấy danh sách dự án đang hoạt động
                var projects = await _context.Project
                    .Where(x => x.IsActive)
                    .Select(p => new { p.Id, p.Slug })
                    .ToListAsync();

                // Tạo quyền cho từng dự án
                permissions.AddRange(projects.Select(project =>
                    $"Permissions.Projects.{project.Slug}"));
                var types = typeof(Permissions).GetTypeInfo().DeclaredNestedTypes;


                foreach (var item in types)
                {
                    allPermissions.GetPermissions(item);
                }
                permissions.AddRange(allPermissions.Select(x => x.Value));
            }
            else
            {
                foreach (var roleName in roles)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);
                    var claims = await _roleManager.GetClaimsAsync(role);
                    var roleClaimValues = claims.Select(x => x.Value).ToList();
                    permissions.AddRange(roleClaimValues);
                }
            }
            return permissions.Distinct().ToList();
        }
    }
}
