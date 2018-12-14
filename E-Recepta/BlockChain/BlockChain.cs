using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BlockChain
{
    public delegate void SendNewChain(List<Block> newChain);
    public class BlockChain
    {

        [JsonProperty]
        private List<Block> blocks;

        private String blockChainName;

        public BlockChainClient blockChainClient { get; private set; }
        private BlockChainValidator validator;
        private Thread updateThread;
        private Thread addThread;
        private Prescription prescriptionToAdd;
        private bool ChainRequestSent = false;

        public BlockChain(BlockChainClient blockChainClient, string blockChainName)
        {
            this.blockChainClient = blockChainClient;
            this.blockChainName = blockChainName;
            this.updateThread = new Thread(new ThreadStart(InternalUpdateBlockChain));
            this.addThread = new Thread(new ThreadStart(InternalAdd));

            blocks = BlockChainSerializer.deserialize(blockChainName);

            //Creating Genesis Block
            if (blocks == null)
            {
                blocks = new List<Block>();
                blocks.Add(new Block(null, null));
            }

            
            validator = new BlockChainValidator(blockChainName);
        }
        public bool CompareBlocks(List<Block> gotBlocks)
        {           
            return validator.Compare(blocks, gotBlocks);
        }

        public void InternalAdd() {

            Prescription prescription = prescriptionToAdd;
            Block block = null;
            do
            {
                UpdateBlockChain();

                Block lastBlock = blocks[blocks.Count - 1];
                block = new Block(lastBlock.GetHash(), prescription);

            } while (!CheckAddedBlock(block));

            blocks.Add(block);
            BlockChainSerializer.serialize(blocks, blockChainName);
            prescriptionToAdd = null;
        }
        public void Add(Prescription prescription)
        {
                if (addThread.ThreadState == ThreadState.Running || addThread.ThreadState == ThreadState.WaitSleepJoin)
                {
                    addThread.Join(); //wait until last block add
                }
                if(addThread.ThreadState == ThreadState.Unstarted)
                {
                prescriptionToAdd = prescription;
                addThread = new Thread(new ThreadStart(InternalAdd));
                addThread.Start();
                } else
            {
                while (prescriptionToAdd != null)
                {
                    //waiting
                }
                prescriptionToAdd = prescription;
                addThread = new Thread(new ThreadStart(InternalAdd));
                addThread.Start();
            }

           
        }

        public bool AddForeignBlock(Block block) {
            if(block.GetPreviousHash().Equals(blocks[blocks.Count-1].GetHash()))
            {
                blocks.Add(block);
                BlockChainSerializer.serialize(blocks, blockChainName);
                return true;
            }

            return false;
        }

        public Block Find(string prescriptionId) {
            UpdateBlockChain();

            bool ignoreGenesisBlock = true;
            foreach (Block block in blocks)
            {
                if (ignoreGenesisBlock)
                {
                    ignoreGenesisBlock = false;
                } else {
                    if (block.GetPrescription().prescriptionId.Equals(prescriptionId))
                    {
                        return block;
                    }
                }
            }

            return null;
        }

        public List<Block> GetAll() {
            UpdateBlockChain();

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

        /*
         * Update blockchain isn't allowed here because it will create infinite update loop.
         * This method is used only for comparing blockchains.
        */
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
        public void UpdateBlockChain()
        {
                if (updateThread.ThreadState != ThreadState.Running && updateThread.ThreadState != ThreadState.WaitSleepJoin)
                {
                    try
                    {
                    updateThread = new Thread(new ThreadStart(InternalUpdateBlockChain));
                    updateThread.Start();
                    updateThread.Join();
                    }
                catch (Exception e)
                    {
                        updateThread.Join();
                    }                                        
                }
                else
                {
                    updateThread.Join();
                }
        }

        public void InternalUpdateBlockChain() {
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

            BlockChainSerializer.serialize(blocks, blockChainName);
        }

        private bool CheckCurrentChain()
        {

            blockChainClient.InitializeVerificationAnswersCollecting(validator.giveVerificationAnswers);

              List<Block> sendBlocks = new List<Block>();

            int index = blocks.Count - BlockChainValidator.MAX_NUMBER_OF_COMPARED_BLOCKS - 1;
            if (index < 0) { index = 0; }

            for (int i = index; i < blocks.Count; i++)
            {
                sendBlocks.Add(blocks[i]);
            }

            blockChainClient.askForVerification(sendBlocks);
            int validatorAnswer;
            do
            {
                validatorAnswer = validator.getVerificationAnswer();
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


        private bool CheckAddedBlock(Block block)
        {
            blockChainClient.InitializeAddBlockAnswersCollecting(validator.giveAddBlockVerificationAnswers);
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
                blocks = newChain;
                ChainRequestSent = false;
            }          
        }

    }
}
