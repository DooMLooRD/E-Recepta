using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorisation
{
    public class UserAuthorisation
    {
        public int CreateSession(string login, string password, string role)
        {
            UserDatabaseData uDBD = new UserDatabaseData();

            string passHash1 = HashGenerator.GetHashFromPassword(password);

            try
            {
                ValueTuple<string,int> userData = uDBD.GetPasswordHash(login, role);
                string passHash2 = userData.Item1;
                int sessionID = userData.Item2;
                if (Compare(passHash1, passHash2))
                {
                    uDBD.SaveLoginAttempt(login, DateTime.Now, true);
                    return sessionID;
                }
                else
                {
                    uDBD.SaveLoginAttempt(login, DateTime.Now, false);
                    throw new ArgumentException("Incorrect password");
                }
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

        private bool Compare(string passwordHash1, string passwordHash2)
        {
            return string.Equals(passwordHash1, passwordHash2);
        }
    }
}
