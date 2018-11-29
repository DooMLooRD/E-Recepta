using System;
using System.Collections.Generic;

namespace BlockChain
{
    public class BlockChain
    {

        private int id;
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
            Block lastBlock = blocks[blocks.Count-1];
            
            Block block = new Block(lastBlock.GetHash(), prescription);
            blocks.Add(block);
        }

        public void AddForeignBlock(Block block, BlockChain blockChain) {

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

        public int GetSize() {
            return blocks.Count;
        }

        public int GetId() {
            return this.id;
        }

        private void UpdateBlockChain() {

        }
    }
}
