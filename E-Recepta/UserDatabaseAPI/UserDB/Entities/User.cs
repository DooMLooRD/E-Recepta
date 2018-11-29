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
        public string Username { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MinLength(11), MaxLength(11)]
        public string Pesel { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}
