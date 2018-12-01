using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorisation
{
    public class UserAuthorisation
    {
        public string CreateSession(string login, string password, string role)
        {
            UserDatabaseData uDBD = new UserDatabaseData();

            string passHash1 = HashGenerator.GetHashFromPassword(password);

            Guid sessionID = Guid.NewGuid();

            try
            {
                string passHash2 = uDBD.GetPasswordHash(login, role);
                if (Compare(passHash1, passHash2))
                {
                    uDBD.SaveLoginAttempt(login, DateTime.Now, true);
                    return sessionID.ToString();
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
