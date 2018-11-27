using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDatabaseAPI.UserDB.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Username { get;set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}
