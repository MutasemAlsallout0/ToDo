using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.ModelViews.ModelView
{
    public class ToDoModelView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string ImageString { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
 
        [Timestamp]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Creator")]
        public int UserId { get; set; }
        public int IsSigned { get; set; }
    }
}
