using ToDo.ModelViews.ModelView;

namespace ToDo.Core.Managers.Interfaces
{
    public interface IToDoTask
    {
        public TodoRespones GetTasks(int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "");
        public ToDoModelView GetTaskById(int id);
        public ToDoModelView AddTask(UserModel currentUser, ToDoRequest toDoRequest);
        public ToDoModelView UpdateTask(UserModel currentUser, ToDoRequest toDoRequest);
        public void DeleteTask(int id);

    }
}
