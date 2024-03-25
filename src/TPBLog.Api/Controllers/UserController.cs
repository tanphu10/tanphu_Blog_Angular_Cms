//using AutoMapper;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using TPBlog.Api.Extensions;
//using TPBlog.Core.Domain.Identity;
//using TPBlog.Core.Models;
//using TPBlog.Core.Models.system;

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPBlog.Api.Extensions;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Models;
using TPBlog.Core.Models.system;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IMapper mapper, UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<UserDto>> GetAllUser()
        {
            var data = await _mapper.ProjectTo<UserDto>(_userManager.Users).ToListAsync();
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(Guid id)
        {
            var data = await _userManager.FindByIdAsync(id.ToString());
            if (data == null)
                return NotFound();
            var user = _mapper.Map<AppUser, UserDto>(data);
            var roles = await _userManager.GetRolesAsync(data);
            user.Roles = roles;
            return Ok(user);

        }
        [HttpGet("paging")]
        public async Task<ActionResult<PageResult<UserDto>>> GetAllUserPaging(string? keyword, int pageIndex, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.UserName.Contains(keyword) || x.FirstName.Contains(keyword));
            var totalRow = query.Count();
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var data = await _mapper.ProjectTo<UserDto>(query).ToListAsync();
            var paginationSet = new PageResult<UserDto>
            {
                Results = data,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize,
            };
            return Ok(paginationSet);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if ((await _userManager.FindByNameAsync(request.UserName) != null))
            {
                return BadRequest();
            }
            if ((await _userManager.FindByEmailAsync(request.Email)) != null)
            {
                return BadRequest();
            }
            //AppUser.DateCreated = DateTime.Now; 
            var user = _mapper.Map<CreateUserRequest, AppUser>(request);
            user.DateCreated = DateTime.Now;
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(string.Join("<br>", result.Errors.Select(x => x.Description)));
        }
        [HttpPut("{id}")]
        //[validateModel]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            _mapper.Map(request, user);
            user.Dob = DateTime.Now;
            var res = await _userManager.UpdateAsync(user);
            if (res.Succeeded)
            {
                return Ok(res);
            }
            return BadRequest(string.Join("<br>", res.Errors.Select(x => x.Description)));
        }
        [HttpPut("password-change-current-user")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(User.GetUserId().ToString());
            if (user == null)
                return NotFound();
            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(string.Join("<br>", result.Errors.Select(x => x.Description)));
        }

        [HttpPost("set-password/{id}")]
        public async Task<IActionResult> SetPassword(Guid id, [FromBody] SetPasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound();
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(string.Join("<br>", result.Errors.Select(x => x.Description)));
        }
        [HttpPost("change-email/{id}")]
        public async Task<IActionResult> ChangeEmail(Guid id, [FromBody] ChangeEmailRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, request.Email);
            var result = await _userManager.ChangeEmailAsync(user, request.Email, token);
            if (!result.Succeeded)
            {
                return BadRequest(string.Join("<br>", result.Errors.Select(x => x.Description)));
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUsers([FromBody] string[] ids)
        {
            foreach (var id in ids)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                await _userManager.DeleteAsync(user);

            }
            return Ok();
        }
        [HttpPut("{id}/assign-users")]
        public async Task<IActionResult> AssignRolesToUser(string id, [FromBody] string[] roles)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _unitOfWork.Users.RemoveUserFromRoles(user.Id, currentRoles.ToArray());
            var addResult = await _userManager.AddToRolesAsync(user, roles);
            if (!addResult.Succeeded)
            {
                List<IdentityError> addedErrorList = addResult.Errors.ToList();
                var errorList = new List<IdentityError>();
                errorList.AddRange(addedErrorList);
                return BadRequest(string.Join("<br>", errorList.Select(x => x.Description)));
            }
            return Ok();

        }
    }
}
