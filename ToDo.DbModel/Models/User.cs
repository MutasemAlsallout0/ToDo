using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ToDo.DbModel.Models
{
    public partial class User
    {
        public User()
        {
            Todos = new HashSet<Todo>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsAdmin { get; set; }
        public bool Archived { get; set; }

        [Timestamp]
        public DateTime CreatedDate { get; set; }

        [Timestamp]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpatedDate { get; set; }
        public virtual ICollection<Todo> Todos { get; set; }
    }
}
