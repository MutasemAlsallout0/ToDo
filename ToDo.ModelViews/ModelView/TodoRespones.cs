using System.Collections.Generic;
using ToDo.Common.Exceptions;

namespace ToDo.ModelViews.ModelView
{
    public class TodoRespones
    {
        public PagedResult<ToDoModelView> Blog { get; set; }
        public Dictionary<int, UserModel> User { get; set; }
    }
}
