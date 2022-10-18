using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;
using ToDo.Common.Exceptions;
using ToDo.Core.Managers.Interfaces;
using ToDo.ModelViews.ModelView;

namespace ToDo.Controllers
{
    public class GetInfoUserTokenFromDb : Controller
    {


        private UserModel _loggedInUser;

        protected UserModel loggedInUser 
        { 
            get 
            {  
                if(_loggedInUser != null) return _loggedInUser;

                Request.Headers.TryGetValue("Authorization", out StringValues Token);

                if (string.IsNullOrEmpty(Token))
                {
                    _loggedInUser = null;
                    return _loggedInUser;
                }

                var claimId = User.Claims.FirstOrDefault(x => x.Type == "Id");

                if (claimId == null || !int.TryParse(claimId.Value, out int id))
                {
                    throw new ServiceValidationException(401, "Invalid or expire token");
                }
                var commonManager = HttpContext.RequestServices.GetService(typeof(ICommonManager)) as ICommonManager;

                _loggedInUser = commonManager.GetinfoUserFromDb(new UserModel { Id = id });
                return _loggedInUser;
            }

        }
        public GetInfoUserTokenFromDb()
        {
        }
    }
}
