using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using TPBlog.Core.ConfigureOptions;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/media")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly MediaSettings _settings;

        public MediaController(IWebHostEnvironment env, IOptions<MediaSettings> settings)
        {
            _hostingEnv = env;
            _settings = settings.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult UploadImage(string type)
        {
            var allowImageTypes = _settings.AllowImageFileTypes?.Split(",");

            var now = DateTime.Now;
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return null;
            }

            var file = files[0];
            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition)?.FileName?.Trim('"');
            if (allowImageTypes?.Any(x => filename?.EndsWith(x, StringComparison.OrdinalIgnoreCase) == true) == false)
            {
                throw new Exception("Không cho phép tải lên file không phải ảnh.");
            }

            var imageFolder = $@"\{_settings.ImageFolder}\images\{type}\{now:MMyyyy}";

            var folder = _hostingEnv.WebRootPath + imageFolder;

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var filePath = Path.Combine(folder, filename);
            using (var fs = global::System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            var path = Path.Combine(imageFolder, filename).Replace(@"\", @"/");
            return Ok(new { path });
        }
        [HttpPost]
        [Route("upload-pdf")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadPdf(string type, IFormFile file)
        {
            // Kiểm tra nếu không có file
            if (file == null || file.Length == 0)
            {
                return BadRequest("File không được upload");
            }

            // Kiểm tra định dạng của file
            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
            {
                return BadRequest("Chỉ File PDF mới được upload");
            }

            var now = DateTime.Now;

            // Đường dẫn lưu trữ tệp
            var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/media/pdf", type, $"{now:MMyyyy}");

            // Tạo thư mục nếu chưa tồn tại
            try
            {
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Không thể tạo thư mục: " + ex.Message);
            }

            // Lấy tên file gốc mà không có phần mở rộng
            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName).ToLower();

            // Thêm thời gian hiện tại vào tên file
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"{originalFileName}_{timestamp}{extension}"; // Tạo tên file mới
            var filePath = Path.Combine(imageFolder, fileName);

            // Lưu tệp vào thư mục đã chỉ định
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Không thể lưu file: " + ex.Message);
            }

            var path = Path.Combine("media/pdf", type, $"{now:MMyyyy}", fileName).Replace(@"\", @"/");
            return Ok(new { path });
        }

    }
}
