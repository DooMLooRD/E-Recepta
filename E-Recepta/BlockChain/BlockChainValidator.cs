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
        private int BlockChainVerification;
        public IDictionary<string, bool> verificationAnswers { get;  private set; }

        public BlockChainValidator()
        {
            BlockChainVerification = 0;         
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
                    Console.WriteLine("Hash1 = " + firstBlocks[i].GetHash());
                    Console.WriteLine("Hash2 = " + firstBlocks[i].GenerateHash());
                    Console.WriteLine("Different data first!!!");
                    return false;
                }
                if (!secondsBlocks[i].GetHash().Equals(secondsBlocks[i].GenerateHash())) {
                    Console.WriteLine("Different data second!!!");
                    return false;
                }
                Console.WriteLine("END SEARCHING!!!");
            }
            Console.WriteLine("RETURN TRUE???");
            return true;
        }
        public void giveVerificationAnswers(IDictionary<string,bool> answers)
        {
            int numberOfTrue = 0;
            int numberOfFalse = 0;
            foreach(KeyValuePair<string,bool> valuePair in answers)
            {
                if (valuePair.Value)
                {
                    numberOfTrue++;
                }
                else
                {
                    numberOfFalse++;
                }
            }

            if(numberOfTrue >= numberOfFalse)
            {
                verificationAnswers = answers;
                BlockChainVerification = 1; //1 = true
            }
            else
            {
                verificationAnswers = answers;
                BlockChainVerification = -1; //-1 = false
            }
        }
        public int getVerificationAnswer()
        {
            if(BlockChainVerification == -1)
            {
                BlockChainVerification = 0;
                return -1; //answer == false
            }
            if(BlockChainVerification == 1)
            {
                BlockChainVerification = 0;
                return 1; //answer == true
            }
            return 0; //answer not redy code
        }




    }
}
