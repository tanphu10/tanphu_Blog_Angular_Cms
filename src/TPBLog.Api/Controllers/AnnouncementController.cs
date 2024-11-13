using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TPBlog.Api.Extensions;
using TPBlog.Api.Services.IServices;
using TPBlog.Api.SignalR;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/annoucement")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly NotificationsHub _notificationsHub;

        private readonly IMapper _mapper;
        public AnnouncementController(IAnnouncementService announcementService, IUnitOfWork unitOfWork, IMapper mapper, NotificationsHub notificationsHub)
        {
            _announcementService = announcementService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationsHub = notificationsHub;
        }

        [HttpGet]
        [Route("mark-read")]
        public async Task<IActionResult> MarkAsRead([FromQuery] int announId)
        {
            var userId = User.GetUserId();
            await _announcementService.MarkAsRead(userId, announId);
            return Ok();
        }
        [HttpDelete(Name = "Delete")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAnnouncementById([Required] int[] ids)
        {
            var userId = User.GetUserId();
            foreach (var id in ids)
            {
                var item = await _unitOfWork.Announcements.GetByIdAsync(id);
                var item2 = _unitOfWork.AnnouncementUsers.Find(x => x.AnnouncementId == id && x.UserId == userId).FirstOrDefault();
                //var product = _mapper.Map<>(item);
                if (item == null)
                {
                    return NotFound();
                }
                _unitOfWork.Announcements.Remove(item);
                if (item2 == null)
                {
                    return NotFound();
                }
                _unitOfWork.AnnouncementUsers.Remove(item2);
                await _unitOfWork.CompleteAsync();
            }
            return NoContent();
        }
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateAnnouncementRequest model)
        {
            var newAnnoun = new Announcement
            {
                Content = model.Content,
                Status = model.Status,
                Title = model.Title,
                DateCreated = DateTime.Now,
                UserId = User.GetUserId()
            };

            await _announcementService.CreateAsync(newAnnoun);  // đảm bảo CreateAsync là async và lưu vào DB

            if (newAnnoun.UserId != null)
            {
                await _unitOfWork.AnnouncementUsers.Add(new AnnouncementUser()
                {
                    AnnouncementId = newAnnoun.Id,
                    UserId = newAnnoun.UserId,
                    HasRead = false
                });
            }
            await _unitOfWork.CompleteAsync();
            var announ = await _unitOfWork.Announcements.GetByIdAsync(newAnnoun.Id);

            //push notification
            if (announ == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<AnnouncementViewModel>(announ);
            _notificationsHub.PushToAllUsers(result);

            return Ok(result);
        }
        [Route("detail/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(AnnouncementViewModel), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AnnouncementViewModel>> DetailsAsync(int id)
        {

            var announcement = await _unitOfWork.Announcements.GetByIdAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<AnnouncementViewModel>(announcement);
            return Ok(result);
        }
        [HttpGet]
        [Route("Get-All-Paging")]
        public async Task<ActionResult<PageResult<AnnouncementViewModel>>> GetNotificationPaging([FromQuery]
           int pageIndex = 1, int pageSize = 10)
        {
            var result = await _unitOfWork.Announcements.GetAllPaging(pageIndex, pageSize);

            return Ok(result);
        }
        [HttpGet]
        [Route("Get-Top-Announcement")]
        public async Task<ActionResult<PageResult<Announcement>>> GetTopMyAnnouncement([FromQuery]
          int pageIndex = 1, int pageSize = 10)
        {
            var userId = User.GetUserId();
            var result = await _unitOfWork.Announcements.ListAllUnread(userId,pageIndex, pageSize);
            return Ok(result);
        }
    }
}
