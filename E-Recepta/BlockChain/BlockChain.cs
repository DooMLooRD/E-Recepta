using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BlockChain
{
    public delegate void SendNewChain(List<Block> newChain);
    public class BlockChain
    {

        [JsonProperty]
        private List<Block> blocks;

        public BlockChainClient blockChainClient { get; private set; }
        private BlockChainValidator validator;
        private bool ChainRequestSent = false;

        public BlockChain(BlockChainClient blockChainClient)
        {
            this.blockChainClient = blockChainClient;

            blocks = new List<Block>();

            //Creating Genesis Block
            if(blocks.Count == 0) {
                blocks.Add(new Block(null, null));
            }
            validator = new BlockChainValidator();
        }

        public void Add(Prescription prescription) {

            Block block = null;

            do
            {
                UpdateBlockChain();

                Block lastBlock = blocks[blocks.Count - 1];
                block = new Block(lastBlock.GetHash(), prescription);

            } while (CheckAddedBlock(block));

            blocks.Add(block);
        }

        public bool AddForeignBlock(Block block) {
            if(block.GetPreviousHash().Equals(blocks[blocks.Count-1].GetHash()))
            {
                blocks.Add(block);
                return true;
            }

            return false;
        }

        public Block Find(string query) {
            return blocks[0];
        }

        public List<Block> GetAll() {
            
            List<Block> allBlocks = new List<Block>();

            bool ignoreGenesisBlock = true;
            foreach(Block block in blocks) {
                if(ignoreGenesisBlock) {
                    ignoreGenesisBlock = false;
                } else {
                    allBlocks.Add(block);
                }
            }

            return allBlocks;
        }

        public List<Block> GetAllBlocks()
        {
            List<Block> allBlocks = new List<Block>();

            foreach (Block block in blocks)
            {
                allBlocks.Add(block);
            }

            return allBlocks;
        }

        public int GetSize() {
            return blocks.Count;
        }

        public void UpdateBlockChain() {

            blockChainClient.sendNewChainMethod = GiveNewChain;
            while (!CheckCurrentChain())
            {
                foreach (KeyValuePair<string,bool> keyValue in validator.verificationAnswers)
                {
                    if(keyValue.Value == false)
                    {
                        ChainRequestSent = true;
                        blockChainClient.SendRequestOfChain(keyValue.Key);
                        break;
                    }
                }
                while (!ChainRequestSent)
                {
                    //wait for answer
                }
            }
        }

        private bool CheckCurrentChain()
        {
            Console.WriteLine("StartChecking");
            blockChainClient.InitializeVerificationAnswersCollecting(validator.giveVerificationAnswers);
            Console.WriteLine("initialized answers collecting");
            blockChainClient.askForVerification(blocks);
            int validatorAnswer;
            do
            {
                validatorAnswer = validator.getVerificationAnswer();
            }
            while (validatorAnswer == 0); //waiting for answers

            if(validatorAnswer == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private bool CheckAddedBlock(Block block)
        {
            Console.WriteLine("StartCheckingAddedBlock");
            blockChainClient.InitializeAddBlockAnswersCollecting(validator.giveAddBlockVerificationAnswers);
            Console.WriteLine("initialized answers collecting");
            blockChainClient.SendBlock(block);
            int validatorAnswer;
            do
            {
                validatorAnswer = validator.getAddBlockVerificationAnswer();
            }
            while (validatorAnswer == 0); //waiting for answers

            if (validatorAnswer == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GiveNewChain(List<Block> newChain)
        {
            if (ChainRequestSent)
            {
                Console.WriteLine("Blocks = " + blocks.Count + " :: newChain = " + newChain.Count);
                blocks = newChain;
                ChainRequestSent = false;
            }          
        }

    }
}
