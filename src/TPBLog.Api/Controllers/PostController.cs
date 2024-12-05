using AutoMapper;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using static TPBlog.Core.SeedWorks.Contants.Permissions;
using TPBlog.Core.Domain.Identity;
using TPBlog.Api.Extensions;
using TPBlog.Api.Services;
using TPBlog.Api.Services.IServices;

namespace TPBlog.Api.Controllers
{

    [Route("api/admin/post")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPostService _postService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPermissionService _permissionService;


        public PostController(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IPostService postService, IPermissionService permissionService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _postService = postService;
            _permissionService = permissionService;
        }
        [HttpPost]
        [Authorize(Posts.Create)]
        public async Task<IActionResult> CreatePost([FromBody] CreateUpdatePostRequest request)
        {
            await _postService.CreatePostServiceAsync(request);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }
        [HttpPut]
        [Authorize(Posts.Edit)]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] CreateUpdatePostRequest request)
        {
            await _postService.UpdatePostServiceAsync(id, request);
            var res = await _unitOfWork.CompleteAsync();
            return Ok();
        }
        [HttpGet]
        [Route("{id}")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<PostDto>> GetPostById(Guid id)
        {
            var post = await _unitOfWork.IC_Posts.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<PostInListDto>> GetAllPost()
        {
            var data = await _postService.GetAllPostAsync();
            return Ok(data);
        }

        [HttpGet]
        [Route("series-belong/{id}")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<List<SeriesInListDto>>> GetSeriesBelong(Guid id)
        {

            var result = await _unitOfWork.IC_Posts.GetAllSeries(id);
            return Ok(result);
        }
        [HttpDelete]
        [Authorize(Posts.Delete)]
        public async Task<IActionResult> DeletePosts([FromQuery] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var post = await _unitOfWork.IC_Posts.GetByIdAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                _unitOfWork.IC_Posts.Remove(post);
            }
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }
        [HttpGet]
        [Route("paging")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<PageResult<PostInListDto>>> GetPostsPaging(string? keyword, Guid? categoryId, Guid? projectId, int pageIndex = 1, int pageSize = 10)
        {
            var userId = User.GetUserId();
            var userPermissions = await _permissionService.UserHasPermissionForProjectAsync();

            var query = await _unitOfWork.IC_Posts.GetAllPaging(keyword, userId, categoryId,projectId, pageIndex, pageSize);
            var allowedProjects = query.Results.Where(p => userPermissions.Contains($"Permissions.Projects.{p.ProjectSlug}"));
            query.Results = allowedProjects.ToList();
            return Ok(query);
        }
        [HttpGet("approve/{id}")]
        [Authorize(Posts.Approve)]
        public async Task<IActionResult> ApprovePost(Guid id)
        {
            await _unitOfWork.IC_Posts.Approve(id, User.GetUserId());
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpGet("approval-submit/{id}")]
        [Authorize(Posts.Edit)]
        public async Task<IActionResult> SendToApprove(Guid id)
        {
            await _unitOfWork.IC_Posts.SendToApprove(id, User.GetUserId());
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpPost("return-back/{id}")]
        [Authorize(Posts.Approve)]
        public async Task<IActionResult> ReturnBack(Guid id, [FromBody] ReturnBackRequest model)
        {
            await _unitOfWork.IC_Posts.ReturnBack(id, User.GetUserId(), model.Reason);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpGet("return-reason/{id}")]
        [Authorize(Posts.Approve)]
        public async Task<ActionResult<string>> GetReason(Guid id)
        {
            var note = await _unitOfWork.IC_Posts.GetReturnReason(id);
            return Ok(note);
        }

        [HttpGet("activity-logs/{id}")]
        [Authorize(Posts.Approve)]
        public async Task<ActionResult<List<PostActivityLogDto>>> GetActivityLogs(Guid id)
        {
            var logs = await _unitOfWork.IC_Posts.GetActivityLogs(id);
            return Ok(logs);
        }
        [HttpGet("tags")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<List<string>>> GetAllTags()
        {
            var logs = await _unitOfWork.IC_Tags.GetAllTags();
            return Ok(logs);
        }
        [HttpGet("tags/{id}")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<List<string>>> GetPostTags(Guid id)
        {
            var tagName = await _unitOfWork.IC_Posts.GetTagsByPostId(id);
            return Ok(tagName);
        }
    }
}
