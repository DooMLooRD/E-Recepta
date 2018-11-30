using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    class BlockChainValidator
    {

        private static int MAX_NUMBER_OF_COMPARED_BLOCKS = 50;
        private List<bool> verificationCounter;

        public BlockChainValidator()
        {
            verificationCounter = new List<bool>();
        }

        public static bool Compare(List<Block> firstBlocks, List<Block> secondsBlocks)
        {
            if(firstBlocks.Count != secondsBlocks.Count)
            {
                Console.WriteLine(firstBlocks.Count + "==" + secondsBlocks.Count);
                Console.WriteLine("Different sizes!!!");
                return false;
            }

            int index = firstBlocks.Count - MAX_NUMBER_OF_COMPARED_BLOCKS - 1;
            if(index < 0) { index = 0; }

            for (int i = 0; i < firstBlocks.Count; i++)
            {
                Console.WriteLine("SEARCHING!!!");
                if (!firstBlocks[i].GetHash().Equals(secondsBlocks[i].GetHash())) {
                    Console.WriteLine("Different hashes!!!");
                    return false;
                }
                if (!firstBlocks[i].GetHash().Equals(firstBlocks[i].GenerateHash())) {
                    Console.WriteLine("Different data first!!!");
                    return false;
                }
                if (!secondsBlocks[i].GetHash().Equals(firstBlocks[i].GenerateHash())) {
                    Console.WriteLine("Different data second!!!");
                    return false;
                }
                Console.WriteLine("END SEARCHING!!!");
            }

            return true;
        }

        public void addVerficiation(bool validBlockChain)
        {
            verificationCounter.Add(validBlockChain);
        }

        public int getVerificationSize()
        {
            return verificationCounter.Count;
        }

        public void clearVerificationCounter()
        {
            verificationCounter.Clear();
        }




    }
}
