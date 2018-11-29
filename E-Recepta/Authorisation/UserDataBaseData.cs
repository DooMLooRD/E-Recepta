using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDatabaseAPI.Service;

namespace Authorisation
{
    class UserDatabaseData
    {
        LoginService loginService;

        public string getPasswordHash(string login, string role)
        {
            return loginService.GetPasswordHash(login, role).Result;
        }

        public void saveLoginAttempt(string username, DateTime loginTime, bool isSuccesful)
        {
            LoginAttemptDTO laDTO = new LoginAttemptDTO { Username = username, LoginTime = loginTime, IsSuccessful = isSuccesful };
            bool x = loginService.AddLoginAttempt(laDTO).Result; //po co mnie ten bool
            return;
        }
    }
}
