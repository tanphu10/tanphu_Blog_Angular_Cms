using Microsoft.AspNetCore.Mvc;
using TPBlog.Core.Models;

namespace TPBlog.WebApp.Components
{
    public class PagerViewComponent:ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PageResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
