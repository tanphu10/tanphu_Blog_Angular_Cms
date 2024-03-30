using System.Text.Json.Serialization;

namespace TPBlog.WebApp.Models
{
    public class UploadResponse
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }
    }
}
