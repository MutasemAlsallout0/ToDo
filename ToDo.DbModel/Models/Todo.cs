using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ToDo.DbModel.Models
{
    public partial class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public bool Archived { get; set; }

        [Timestamp]
        public DateTime CreatedDate { get; set; }

        [Timestamp]
        public DateTime UpatedDate { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public int IsSigned { get; set; }
    }
}
