using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        public TaskController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]
        //[Authorize(Permissions.Tasks.Create)]
        public async Task<IActionResult> CreateTasks([FromBody] CreateUpdateTaskRequest request)
        {

            await _unitOfWork.IC_Tasks.CreateTaskWithAssignmentAsync(request);
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
            await _unitOfWork.IC_Tasks.UpdateTaskWithAssignmentAsync(id, request);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpGet("task/{id}")]
        //[Authorize(Permissions.Tasks.View)]
        public async Task<ActionResult<List<Task>>> GetTaskProject(Guid id)
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
        //[Authorize(Permissions.Tasks.View)]
        public async Task<ActionResult<List<TaskInListDto>>> GetAllProjects()
        {

            //var userPermissions = await _permission.UserHasPermissionForProjectAsync();

            var allProjects = await _unitOfWork.IC_Tasks.GetAllAsync();


            //var allowedProjects = allProjects.Where(p => userPermissions.Contains($"Permissions.Projects.{p.Slug}"));

            var Projects = _mapper.Map<List<ProjectInListDto>>(allProjects);
            return Ok(Projects);
        }



    }
}
