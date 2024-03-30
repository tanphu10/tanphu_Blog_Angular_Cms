using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Data.SeedWorks;
using static TPBlog.Core.SeedWorks.Contants.Permissions;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TagController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<ActionResult> CreateTag([FromBody] TagDto request)
        {

            //khúc này nó ánh xạ qua post để có thể tạo 1 biến chứa tất cả cacs thuộc tính của post
            var post = _mapper.Map<TagDto, Tag>(request);
            //bước này dùng để thêm data post vào trong cơ sở dữ liệu
            _unitOfWork.Tags.Add(post);
            var res = await _unitOfWork.CompleteAsync();
            return res > 0 ? Ok() : BadRequest();
        }
        [HttpGet]
        //[Authorize(Posts.)]

        public async Task<ActionResult<TagDto>> GetAllTagsAsync()
        {
            var data = await _unitOfWork.Tags.GetAllAsync();
            return Ok(data);
        }
        [HttpGet("/detail/{tagid}")]
        public async Task<ActionResult<TagDto>> GetTagById(Guid tagid
            )
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var data = await _unitOfWork.Tags.GetByIdAsync(tagid);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);

        }
        [HttpPut]
        public async Task<ActionResult<TagDto>> UpdateTag([FromBody] TagDto request)
        {
            var checkdata = await _unitOfWork.Tags.GetByIdAsync(request.Id);
            if (checkdata == null)
            {
                return NotFound();
            }
            _mapper.Map(request, checkdata);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok(checkdata) : BadRequest();
        }
    }
}
