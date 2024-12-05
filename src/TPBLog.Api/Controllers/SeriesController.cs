using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPBlog.Api.Services.IServices;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Core.SeedWorks.Contants;
using TPBlog.Data.SeedWorks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/Series")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;

        public SeriesController(IMapper mapper, IUnitOfWork unitOfWork, IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _permissionService = permissionService;
        }
        [HttpPost]
        [Authorize(Permissions.Series.Create)]
        public async Task<IActionResult> CreateSeries([FromBody] CreateUpdateSeriesRequest request)
        {

            var project = await _unitOfWork.IC_Projects.GetByIdAsync(request.ProjectId);
            if (project == null)
            {
                throw new Exception("không tồn tại dự án");
            }

            var post = _mapper.Map<CreateUpdateSeriesRequest, Core.Domain.Content.IC_Series>(request);
            post.ProjectSlug = project.Slug;
            _unitOfWork.IC_Series.Add(post);


            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut]
        [Authorize(Permissions.Series.Edit)]
        public async Task<IActionResult> UpdateSeries(Guid id, [FromBody] CreateUpdateSeriesRequest request)
        {
            var post = await _unitOfWork.IC_Series.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            _mapper.Map(request, post);

            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }
        [Route("post-series")]
        [HttpPut()]
        [Authorize(Permissions.Series.Create)]
        public async Task<IActionResult> AddPostSeries([FromBody] AddPostSeriesRequest request)
        {
            var isExisted = await _unitOfWork.IC_Series.IsPostInSeries(request.SeriesId, request.PostId);
            if (isExisted)
            {
                return BadRequest($"Bài viết này đã nằm trong loạt bài.");
            }
            await _unitOfWork.IC_Series.AddPostToSeries(request.SeriesId, request.PostId, request.SortOrder);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }
        [HttpGet("post-series/{id}")]
        [Authorize(Permissions.Series.View)]
        public async Task<ActionResult<List<PostInListDto>>> GetPostsInSeries(Guid id)
        {
            var posts = await _unitOfWork.IC_Series.GetAllPostsInSeries(id);
            return Ok(posts);
        }
        [Route("post-series")]
        [HttpDelete()]
        [Authorize(Permissions.Series.Delete)]
        public async Task<IActionResult> DeletePostSeries([FromBody] AddPostSeriesRequest request)
        {
            var isExisted = await _unitOfWork.IC_Series.IsPostInSeries(request.SeriesId, request.PostId);
            if (!isExisted)
            {
                return NotFound();
            }
            await _unitOfWork.IC_Series.RemovePostToSeries(request.SeriesId, request.PostId);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }
        [HttpDelete]
        [Authorize(Permissions.Series.Delete)]
        public async Task<IActionResult> DeleteSeries([FromQuery] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var post = await _unitOfWork.IC_Series.GetByIdAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                _unitOfWork.IC_Series.Remove(post);
            }
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }
        [HttpGet]
        [Route("{id}")]
        [Authorize(Permissions.Series.View)]
        public async Task<ActionResult<SeriesDto>> GetSeriesById(Guid id)
        {
            var post = await _unitOfWork.IC_Series.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpGet]
        [Route("paging")]
        [Authorize(Permissions.Series.View)]
        public async Task<ActionResult<PageResult<SeriesInListDto>>> GetSeriesPaging(string? keyword, Guid? projectId, int pageIndex = 1, int pageSize = 10)
        {
            var userPermissions = await _permissionService.UserHasPermissionForProjectAsync();

            var result = await _unitOfWork.IC_Series.GetAllPaging(keyword, projectId, pageIndex, pageSize);
            var allowedProjects = result.Results.Where(p => userPermissions.Contains($"Permissions.Projects.{p.ProjectSlug}"));
            result.Results = allowedProjects.ToList();
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Permissions.Series.View)]
        public async Task<ActionResult<List<SeriesInListDto>>> GetAllSeries()
        {
            var result = await _unitOfWork.IC_Series.GetAllAsync();
            var series = _mapper.Map<List<SeriesInListDto>>(result);
            return Ok(series);
        }
    }
}
