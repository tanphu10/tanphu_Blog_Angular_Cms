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
            var posts = await _unitOfWork.BaiPost.GetPostByCategoryPaging(categorySlug, page);
            var category = await _unitOfWork.PostCategories.GetBySlug(categorySlug);
            return View(new PostListByCategoryViewModel()
            {
                Posts = posts,
                Category = category
            }); ;
        }
        [Route("tag/{tagSlug}")]
        public IActionResult ListByTag([FromRoute] string tagLug, [FromQuery] int? page = 1)
        {

            return View();
        }
        [Route("posts/detail/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var posts = await _unitOfWork.BaiPost.GetBySlug(slug);
            var category = await _unitOfWork.PostCategories.GetBySlug(posts.CategorySlug);
            return View(new PostDetailViewModel()
            {
                Posts = posts,
                Category = category
            });
        }
    }
}
