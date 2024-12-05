using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.SeedWorks.Contants
{
    public static class Permissions
    {
        public static class Dashboard
        {
            [Description("Xem dashboard")]
            public const string View = "Permissions.Dashboard.View";
        }
        public static class Tasks
        {
            [Description("Xem Task")]
            public const string View = "Permissions.Tasks.View";
            [Description("Tạo mới Task")]
            public const string Create = "Permissions.Tasks.Create";
            [Description("Sửa Task")]
            public const string Edit = "Permissions.Tasks.Edit";
            [Description("Xóa quyềTaskn")]
            public const string Delete = "Permissions.Tasks.Delete";
        }
        public static class Roles
        {
            [Description("Xem quyền")]
            public const string View = "Permissions.Roles.View";
            [Description("Tạo mới quyền")]
            public const string Create = "Permissions.Roles.Create";
            [Description("Sửa quyền")]
            public const string Edit = "Permissions.Roles.Edit";
            [Description("Xóa quyền")]
            public const string Delete = "Permissions.Roles.Delete";
        }
        public static class Users
        {
            [Description("Xem người dùng")]
            public const string View = "Permissions.Users.View";
            [Description("Tạo người dùng")]
            public const string Create = "Permissions.Users.Create";
            [Description("Sửa người dùng")]
            public const string Edit = "Permissions.Users.Edit";
            [Description("Xóa người dùng")]
            public const string Delete = "Permissions.Users.Delete";
        }
        public static class PostCategories
        {
            [Description("Xem danh mục bài viết")]
            public const string View = "Permissions.PostCategories.View";
            [Description("Tạo danh mục bài viết")]
            public const string Create = "Permissions.PostCategories.Create";
            [Description("Sửa danh mục bài viết")]
            public const string Edit = "Permissions.PostCategories.Edit";
            [Description("Xóa danh mục bài viết")]
            public const string Delete = "Permissions.UsPostCategoriesers.Delete";
        }
        public static class Posts
        {
            [Description("Xem bài viết")]
            public const string View = "Permissions.Posts.View";
            [Description("Tạo bài viết")]
            public const string Create = "Permissions.Posts.Create";
            [Description("Sửa bài viết")]
            public const string Edit = "Permissions.Posts.Edit";
            [Description("Xóa bài viết")]
            public const string Delete = "Permissions.Posts.Delete";
            [Description("Duyệt bài viết")]
            public const string Approve = "Permissions.Posts.Approve";
        }

        public static class Series
        {
            [Description("Xem loạt bài")]
            public const string View = "Permissions.Series.View";
            [Description("Tạo loạt bài")]
            public const string Create = "Permissions.Series.Create";
            [Description("Sửa loạt bài")]
            public const string Edit = "Permissions.Series.Edit";
            [Description("Xóa loạt bài")]
            public const string Delete = "Permissions.Series.Delete";
        }
        public static class Projects
        {
            [Description("Xem dự án")]
            public const string View = "Permissions.Projects.View";
            [Description("Tạo dự án")]
            public const string Create = "Permissions.Projects.Create";
            [Description("Sửa dự án")]
            public const string Edit = "Permissions.Projects.Edit";
            [Description("Xóa dự án")]
            public const string Delete = "Permissions.Projects.Delete";
        }
        public static class Inventories
        {
            [Description("Xem Tồn Kho ")]
            public const string View = "Permissions.Inventories.View";
            [Description("Tạo Tồn Kho")]
            public const string Create = "Permissions.Inventories.Create";
            [Description("Sửa Tồn Kho")]
            public const string Edit = "Permissions.Inventories.Edit";
            [Description("Xóa Tồn Kho")]
            public const string Delete = "Permissions.Inventories.Delete";
        }
        public static class InventoryCategories
        {
            [Description("Xem danh mục tồn kho")]
            public const string View = "Permissions.InventoryCategories.View";
            [Description("Tạo danh mục tồn kho")]
            public const string Create = "Permissions.InventoryCategories.Create";
            [Description("Sửa danh mục tồn kho")]
            public const string Edit = "Permissions.InventoryCategories.Edit";
            [Description("Xóa danh mục tồn kho")]
            public const string Delete = "Permissions.InventoryCategories.Delete";
        }
        public static class Products
        {
            [Description("Xem sản phẩm")]
            public const string View = "Permissions.Products.View";
            [Description("Tạo sản phẩm")]
            public const string Create = "Permissions.Products.Create";
            [Description("Sửa sản phẩm")]
            public const string Edit = "Permissions.Products.Edit";
            [Description("Xóa sản phẩm")]
            public const string Delete = "Permissions.Products.Delete";
        }
        public static class ProductCategories
        {
            [Description("Xem danh mục sản phẩm")]
            public const string View = "Permissions.ProductCategories.View";
            [Description("Tạo danh mục sản phẩm")]
            public const string Create = "Permissions.ProductCategories.Create";
            [Description("Sửa danh mục sản phẩm")]
            public const string Edit = "Permissions.ProductCategories.Edit";
            [Description("Xóa danh mục sản phẩm")]
            public const string Delete = "Permissions.ProductCategories.Delete";
        }
        public static class Announcements
        {
            [Description("Xem thông báo")]
            public const string View = "Permissions.Announcements.View";
            [Description("Tạo thông báo")]
            public const string Create = "Permissions.Announcements.Create";
            [Description("Sửa thông báo")]
            public const string Edit = "Permissions.Announcements.Edit";
            [Description("Xóa thông báo")]
            public const string Delete = "Permissions.Announcements.Delete";
        }
        public static class Royalty
        {
            [Description("Xem nhuận bút")]
            public const string View = "Permissions.Royalty.View";
            [Description("Trả nhuận bút")]
            public const string Pay = "Permissions.Royalty.Pay";

        }
        public static class Admin
        {
            [Description("Vào Trang Admin")]
            public const string View = "Permissions.Admin.View";
        }

    }
}
