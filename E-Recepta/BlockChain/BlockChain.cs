using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BlockChain
{
    public class BlockChain
    {
        [JsonProperty]
        private List<Block> blocks;

        public BlockChainClient blockChainClient { get; private set; }

        public BlockChain(BlockChainClient blockChainClient)
        {
            this.blockChainClient = blockChainClient;

            blocks = new List<Block>();

            //Creating Genesis Block
            if(blocks.Count == 0) {
                blocks.Add(new Block(null, null));
            }
        }

        public void Add(Prescription prescription) {
            UpdateBlockChain();

            Block lastBlock = blocks[blocks.Count-1];
            Block block = new Block(lastBlock.GetHash(), prescription);
            Console.WriteLine(JsonConvert.SerializeObject(block));

            blockChainClient.SendBlock(block);

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
            blockChainClient.askForVerification(blocks);
        }

    }
}
