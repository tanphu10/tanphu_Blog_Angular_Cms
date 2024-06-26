﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using static TPBlog.Core.SeedWorks.Contants.Permissions;
using TPBlog.Api.Extensions;
using TPBlog.Core.Domain.Identity;
using System.Diagnostics;
using TPBlog.Core.Helpers;

namespace TPBlog.Api.Controllers
{

    [Route("api/admin/post")]
    [ApiController]
    //[Authorize]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public PostController(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Posts.Create)]
        public async Task<IActionResult> CreatePost([FromBody] CreateUpdatePostRequest request)
        {
            if (await _unitOfWork.BaiPost.IsSlugAlreadyExisted(request.Slug))
            {
                return BadRequest("Đã tồn tại slug");
            }
            var post = _mapper.Map<CreateUpdatePostRequest, Post>(request);
            var postId = Guid.NewGuid();
            var category = await _unitOfWork.PostCategories.GetByIdAsync(request.CategoryId);
            post.Id = postId;
            post.CategoryName = category.Name;
            post.CategorySlug = category.Slug;
            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            post.AuthorUserId = userId;
            post.AuthorName = user.GetFullName();
            post.AuthorUserName = user.UserName;
            post.DateCreated = DateTime.Now;
            _unitOfWork.BaiPost.Add(post);
            //Process tag 
            if (request.Tags != null && request.Tags.Length > 0)
            {
                foreach (var tagName in request.Tags)
                {
                    var tagSlug = TextHelper.ToUnsignedString(tagName);
                    var tag = await _unitOfWork.Tags.GetBySlug(tagSlug);
                    Guid tagId;
                    if (tag == null)
                    {
                        tagId = Guid.NewGuid();
                        _unitOfWork.Tags.Add(new Tag() { Id = tagId, Name = tagName, Slug = tagSlug });

                    }
                    else
                    {
                        tagId = tag.Id;
                    }
                    await _unitOfWork.BaiPost.AddTagToPost(postId, tagId);
                }
            }
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }
        [HttpPut]
        [Authorize(Posts.Edit)]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] CreateUpdatePostRequest request)
        {
            if (await _unitOfWork.BaiPost.IsSlugAlreadyExisted(request.Slug, id))
            {
                return BadRequest("đã tồn tại slug");
            }
            var post = await _unitOfWork.BaiPost.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            if (post.CategoryId != request.CategoryId)
            {
                var category = await _unitOfWork.PostCategories.GetByIdAsync(request.CategoryId);
                post.CategoryName = category.Name;
                post.CategorySlug = category.Slug;
            }
            _mapper.Map(request, post);
            //Process tag 
            if (request.Tags != null && request.Tags.Length > 0)
            {
                foreach (var tagName in request.Tags)
                {
                    var tagSlug = TextHelper.ToUnsignedString(tagName);
                    var tag = await _unitOfWork.Tags.GetBySlug(tagSlug);
                    Guid tagId;
                    if (tag == null)
                    {
                        tagId = Guid.NewGuid();
                        _unitOfWork.Tags.Add(new Tag() { Id = tagId, Name = tagName, Slug = tagSlug });

                    }
                    else
                    {
                        tagId = tag.Id;
                    }
                    await _unitOfWork.BaiPost.AddTagToPost(id, tagId);
                }
            }
            var res = await _unitOfWork.CompleteAsync();
            return Ok();
        }
        [HttpGet]
        [Route("{id}")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<PostDto>> GetPostById(Guid id)
        {
            var post = await _unitOfWork.BaiPost.GetByIdAsync(id);
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
            var data = await _unitOfWork.BaiPost.GetAllAsync();
            return Ok(data);
        }

        [HttpGet]
        [Route("series-belong/{id}")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<List<SeriesInListDto>>> GetSeriesBelong(Guid id)
        {

            var result = await _unitOfWork.BaiPost.GetAllSeries(id);
            return Ok(result);
        }
        [HttpDelete]
        [Authorize(Posts.Delete)]
        public async Task<IActionResult> DeletePosts([FromQuery] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var post = await _unitOfWork.BaiPost.GetByIdAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                _unitOfWork.BaiPost.Remove(post);
            }
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }
        [HttpGet]
        [Route("paging")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<PageResult<PostInListDto>>> GetPostsPaging(string? keyword, Guid? categoryId,
           int pageIndex, int pageSize = 10)
        {
            var userId = User.GetUserId();
            var result = await _unitOfWork.BaiPost.GetAllPaging(keyword, userId, categoryId, pageIndex, pageSize);
            return Ok(result);
        }
        [HttpGet("approve/{id}")]
        [Authorize(Posts.Approve)]
        public async Task<IActionResult> ApprovePost(Guid id)
        {
            await _unitOfWork.BaiPost.Approve(id, User.GetUserId());
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpGet("approval-submit/{id}")]
        [Authorize(Posts.Edit)]
        public async Task<IActionResult> SendToApprove(Guid id)
        {
            await _unitOfWork.BaiPost.SendToApprove(id, User.GetUserId());
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpPost("return-back/{id}")]
        [Authorize(Posts.Approve)]
        public async Task<IActionResult> ReturnBack(Guid id, [FromBody] ReturnBackRequest model)
        {
            await _unitOfWork.BaiPost.ReturnBack(id, User.GetUserId(), model.Reason);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpGet("return-reason/{id}")]
        [Authorize(Posts.Approve)]
        public async Task<ActionResult<string>> GetReason(Guid id)
        {
            var note = await _unitOfWork.BaiPost.GetReturnReason(id);
            return Ok(note);
        }

        [HttpGet("activity-logs/{id}")]
        [Authorize(Posts.Approve)]
        public async Task<ActionResult<List<PostActivityLogDto>>> GetActivityLogs(Guid id)
        {
            var logs = await _unitOfWork.BaiPost.GetActivityLogs(id);
            return Ok(logs);
        }
        [HttpGet("tags")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<List<string>>> GetAllTags()
        {
            var logs = await _unitOfWork.Tags.GetAllTags();
            return Ok(logs);
        }
        [HttpGet("tags/{id}")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<List<string>>> GetPostTags(Guid id)
        {
            var tagName = await _unitOfWork.BaiPost.GetTagsByPostId(id);
            return Ok(tagName);
        }
    }
}

