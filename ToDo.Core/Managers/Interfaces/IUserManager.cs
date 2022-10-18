
using ToDo.ModelViews.ModelView;

namespace ToDo.Core.Managers.Interfaces
{
    public interface IUserManager
    {
        public LoginRespones Login(LoginRequest userLogin);
        public LoginRespones SignUp(UserRegistration userReg);
        public UserModel UpdateInfoUser(UserModel currentUser, UserModel userModel);
        public void Delete(UserModel currentUser, int id);
    }
}
