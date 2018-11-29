using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorisation
{
    public class UserAuthorisation
    {
        public string createSession (string login, string password)
        {
            string passHash = HashGenerator.getHashFromPassword(password);

            return null;
        }
    }
}
