using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/Tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TagsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTags([FromBody] TagDto request)
        {
            var post = _mapper.Map<TagDto, Tag>(request);
            _unitOfWork.Tags.Add(post);
            var res = await _unitOfWork.CompleteAsync();
            return res > 0 ? Ok() : BadRequest();
        }
        [HttpGet]
        public async Task<ActionResult<TagDto>> GetAllTag()
        {
            var res = await _unitOfWork.Tags.GetAllAsync();
            return Ok(res);
        }
    }
}
