using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TPBlog.Api.Extensions;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models.system;
using TPBlog.Core.SeedWorks.Contants;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        public RoleController(RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        [HttpPost]
        [Authorize(Permissions.Roles.Create)]
        public async Task<ActionResult> CreateRole([FromBody] CreateUpdateRoleRequest request)
        {
            await _roleManager.CreateAsync(new AppRole()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                DisplayName = request.DisplayName,
            });
            return new OkResult();
        }
        [HttpPut]
        [Authorize(Permissions.Roles.Edit)]
        public async Task<ActionResult> UpdateRole(Guid id, [FromBody] CreateUpdateRoleRequest request)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
                return NotFound();
            role.Name = request.Name;
            role.DisplayName = request.DisplayName;
            await _roleManager.UpdateAsync(role);
            return Ok();
        }
        [HttpDelete]
        [Authorize(Permissions.Roles.Delete)]
        public async Task<ActionResult> DeleteRoles([FromBody] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role is null)
                return NotFound();
                await _roleManager.DeleteAsync(role);
            }
            return Ok();
        }
        [HttpGet("{id}")]
        //[Authorize(Permissions.Roles.View)]
        public async Task<ActionResult<RoleDto>> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
                return NotFound();
            return Ok(_mapper.Map<AppRole, RoleDto>(role));
        }
        [HttpGet]
        [Route("paging")]
        [Authorize(Permissions.Roles.View)]
        public async Task<ActionResult<PageResult<RoleDto>>> GetPagingRoleAsync(string? keyword, int pageIndex, int PageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword) || x.DisplayName.Contains(keyword));
            var totalRow = query.Count();
            query = query.Skip((pageIndex - 1) * PageSize).Take(PageSize);
            var data = await _mapper.ProjectTo<RoleDto>(query).ToListAsync();
            var paginationSet = new PageResult<RoleDto>
            {
                Results = data,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = PageSize,
            };
            return Ok(paginationSet);
        }
        [HttpGet("all")]
        //[Authorize(Permissions.Roles.View)]
        public async Task<ActionResult<List<RoleDto>>> GetAllRoleAsync()
        {
            var data = await _mapper.ProjectTo<RoleDto>(_roleManager.Roles).ToListAsync();
            return Ok(data);
        }
        [HttpGet("{id}/permission")]
        //[Route("{roleid}/permission")]
        //[Authorize(Permissions.Roles.View)] 
        public async Task<ActionResult<PermissionDto>> GetAllRolePermission(string id)
        {
            var roleId = id;
            var model = new PermissionDto();
            var allPermissions = new List<RoleClaimsDto>();
            var types = typeof(Permissions).GetTypeInfo().DeclaredNestedTypes;

            foreach (var type in types)
            {
                allPermissions.GetPermissions(type);
            }
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();
            model.RoleId = roleId;
            //ở đây sẽ lấy tất cả các quyền mà role đó có ra biến claims
            var claims = await _roleManager.GetClaimsAsync(role);
            //lấy list value của permission
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            //ở đây  sẽ lấy value tất cả quyền mà role đó có
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            //là authorized lấy phần chung của quyền role đó với all các quyền
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;
            return Ok(model);
        }
        [HttpPut("permissions")]
        //[Authorize(Permissions.Roles.Edit)]
        public async Task<IActionResult> SavePermission([FromBody] PermissionDto model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound();
            }
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);

            }
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role, claim.Value);
            }
            return Ok();
        }
    }
}
