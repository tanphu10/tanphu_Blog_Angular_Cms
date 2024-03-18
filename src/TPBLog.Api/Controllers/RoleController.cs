using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models.system;

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
        public async Task<ActionResult> UpdateRole(Guid id, [FromBody] CreateUpdateRoleRequest request)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
                return NotFound();
            role.Name = request.Name;
            role.DisplayName = request.DisplayName;
            await _roleManager.UpdateAsync(role);
            return Ok()
;
        }
        [HttpDelete]
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
        public async Task<ActionResult<RoleDto>> GetRoleByIdAsync(Guid id)
        {
            var role = _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
                return NotFound();
            return Ok(role);
        }
        [HttpGet]
        [Route("paging")]
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

        public async Task<ActionResult<PageResult<RoleDto>>> GetAllRoleAsync()
        {
            var data = await _mapper.ProjectTo<RoleDto>(_roleManager.Roles).ToListAsync();
            return Ok(data);
        }
    }
}
