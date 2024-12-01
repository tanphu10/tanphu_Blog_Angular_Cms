using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TPBlog.Api.Services.IServices;
using TPBlog.Core.Domain.Identity;
using TPBlog.Core.Models.system;
using TPBlog.Core.SeedWorks.Contants;
using TPBlog.Data;
using System.Reflection;
using TPBlog.Api.Extensions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;


namespace TPBlog.Api.Services
{
    public class PermissionService : IPermissionService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionService(IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;

        }

    
        public async Task<List<string>> UserHasPermissionForProjectAsync()
        {

            var user = _httpContextAccessor.HttpContext?.User;
            var permissionsClaim = user.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value;
            // Kiểm tra nếu HttpContext hoặc User không tồn tại
            // Nếu không có permission claim, trả về null
            if (string.IsNullOrEmpty(permissionsClaim))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<List<string>>(permissionsClaim);
        }
    }
}
