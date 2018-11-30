using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Authorisation
{
    class HashGenerator
    {
        static public string GetHashFromPassword (string password)
        {
            byte[] tmpSource;
            byte[] tmpHash;
            string hash;

            tmpSource = ASCIIEncoding.ASCII.GetBytes(password);
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

            hash = ASCIIEncoding.ASCII.GetString(tmpHash);

            return hash;
        }
    }
}
