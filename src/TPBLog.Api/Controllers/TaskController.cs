using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPBlog.Api.Extensions;
using TPBlog.Api.SignalR;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly NotificationsHub _notificationsHub;

        public TaskController(IUnitOfWork unitOfWork, IMapper mapper, NotificationsHub notificationsHub)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationsHub = notificationsHub;
        }
        [HttpGet]
        [Route("Get-Top-Task-Notifications")]
        public async Task<ActionResult<PageResult<TaskNotificationViewModel>>> GetTopMyTaskNotifications([FromQuery]
          int pageIndex = 1, int pageSize = 10)
        {
            var userId = User.GetUserId();
            var result = await _unitOfWork.IC_Tasks.ListAllTaskUnreadAsync(userId, pageIndex, pageSize);
            return Ok(result);
        }
        [HttpGet]
        [Route("mark-read")]
        public async Task<IActionResult> MarkAsTaskRead([FromQuery] Guid announId)
        {
            var userId = User.GetUserId();
            await _unitOfWork.IC_Tasks.MarkTaskAsReadAsync(userId, announId);
            return Ok();
        }
        [Route("user-annoucements/{id}")]
        [HttpGet]
        public async Task<ActionResult<PageResult<TaskNotificationViewModel>>> UserTaskNotificationAsync(Guid id)
        {
            var announcement = await _unitOfWork.IC_Tasks.GetUserTaskNotificationAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            return Ok(announcement);
        }
        [HttpPost]
        //[Authorize(Permissions.Tasks.Create)]
        public async Task<IActionResult> CreateTasks([FromBody] CreateUpdateTaskRequest request)
        {
            var userId = User.GetUserId();


            await _unitOfWork.IC_Tasks.CreateTaskAsync(userId,request);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
        [HttpPost("assign-task-user")]
        //[Authorize(Permissions.Tasks.Create)]
        public async Task<IActionResult> AssignToUser(Guid taskId,[FromBody] AssignToUserRequest request)
        {

            await _unitOfWork.IC_Tasks.AssignToUserAsync(taskId, request);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
        [HttpPut]
        //[Authorize(Permissions.Tasks.Edit)]
        public async Task<IActionResult> UpdateTasks(Guid id, [FromBody] CreateUpdateTaskRequest request)
        {
            var task = await _unitOfWork.IC_Tasks.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            await _unitOfWork.IC_Tasks.UpdateTaskAsync(id, request);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpGet("task/{id}")]
        //[Authorize(Permissions.Tasks.View)]
        public async Task<ActionResult<TaskDto>> GetTaskProject(Guid id)
        {
            var task = await _unitOfWork.IC_Tasks.GetByIdAsync(id);
            return Ok(task);
        }

        [HttpDelete]
        //[Authorize(Permissions.Tasks.Delete)]
        public async Task<IActionResult> DeleteTask([FromQuery] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var task = await _unitOfWork.IC_Tasks.GetByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                var userTask = await _unitOfWork.IC_Tasks.ValidateUserInTask(id);
                if (userTask)
                {
                    return BadRequest("Existed User");
                }

                _unitOfWork.IC_Tasks.Remove(task);
            }
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpGet]
        [Route("task-paging")]
        //[Authorize(Permissions.Tasks.View)]
        public async Task<ActionResult<PageResult<TaskInListDto>>> GetTaskPaging(string? keyword, DateTime? fromDate, DateTime? toDate, Guid? projectId, int pageIndex, int pageSize = 10)
        {
            var result = await _unitOfWork.IC_Tasks.GetAllPagingAsync(keyword, projectId, fromDate, fromDate, pageIndex, pageSize);
            return Ok(result);
        }
        [HttpGet]
        [Route("task-user-paging")]
        //[Authorize(Permissions.Tasks.View)]
        public async Task<ActionResult<PageResult<TaskInListDto>>> GetUserTaskPaging(string? keyword, DateTime? fromDate, DateTime? toDate, Guid? userId, int pageIndex, int pageSize = 10)
        {
            var result = await _unitOfWork.IC_Tasks.GetUserTaskPagingAsync(keyword, userId, fromDate, fromDate, pageIndex, pageSize);

            return Ok(result);
        }

        [HttpGet]
        [Route("task-all-user")]
        public async Task<ActionResult<TaskInListDto>> GetAllUserTask(Guid userId)
        {
            var task = await _unitOfWork.IC_Tasks.GetAllUserTaskAsync(userId);
            return Ok(task);
        }

        [HttpGet]
        //[Authorize(Permissions.Tasks.View)]
        public async Task<ActionResult<List<TaskInListDto>>> GetAllTasks()
        {

            //var userPermissions = await _permission.UserHasPermissionForProjectAsync();

            var all = await _unitOfWork.IC_Tasks.GetAllAsync();


            //var allowedProjects = allProjects.Where(p => userPermissions.Contains($"Permissions.Projects.{p.Slug}"));

            var tasks = _mapper.Map<List<TaskInListDto>>(all);
            return Ok(tasks);
        }



    }
}
