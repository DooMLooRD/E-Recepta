using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDatabaseAPI.Service
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; } = "";
        public string Pesel { get; set; }
        public string Role { get; set; }
    }
}
