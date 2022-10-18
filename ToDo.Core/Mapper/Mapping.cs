using AutoMapper;
using ToDo.Common.Exceptions;
using ToDo.DbModel.Models;
using ToDo.ModelViews.ModelView;

namespace ToDo.Core.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {

            CreateMap<LoginRespones, User>().ReverseMap();
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<ToDoModelView, Todo>().ReverseMap();
            CreateMap<PagedResult<ToDoModelView>, PagedResult<Todo>>().ReverseMap();

        }
    }
}
