using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Core.Managers.Interfaces;
using ToDo.ModelViews.ModelView;

namespace ToDo.Controllers
{
    [ApiController]
    [Authorize]
    public class ToDoTaskController : GetInfoUserTokenFromDb
    {
        private readonly IToDoTask _toDoTask;
        public ToDoTaskController(IToDoTask toDoTask)
        {
            _toDoTask = toDoTask;
        }

        [HttpGet]
        [Route("api/ToDoTask/GetTasks")]
        public IActionResult GetTasks(int page = 1,
                                     int pageSize = 10,
                                     string sortColumn = "",
                                     string sortDirection = "ascending",
                                     string searchText = "")
        {
            var result = _toDoTask.GetTasks(page, pageSize, sortColumn, sortDirection, searchText);
            return Ok(result);
        }

        [HttpGet]
        [Route("api/ToDoTask/GetTaskById/{id}")]
        public IActionResult GetTaskById(int id)
        {
            var result = _toDoTask.GetTaskById(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/ToDoTask/AddTask")]
        public IActionResult AddTask(ToDoRequest toDoRequest)
        {
            var result = _toDoTask.AddTask(loggedInUser,toDoRequest);
            return Ok(result);
        }

        [HttpPut]
        [Route("api/ToDoTask/UpdateTask")]
        public IActionResult UpdateTask(ToDoRequest toDoRequest)
        {
            var result = _toDoTask.UpdateTask(loggedInUser, toDoRequest);
            return Ok(result);
        }

        [HttpDelete]
        [Route("api/ToDoTask/DeleteTask")]
        public IActionResult DeleteTask(int id)
        {
            _toDoTask.DeleteTask(id);
            return Ok();
        }
    }
}
