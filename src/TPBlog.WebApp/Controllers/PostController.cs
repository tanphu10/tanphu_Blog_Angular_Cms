using Microsoft.AspNetCore.Mvc;
using TPBlog.Data.SeedWorks;
using TPBlog.WebApp.Models;

namespace TPBlog.WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        [Route("posts")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("posts/{categorySlug}")]
        public async Task<IActionResult> ListByCategory([FromRoute] string categorySlug, [FromQuery] int page = 1)
        {
            var posts = await _unitOfWork.IC_Posts.GetPostByCategoryPaging(categorySlug, page);
            var category = await _unitOfWork.IC_PostCategories.GetBySlug(categorySlug);
            return View(new PostListByCategoryViewModel()
            {
                Posts = posts,
                Category = category
            }); ;
        }

        //vì sao ở đây dùng id là tagSlug lại không được 
        [Route("tag/{slug}")]
        public async Task<IActionResult> ListByTag([FromRoute] string slug, [FromQuery] int page = 1)
        {
            var posts = await _unitOfWork.IC_Posts.GetPostByTagPaging(slug, page);
            var tag = await _unitOfWork.IC_Tags.GetBySlug(slug);
            return View(new PostListByTagViewModel()
            {
                Posts = posts,
                Tag = tag
            }); ;
        }
        [Route("posts/detail/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var posts = await _unitOfWork.IC_Posts.GetBySlug(slug);
            var category = await _unitOfWork.IC_PostCategories.GetBySlug(posts.CategorySlug);
            var tags = await _unitOfWork.IC_Posts.GetTagsObjectsByPostId(posts.Id);
            return View(new PostDetailViewModel()
            {
                Posts = posts,
                Category = category,
                Tags=tags
            });
        }
    }
}
