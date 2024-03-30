namespace TPBlog.WebApp.Models
{
    public class NavigationViewModel
    {
        public string  Slug { get; set; }
        public string Name { get; set; }
        public List<NavigationViewModel> Children { get; set; } = new List<NavigationViewModel>();
        public bool HasChildren
        {
            get
            {
                return Children.Count > 0;
            }
        }
    }
}
