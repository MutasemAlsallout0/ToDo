using ToDo.ModelViews.ModelView;

namespace ToDo.Core.Managers.Interfaces
{
    public interface ICommonManager
    {
        public UserModel GetinfoUserFromDb(UserModel userModel);
    }
}
