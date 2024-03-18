using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;
using Microsoft.AspNetCore.Authorization;

namespace TPBlog.Api.Controllers
{

    [Route("api/admin/post")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PostController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost([FromBody] CreateUpdatePostRequest request)
        {

            //khúc này nó ánh xạ qua post để có thể tạo 1 biến chứa tất cả cacs thuộc tính của post
            var post = _mapper.Map<CreateUpdatePostRequest, Post>(request);
            //bước này dùng để thêm data post vào trong cơ sở dữ liệu
            _unitOfWork.BaiPost.Add(post);
            var res = await _unitOfWork.CompleteAsync();
            return res > 0 ? Ok() : BadRequest();
        }
        [HttpPut]
        //[Route("{id}")]
        public async Task<ActionResult> UpdatePost(Guid id, [FromBody] CreateUpdatePostRequest request)
        {
            var post = await _unitOfWork.BaiPost.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            _mapper.Map(request, post);
            var res = await _unitOfWork.CompleteAsync();
            return res > 0 ? Ok() : BadRequest();
        }
        [HttpGet]
        [Route("{id}")]
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
        public async Task<ActionResult<PostDto>> GetAllPost()
        {
            var result = await _unitOfWork.BaiPost.GetAllAsync();
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePosts([FromQuery] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var post = await _unitOfWork.BaiPost.GetByIdAsync(id);

                if (post != null)
                {
                    _unitOfWork.BaiPost.Remove(post);
                    return Ok();
                }
                return NotFound();
            }
            var res = await _unitOfWork.CompleteAsync();
            return res > 0 ? Ok("đã xóa thành công") : BadRequest();
        }
        [HttpGet]
        [Route("paging")]
        public async Task<ActionResult<PageResult<PostInListDto>>> GetPostPaging(string? keyword, Guid? categoryId, int pageIndex, int pagesize = 10)
        {
            var result = await _unitOfWork.BaiPost.GetPagingPostAsync(keyword, categoryId, pageIndex, pagesize);
            return Ok(result);
        }
    }
}

