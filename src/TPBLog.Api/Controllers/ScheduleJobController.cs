using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TPBlog.Core.Models.Auth;
using TPBlog.Core.Shared.Contracts;

namespace TPBlog.Api.Controllers
{
    [Route("api/scheduled-jobs")]
    [ApiController]
    public class ScheduleJobController : ControllerBase
    {
        private readonly IBackgroundJobService _backgroundJobService;

        public ScheduleJobController(IBackgroundJobService backgroundJobService)
        {
            _backgroundJobService = backgroundJobService;

        }
        /// <summary>
        /// Lên lịch Send Email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("send-email-reminder")]
        public IActionResult SendReminderEmail([FromBody] ReminderDto model)
        {

            var jobId = _backgroundJobService.SendEmailContent(model.email, model.subject, model.emailContent, model.enqueueAt);
            return Ok(jobId);
        }
        [HttpDelete]
        [Route("delete/jobId/{id}")]
        public IActionResult DeleteJobId([Required] string id)
        {
            var result = _backgroundJobService.scheduledJobService.Delete(id);
            return Ok(result);
        }
        [HttpGet]

        public IActionResult GetAllApi()
        {

            var result = _backgroundJobService.GetApiAsync();
            return Ok(result);
        }
    }
}
