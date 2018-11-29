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
        if(e.Data.Split(',')[0].Equals("packet_connect"))
            {
                Console.WriteLine("Server: Trying to connect my client to another server: " + e.Data.Split(',')[1] + ":" + e.Data.Split(',')[2]);
                    blockChain.blockChainClient.Connect(e.Data.Split(',')[1], e.Data.Split(',')[2]);
                Send("You are successfully connected to " + ipAddress + ":" + port);
            }
    }

}
}
