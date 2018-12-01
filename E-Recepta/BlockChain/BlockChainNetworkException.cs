using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    public class BlockChainNetworkException : Exception
    {
        public BlockChainNetworkException(string message) : base(message)
        {
            
        }
    }
}
