using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Core.Managers.Interfaces;
using ToDo.ModelViews.ModelView;

namespace ToDo.Controllers
{
     [ApiController]
     [Authorize]
    public class UserController : GetInfoUserTokenFromDb
    {
        private IUserManager _userManager;
        public UserController(IUserManager userManager)
        {
            _userManager = userManager; 
        }


        [HttpPost]
        [Route("api/user/login")]
        [AllowAnonymous]
        public IActionResult Login(LoginRequest userLogin)
        {
            var res = _userManager.Login(userLogin);
            return Ok(res);
        }

        [HttpPost]
        [Route("api/user/signUp")]
        [AllowAnonymous]
        public IActionResult SignUp(UserRegistration userRegistration)
        {
            var res = _userManager.SignUp(userRegistration);
            return Ok(res);
        }

        [HttpPut]
        [Route("api/user/updateInfoUser")]
        public IActionResult UpdateInfoUser(UserModel userModel)
        {
            var res = _userManager.UpdateInfoUser(loggedInUser, userModel);
            return Ok(res);
        }

        [HttpDelete]
        [Route("api/user/delete")]
        public IActionResult Delete(int id)
        {
            _userManager.Delete(loggedInUser, id);
            return Ok();
        }
    }
}
