using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Shared.Contracts;
using TPBlog.Core.Shared.Services.Email;

namespace TPBlog.Data.Shared.Contracts
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private IScheduleJobService _jobService;
        private ISmtpEmailService _emailService;
        private readonly HttpClient _httpClient;
        public BackgroundJobService(IScheduleJobService jobService, ISmtpEmailService emailService, HttpClient httpClient)
        {
            _jobService = jobService;
            _emailService = emailService;
            _httpClient = httpClient;
        }

        public IScheduleJobService scheduledJobService => _jobService;

        public async Task<List<City>> GetApiAsync()
        {
            string url = "https://api.openweathermap.org/data/2.5/group?id=1580578,1581129,1581297,1581188,1587923&units=metric&appid=91b7466cc755db1a94caf6d86a9c788a";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(responseBody);
                return weatherResponse?.List ?? new List<City>();
            }
            catch (JsonSerializationException jsonEx)
            {
                // Xử lý lỗi phân tích JSON
                Console.WriteLine($"Lỗi phân tích JSON: {jsonEx.Message}");
                return new List<City>();
            }
            catch (HttpRequestException httpEx)
            {
                // Xử lý lỗi HTTP
                Console.WriteLine($"Lỗi yêu cầu HTTP: {httpEx.Message}");
                return new List<City>();
            }
        }
        public string? SendEmailContent(string email, string subject, string emailContent, DateTimeOffset enqueueAt)
        {
            var _enqueueAt = enqueueAt.AddSeconds(30);
            var emailRequest = new MailRequest
            {
                ToAddress = email,
                Body = emailContent,
                Subject = subject
            };
            try
            {

                var jobId = _jobService.Schedule(() => _emailService.SendEmail(emailRequest), _enqueueAt);
                return jobId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
