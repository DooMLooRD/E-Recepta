﻿using BlockChain.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockChain
{
    public class BlockChainServer : WebSocketBehavior
    {
        private WebSocketServer wss = null;
        private BlockChain blockChain;
        private string ipAddress;
        private string port;

        public BlockChainServer(BlockChain blockChain, string ipAddress, string port)
        {
            this.blockChain = blockChain;
            this.ipAddress = ipAddress;
            this.port = port;

            Console.WriteLine(blockChain.GetHashCode());
        }

    public void Start()
    {

        wss = new WebSocketServer($"ws://" + ipAddress + ":" + port + "");
           
        wss.AddWebSocketService<BlockChainServer>("/Blockchain", () => new BlockChainServer(blockChain, ipAddress, port));
        wss.Start();
        wss.Log.Output = (_, __) => { };
        Console.WriteLine($"Started server at ws://" + ipAddress + ":" + port + "");
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        if(NetworkUtils.SplitPacket(e.Data, 0).Equals("packet_connect"))
            {

                Console.WriteLine("Server: Trying to connect my client to another server: " + NetworkUtils.SplitPacket(e.Data, 1) + ":" + NetworkUtils.SplitPacket(e.Data, 2));
                blockChain.blockChainClient.Connect(NetworkUtils.SplitPacket(e.Data, 1), NetworkUtils.SplitPacket(e.Data, 2));
                Send("You are successfully connected to " + ipAddress + ":" + port);
            }

            if (NetworkUtils.SplitPacket(e.Data, 0).Equals("packet_block"))
            {

                bool blockAdded = blockChain.AddForeignBlock(JsonConvert.DeserializeObject<Block>(NetworkUtils.SplitPacket(e.Data, 1)));
                Console.WriteLine("Foregin block added ==" + blockAdded);
                Send("packet_block" + NetworkUtils.packetSeparator + blockAdded + NetworkUtils.packetSeparator + "ws://" + ipAddress + ":" + port + "/Blockchain");
                Console.WriteLine(NetworkUtils.SplitPacket(e.Data, 1));
            }
            
            if (NetworkUtils.SplitPacket(e.Data, 0).Equals("packet_verification"))
            {
                bool isBlockChainValid = BlockChainValidator.Compare(blockChain.GetAllBlocks(), JsonConvert.DeserializeObject<List<Block>>(NetworkUtils.SplitPacket(e.Data, 1)));
                Send("packet_verification" + NetworkUtils.packetSeparator + isBlockChainValid + NetworkUtils.packetSeparator + "ws://" + ipAddress + ":" + port + "/Blockchain");
                Console.WriteLine("Sending isBlockChainValid " + isBlockChainValid);
                
            }

            if (NetworkUtils.SplitPacket(e.Data, 0).Equals("packet_chainrequest"))
            {
                Console.WriteLine("Request of chain");
                Send("packet_chain" + NetworkUtils.packetSeparator + JsonConvert.SerializeObject(blockChain.GetAllBlocks()));
            }
        }

}
}
