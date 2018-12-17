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
        public async Task<UserDTO> GetUser(int id)
        {
            using (DatabaseUserContext context = new DatabaseUserContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(n => n.Id == id);
                return user != null ? new UserDTO
                {
                    Id = user.Id,
                    LastName = user.LastName,
                    Name = user.Name,
                    Pesel = user.Pesel,
                    Role = user.Role,
                    Username = user.Username
                } : null;
            }
        }
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
                    .Select(n => new UserDTO
                    {
                        Id = n.Id,
                        LastName = n.LastName,
                        Name = n.Name,
                        Pesel = n.Pesel,
                        Role = n.Role,
                        Username = n.Username
                    }).ToListAsync();
            }
        }
    }
}
