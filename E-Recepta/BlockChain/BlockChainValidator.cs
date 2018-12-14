using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    class BlockChainValidator
    {

        private static int MAX_NUMBER_OF_COMPARED_BLOCKS = 25;
        private int BlockChainVerificationDone;
        private int BlockChainAddBlockVerificationDone;
        private string OwnerName;
        public IDictionary<string, bool> verificationAnswers { get;  private set; }
        public List<bool> addBlockVerificationAnswers { get; private set; }

        public BlockChainValidator(string OwnerName)
        {
            this.OwnerName = OwnerName;
            BlockChainVerificationDone = 0;
            BlockChainAddBlockVerificationDone = 0;
        }

        public bool Compare(List<Block> ownBlocks, List<Block> gotBlocks)
        {
            if(ownBlocks.Count != gotBlocks.Count)
            {
                TraceingManager.Message(LogMessages.NotEqualsBlockChainsLenghtMessage + OwnerName);
                return false;
            }

            int index = ownBlocks.Count - MAX_NUMBER_OF_COMPARED_BLOCKS - 1;
            if(index < 0) { index = 0; }

            for (int i = index; i < ownBlocks.Count; i++)
            {
                if (!ownBlocks[i].GetHash().Equals(gotBlocks[i].GetHash())) {
                    TraceingManager.Message(LogMessages.NotEqualHashMessage + OwnerName + i);
                    return false;
                }
                if (!ownBlocks[i].GetHash().Equals(gotBlocks[i].GenerateHash())) {
                    TraceingManager.Message(LogMessages.IncorrectHashMessage + OwnerName + i);
                    return false;
                }
                if (!gotBlocks[i].GetHash().Equals(gotBlocks[i].GenerateHash())) {
                    TraceingManager.Message(LogMessages.IncorrectHashMessage + OwnerName + i);
                    return false;
                }
            }
            return true;
        }


        #region BlockChain_BlockChainAddBlock

        public void giveAddBlockVerificationAnswers(List<bool> answers)
        {
            int numberOfTrue = 0;
            int numberOfFalse = 0;
            foreach (bool answer in answers)
            {
                if (answer)
                {
                    numberOfTrue++;
                }
                else
                {
                    numberOfFalse++;
                }
            }

            // Equal or more than 50%
            if (numberOfTrue >= numberOfFalse)
            {
                addBlockVerificationAnswers = answers;
                BlockChainAddBlockVerificationDone = 1; //1 = true
            }
            else
            {
                addBlockVerificationAnswers = answers;
                BlockChainAddBlockVerificationDone = -1; //-1 = false
            }
        }

        public int getAddBlockVerificationAnswer()
        {
            if (BlockChainAddBlockVerificationDone == -1)
            {
                BlockChainAddBlockVerificationDone = 0;
                return -1; //answer == false
            }
            if (BlockChainAddBlockVerificationDone == 1)
            {
                BlockChainAddBlockVerificationDone = 0;
                return 1; //answer == true
            }
            return 0; //answer not redy code
        }

        #endregion

        #region BlockChain_BlockChainUpdate

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

            // Equal or more than 50%
            if(numberOfTrue >= numberOfFalse)
            {
                verificationAnswers = answers;
                BlockChainVerificationDone = 1; //1 = true
            }
            else
            {
                verificationAnswers = answers;
                BlockChainVerificationDone = -1; //-1 = false
            }
        }

        public int getVerificationAnswer()
        {
            if(BlockChainVerificationDone == -1)
            {
                BlockChainVerificationDone = 0;
                return -1; //answer == false
            }
            if(BlockChainVerificationDone == 1)
            {
                BlockChainVerificationDone = 0;
                return 1; //answer == true
            }
            return 0; //answer not redy code
        }

        #endregion




    }
}
