using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TPBlog.Api.Extensions;
using TPBlog.Api.Services.IServices;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Core.SeedWorks.Contants;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/Project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permission;
        public ProjectController(IMapper mapper, IUnitOfWork unitOfWork, IPermissionService permission)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _permission = permission;
        }
        [HttpPost]
        [Authorize(Permissions.Projects.Create)]
        public async Task<IActionResult> CreateProject([FromBody] CreateUpdateProjectRequest request)
        {
            var post = _mapper.Map<CreateUpdateProjectRequest, IC_Project>(request);
            post.DateCreated = DateTimeOffset.Now;
            _unitOfWork.IC_Projects.Add(post);

            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut]
        [Authorize(Permissions.Projects.Edit)]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] CreateUpdateProjectRequest request)
        {
            var post = await _unitOfWork.IC_Projects.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            _mapper.Map(request, post);

            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }
        [Route("post-project")]
        [HttpPut()]
        [Authorize(Permissions.Projects.Edit)]
        public async Task<IActionResult> AddPostProject([FromBody] AddPostProjectRequest request)
        {
            var isExisted = await _unitOfWork.IC_Projects.IsPostInProject(request.ProjectId, request.PostId);
            if (isExisted)
            {
                return BadRequest($"Bài viết này đã nằm trong Project.");
            }
            await _unitOfWork.IC_Projects.AddPostToProject(request.ProjectId, request.PostId, request.SortOrder);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }
        [HttpGet("post-project/{id}")]
        [Authorize(Permissions.Projects.View)]
        public async Task<ActionResult<List<PostInListDto>>> GetPostsInProject(Guid id)
        {
            var posts = await _unitOfWork.IC_Projects.GetAllPostsInProject(id);
            return Ok(posts);
        }
        [Route("post-project")]
        [HttpDelete()]
        [Authorize(Permissions.Projects.Delete)]
        public async Task<IActionResult> DeletePostProject([FromBody] AddPostProjectRequest request)
        {
            var isExisted = await _unitOfWork.IC_Projects.IsPostInProject(request.ProjectId, request.PostId);
            if (!isExisted)
            {
                return NotFound();
            }
            await _unitOfWork.IC_Projects.RemovePostToProject(request.ProjectId, request.PostId);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }
        [HttpDelete]
        [Authorize(Permissions.Projects.Delete)]
        public async Task<IActionResult> DeleteProject([FromQuery] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var post = await _unitOfWork.IC_Projects.GetByIdAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                _unitOfWork.IC_Projects.Remove(post);
            }
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }
        [HttpGet]
        [Route("{id}")]
        [Authorize(Permissions.Projects.View)]
        public async Task<ActionResult<ProjectDto>> GetProjectById(Guid id)
        {
            var post = await _unitOfWork.IC_Projects.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpGet]
        [Route("paging")]
        [Authorize(Permissions.Projects.View)]
        public async Task<ActionResult<PageResult<ProjectInListDto>>> GetProjectPaging(string? keyword,
          int pageIndex = 1, int pageSize = 10)
        {
            var result = await _unitOfWork.IC_Projects.GetAllPaging(keyword, pageIndex, pageSize);

            return Ok(result);
        }
        [HttpGet]
        [Authorize(Permissions.Projects.View)]
        public async Task<ActionResult<List<ProjectInListDto>>> GetAllProjects()
        {

            var userPermissions = await _permission.UserHasPermissionForProjectAsync();

            var allProjects = await _unitOfWork.IC_Projects.GetAllAsync();


            var allowedProjects = allProjects.Where(p => userPermissions.Contains($"Permissions.Projects.{p.Slug}"));

            var Projects = _mapper.Map<List<ProjectInListDto>>(allowedProjects);
            return Ok(Projects);
        }
    }
}
