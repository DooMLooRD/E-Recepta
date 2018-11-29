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
        public async Task<IEnumerable<User>> GetUsers(string name, string lastName, string pesel, string role, string username)
        {
            using (DatabaseUserContext context = new DatabaseUserContext())
            {
                return await context.Users
                    .Where(u =>
                        (u.Role == "" || u.Role == role)
                        && (u.Name == "" || u.Name == name)
                        && (u.LastName == "" || u.LastName == lastName)
                        && (u.Pesel == "" || u.Pesel == pesel)
                        && (u.Username == "" || u.Username == username)).ToListAsync();
            }
        }
    }
}
