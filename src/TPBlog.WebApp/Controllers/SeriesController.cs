using Microsoft.AspNetCore.Mvc;
using TPBlog.Core.Models;
using TPBlog.Data.SeedWorks;
using TPBlog.WebApp.Models;

namespace TPBlog.WebApp.Controllers
{
    public class SeriesController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public SeriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //[Route("/series")]
        //public async Task<IActionResult> Index([FromQuery] int page = 1)
        //{
        //    var series = await _unitOfWork.Series.GetAllPaging(string.Empty, page);
        //    return View(series);
        //}

        //[Route("series/detail/{id}")]
        [Route("series/detail/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var post = await _unitOfWork.IC_Series.GetPostsInSeriesPaging(slug);
            var series = await _unitOfWork.IC_Series.GetBySlug(slug);
            return View(new SeriesDetailViewModel()
            {
                Series = series,
                Posts = post,
            });
        }
    }
}
