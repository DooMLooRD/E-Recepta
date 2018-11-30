using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    public class Medicine
    {

        [JsonProperty]
        public int id { get; set; }

        [JsonProperty]
        public int amount { get; set; }

        public Medicine(int id, int amount)
        {
            this.id = id;
            this.amount = amount;
        }
    }
}
