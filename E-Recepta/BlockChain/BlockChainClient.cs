using BlockChain.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace BlockChain
{
    public class BlockChainClient
    {

        private string clientIpAddress;
        private string clientPort;
        private BlockChainValidator blockChainValidator;

        IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();

        public BlockChainClient(string clientIpAddress, string clientPort)
        {
            this.clientIpAddress = clientIpAddress;
            this.clientPort = clientPort;
            this.blockChainValidator = new BlockChainValidator();
        }

        public void Connect(string serverAddress, string port)
        {
            try
            {
                if (!wsDict.ContainsKey("ws://" + serverAddress + ":" + port + "/Blockchain"))
                {
                    Console.WriteLine("[" + wsDict.Count + "]Trying... " + serverAddress + ":" + port);
                    WebSocket ws = new WebSocket($"ws://" + serverAddress + ":" + port + "/Blockchain");

                    // Remove StackTrace
                    ws.Log.Output = (_, __) => { };

                    // Listen messages from server
                    ws.OnMessage += (sender, e) =>
                    {
                        if(NetworkUtils.SplitPacket(e.Data, 0).Equals("packet_verification"))
                        {
                            Console.WriteLine(NetworkUtils.SplitPacket(e.Data, 1).Equals("packet_verification"));
                        }
                    };

                    // Listen connection and delete it from dictionary if connection is closed.
                    ws.OnClose += (sender, e) => {
                        Console.WriteLine("Somebody closed the connection with you.");
                        WebSocket SenderWebSocket = (WebSocket) sender;
                        Close(SenderWebSocket.Url.ToString());
                    };

                    // Show message when error occured.
                    ws.OnError += (sender, e) => {
                    Console.WriteLine("Connection problem.");
                    };

                    ws.Connect();
                    ws.Send("packet_connect?bc_sep?" + clientIpAddress + "?bc_sep?" + clientPort);
                    wsDict.Add(ws.Url.ToString(), ws);
                }
            } catch(Exception ex)
            {
                Console.WriteLine("Connection failed");
            }
        }

        public void SendBlock(Block block)
        {
            foreach (var item in wsDict)
            {
                item.Value.Send("packet_block" + "?bc_sep?" + JsonConvert.SerializeObject(block));
            }
        }

        public void askForVerification(List<Block> blocks)
        {
            foreach (var item in wsDict)
            {
                item.Value.Send("packet_verification" + NetworkUtils.packetSeparator + JsonConvert.SerializeObject(blocks));
            }
        }

        public void Send(string url, string data)
        {
            foreach (var item in wsDict)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        public void Broadcast(string data)
        {
            foreach (var item in wsDict)
            {
                item.Value.Send(data);
            }
        }

        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach (var item in wsDict)
            {
                servers.Add(item.Key);
            }
            return servers;
        }

        public int GetNumberOfConnectedPeers()
        {
            return wsDict.Count;
        }

        public void Close(string url)
        {
            wsDict.Remove(url);
        }

    }
}
