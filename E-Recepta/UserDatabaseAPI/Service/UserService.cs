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
        public async Task<IEnumerable<User>> GetAllPatients()
        {
            using (DatabaseUserContext context = new DatabaseUserContext())
            {
                return await context.Users.Where(u => u.Role == "Patient").ToListAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllPharmacists()
        {
            using (DatabaseUserContext context = new DatabaseUserContext())
            {
                return await context.Users.Where(u => u.Role == "Pharmacist").ToListAsync();
            }
        }
    }
}
