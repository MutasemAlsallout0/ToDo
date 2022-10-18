using AutoMapper;
using System.Linq;
using ToDo.Common.Exceptions;
using ToDo.Core.Managers.Interfaces;
using ToDo.DbModel.Models;
using ToDo.ModelViews.ModelView;

namespace ToDo.Core.Managers
{
    public class CommonManager : ICommonManager
    {
        private readonly tododbContext _context;
        private readonly IMapper _mapper;

        public CommonManager(tododbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public UserModel GetinfoUserFromDb(UserModel userModel)
        {
            var dbUser = _context.Users
                            .FirstOrDefault(x => x.Id == userModel.Id)
                            ?? throw new ServiceValidationException("Invalid email or password");
            return _mapper.Map<UserModel>(dbUser);
        }


    }
}
