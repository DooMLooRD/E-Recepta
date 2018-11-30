using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDatabaseAPI.UserDB;
using UserDatabaseAPI.UserDB.Entities;

namespace UserDatabaseAPI.Service
{
    public class UserService
    {
        public async Task<IEnumerable<UserDTO>> GetUsers(string name, string lastName, string pesel, string role, string username)
        {
            using (DatabaseUserContext context = new DatabaseUserContext())
            {
                return await context.Users
                    .Where(u =>
                        (role == "" || u.Role == role)
                        && (name == "" || u.Name == name)
                        && (lastName == "" || u.LastName == lastName)
                        && (pesel == "" || u.Pesel == pesel)
                        && (username == "" || u.Username == username))
                    .Select(n=> new UserDTO
                    {
                        Id = n.Id,
                        LastName = n.LastName,
                        Name = n.LastName,
                        Pesel = n.Pesel,
                        Role = n.Role,
                        Username = n.Username
                    }).ToListAsync();
            }
        }
    }
}
