using System;
using System.Collections.Generic;

namespace EPrescription
{
    class BlockChain
    {

        private int id;
        private List<Block> blocks;

        public BlockChain(int id)
        {
            this.id = id;
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
