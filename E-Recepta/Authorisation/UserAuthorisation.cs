using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorisation
{
    public class UserAuthorisation
    {
        public string CreateSession (string login, string password, string role)
        {
            UserDatabaseData uDBD = new UserDatabaseData();

            string passHash1 = HashGenerator.GetHashFromPassword(password);

            string sessionID = "aaa";

            try
            {
                string passHash2 = uDBD.GetPasswordHash(login, role);
                if (Compare(passHash1, passHash2)) return sessionID;
            }
            catch(ArgumentException e)
            {
                WrongCredentialHandler wch = new WrongCredentialHandler(); //zamiast tego po prostu throw??
                return null;
            }
            return null;
        }

        private bool Compare(string passwordHash1, string passwordHash2)
        {
            return string.Equals(passwordHash1,passwordHash2);
        }
    }
}
