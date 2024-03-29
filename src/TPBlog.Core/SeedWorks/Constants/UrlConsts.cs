using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.SeedWorks.Constants
{
    public static class UrlConsts
    {
        public static string Home = "/";
        public static string Posts = "/posts";
        public static string About = "/about";
        public static string Contact = "/contacts";
        public static string PostByCategorySlug = "/posts/{0}";
        public static string PostDetails = "/posts/detail/{0}";
        public static string PostByTagSlug = "/tag/{0}";
        public static string Series = "/series";
        public static string SeriesDetails = "/series/detail/{0}";
        public static string Login = "/login";
        public static string Register = "/register";
        public static string Profile = "/profile";
        public static string Author = "/author/{0}";
        public static string ChangePassword = "/profile/change-password";
        public static string ChangeProfile = "/profile/edit";
        public static string ForgotPassword = "/forgot-password";
        public static string ResetPassword = "/reset-password";




    }
}
