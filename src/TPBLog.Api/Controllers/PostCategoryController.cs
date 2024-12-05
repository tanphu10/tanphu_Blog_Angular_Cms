using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;
using static TPBlog.Core.SeedWorks.Contants.Permissions;
using TPBlog.Api.Services.IServices;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/postCategory")]
    [ApiController]
    public class PostCategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        public PostCategoryController(IUnitOfWork unitOfWork, IMapper mapper, IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _permissionService = permissionService;
        }
        [HttpPost]
        [Authorize(PostCategories.View)]

        public async Task<IActionResult> CreatePostCategory([FromBody] CreateUpdatePostCategoryRequest request)
        {

            var project = await _unitOfWork.IC_Projects.GetByIdAsync(request.ProjectId);

            if (project == null)
            {
                throw new Exception("không tồn tại dự án");
            }
            request.DateCreated = DateTimeOffset.Now;


            var post = _mapper.Map<CreateUpdatePostCategoryRequest, IC_PostCategory>(request);

            post.ProjectSlug = project.Slug;
            _unitOfWork.IC_PostCategories.Add(post);

            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut]
        [Authorize(PostCategories.Edit)]
        public async Task<IActionResult> UpdatePostCategory(Guid id, [FromBody] CreateUpdatePostCategoryRequest request)
        {
            var post = await _unitOfWork.IC_PostCategories.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            post.DateLastModified = DateTimeOffset.Now;

            _mapper.Map(request, post);

            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpDelete]
        [Authorize(PostCategories.Delete)]
        public async Task<IActionResult> DeletePostCategory([FromQuery] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var post = await _unitOfWork.IC_PostCategories.GetByIdAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                if (await _unitOfWork.IC_PostCategories.HasPost(id))
                {
                    return BadRequest("Danh mục đang chứa bài viết, không thể xóa");
                }
                _unitOfWork.IC_PostCategories.Remove(post);
            }
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(PostCategories.View)]
        public async Task<ActionResult<PostCategoryDto>> GetPostCategoryById(Guid id)
        {
            var category = await _unitOfWork.IC_PostCategories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = _mapper.Map<PostCategoryDto>(category);
            return Ok(categoryDto);
        }

        [HttpGet]
        [Route("paging")]
        [Authorize(PostCategories.View)]
        public async Task<ActionResult<PageResult<PostCategoryDto>>> GetPostCategoriesPaging(string? keyword, Guid? projectId,
            int pageIndex, int pageSize = 10)
        {

            //var userId = User.GetUserId();
            var userPermissions = await _permissionService.UserHasPermissionForProjectAsync();

            var query = await _unitOfWork.IC_PostCategories.GetPagingPostCategoryAsync(keyword, projectId, pageIndex, pageSize);
            var allowedProjects = query.Results.Where(p => userPermissions.Contains($"Permissions.Projects.{p.ProjectSlug}"));
            query.Results = allowedProjects.ToList();
            return Ok(query);

        }

        [HttpGet]
        [Authorize(PostCategories.View)]
        public async Task<ActionResult<List<PostCategoryDto>>> GetPostCategories()
        {
            var userPermissions = await _permissionService.UserHasPermissionForProjectAsync();

            //var allProjects = await _unitOfWork.Projects.GetAllAsync();

            var query = await _unitOfWork.IC_PostCategories.GetAllAsync();

            var allowedProjects = query.Where(p => userPermissions.Contains($"Permissions.Projects.{p.ProjectSlug}"));
            var model = _mapper.Map<List<PostCategoryDto>>(query);
            return Ok(model);
        }
    }
}
