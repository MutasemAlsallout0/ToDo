
using System.ComponentModel.DataAnnotations;

namespace ToDo.ModelViews.ModelView
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
