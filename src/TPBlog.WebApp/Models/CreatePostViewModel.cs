using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;

namespace TPBlog.WebApp.Models
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        public required string Title { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public string Content { get; set; }
        public string? ThumbnailImage { get; set; }
        public SelectList? Categories { get; set; }
        public string SeoDescription { get; set; }
    }
}
