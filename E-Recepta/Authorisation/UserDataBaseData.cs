using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDatabaseAPI.;
using UserDatabaseAPI.Service;

namespace Authorisation
{
    class UserDatabaseData
    {
        LoginService loginService = new LoginService();

        public string GetPasswordHash(string login, string role)
        {
            return loginService.GetPasswordHash(login, role).Result;
        }

        public async void SaveLoginAttempt(string username, DateTime loginTime, bool isSuccesful)
        {
            LoginAttemptDTO laDTO = new LoginAttemptDTO { Username = username, LoginTime = loginTime, IsSuccessful = isSuccesful };
            //bool x = loginService.AddLoginAttempt(laDTO).Result; //niepotrzebny bool
            await loginService.AddLoginAttempt(laDTO);
            return;
        }
    }
}
