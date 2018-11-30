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
    public class LoginService
    {
        public async Task<string> GetPasswordHash(string username, string role)
        {
            if (!await IsUserInDatabase(username))
                throw new ArgumentException("User not exist in database");
            if (!await HasUserValidRole(username, role))
                throw new ArgumentException("User has invalid role");
            using (DatabaseUserContext context = new DatabaseUserContext())
            {
                var user = await context.Users.FirstAsync(u => u.Username == username);
                return user.PasswordHash;
            }

        }

        public async Task AddLoginAttempt(LoginAttemptDTO loginAttempt)
        {

            using (DatabaseUserContext context = new DatabaseUserContext())
            {
                context.LoginAttempts.Add(new LoginAttempt()
                {
                    IsSuccessful = loginAttempt.IsSuccessful,
                    UserId = context.Users.First(u => u.Username == loginAttempt.Username).Id,
                    LoginTime = loginAttempt.LoginTime
                });
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<LoginAttemptDTO>> GetAllLoginAttempts(string username)
        {
            using (DatabaseUserContext context = new DatabaseUserContext())
            {
                var loginAttempts = context.LoginAttempts.Where(s => s.User.Username == username).Select(l => new LoginAttemptDTO
                {
                    Username = l.User.Username,
                    IsSuccessful = l.IsSuccessful,
                    LoginTime = l.LoginTime
                });
                return await loginAttempts.ToListAsync();
            }
        }

        private async Task<bool> IsUserInDatabase(string username)
        {
            using (DatabaseUserContext context = new DatabaseUserContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
                return user != null;
            }
        }

        private async Task<bool> HasUserValidRole(string username, string role)
        {
            using (DatabaseUserContext context = new DatabaseUserContext())
            {
                var user = await context.Users.FirstAsync(u => u.Username == username);
                if (user.Role == "Doctor")
                    return role == "Doctor" || role == "Patient";
                if(user.Role == "Pharmacist")
                    return role == "Pharmacist" || role == "Patient";
                return role == "Patient";

            }
        }
    }
}
