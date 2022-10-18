using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
 
using ToDo.Common.Exceptions;
using ToDo.Common.Helper;
using ToDo.Core.Managers.Interfaces;
using ToDo.DbModel.Models;
using ToDo.ModelViews.ModelView;

namespace ToDo.Core.Managers
{
    public class ToDoTask : IToDoTask
    {
        private readonly tododbContext _context;
        private readonly IMapper _mapper;
 
        public ToDoTask(tododbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
             
        }

        public TodoRespones GetTasks(int page = 1,
                                     int pageSize = 10,
                                     string sortColumn = "",
                                     string sortDirection = "ascending",
                                     string searchText = "")
        {
            var quereyResult = _context.Todos.Where(x => string.IsNullOrWhiteSpace(searchText)
                                            || (x.Title.Contains(searchText)
                                                || x.Content.Contains(searchText)));

            if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.ToLower().Equals("ascending"))
            {
                quereyResult = quereyResult.OrderBy(sortColumn);
            }
            else if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.ToLower().Equals("descending"))
            {
                quereyResult = quereyResult.OrderByDescending(sortColumn);
            }

            var res = quereyResult.GetPaged(page, pageSize);

            var userIds = res.Data
                            .Select(x => x.UserId)
                            .Distinct()
                            .ToList();

            var users = _context.Users
                            .Where(x => userIds.Contains(x.Id))
                            .ToDictionary(x => x.Id, a => _mapper.Map<UserModel>(a));
            var data = new TodoRespones
            {
                Blog = _mapper.Map<PagedResult<ToDoModelView>>(res),
                User = users,
            };
            data.Blog.Sortable.Add("Title", "Title");
            data.Blog.Sortable.Add("CreatedDate", "Created Date");

            return data;
        }
        public ToDoModelView GetTaskById(int id)
        {
            var dbTask = _context.Todos
                     .FirstOrDefault(t => t.Id == id)
                     ?? throw new ServiceValidationException("Invlid blog id Task");
            return _mapper.Map<ToDoModelView>(dbTask);
        }
        public ToDoModelView AddTask(UserModel currentUser, ToDoRequest toDoRequest)
        {

            

            var url = "";
            if (!string.IsNullOrWhiteSpace(toDoRequest.ImageString))
            {
                url = Helper.SaveImage(toDoRequest.ImageString, "TasksImages");
            }
            var todo = new Todo
            {
                Title = toDoRequest.Title,
                Content = toDoRequest.Content,
                
                UserId = currentUser.Id
            };    

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseUrl = "https://localhost:44338";
                todo.Image = $@"{baseUrl}/api/tasks?filename={url}";
            }

            if (currentUser.IsAdmin)
            {
                todo.IsSigned = toDoRequest.IsSigned;
            }
            else
            {
                todo.IsSigned = currentUser.Id;
            }

            _ = _context.Todos.Add(todo).Entity;
            _context.SaveChanges();
            return _mapper.Map<ToDoModelView>(todo);
        }
        public ToDoModelView UpdateTask(UserModel currentUser, ToDoRequest toDoRequest)
        {
            var dbTask = _context.Todos
                                    .FirstOrDefault(x => x.Id == toDoRequest.Id)
                                    ?? throw new ServiceValidationException("Invlid Task id recived"); ;
                 
            dbTask.Title = toDoRequest.Title;
            dbTask.Content = toDoRequest.Content;
            dbTask.IsRead = toDoRequest.IsRead;

            var url = "";

            if (!string.IsNullOrWhiteSpace(toDoRequest.ImageString))
            {
                url = Helper.SaveImage(toDoRequest.ImageString, "TasksImages");
            }

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseUrl = "https://localhost:44338";
                dbTask.Image = $@"{baseUrl}/api/tasks?filename={url}";
            }

            _context.SaveChanges();
            return _mapper.Map<ToDoModelView>(dbTask);
        }
        public void DeleteTask(int id)
        {
            var dbTask = _context.Todos
                                 .FirstOrDefault(t => t.Id == id)
                                 ?? throw new ServiceValidationException("Invlid blog id Task");
            dbTask.Archived = true;
            _context.SaveChanges();

        }


    }
}
