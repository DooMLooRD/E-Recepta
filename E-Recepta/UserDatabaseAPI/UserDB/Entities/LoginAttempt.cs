using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDatabaseAPI.UserDB.Entities
{
    public class LoginAttempt
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public bool IsSuccessful { get; set; }

        public User User { get; set; }

    }
}
