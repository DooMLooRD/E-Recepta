using BlockChain.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace BlockChain
{
    public delegate void VerificationAnswersGot(IDictionary<string, bool> answersToSend);
    public delegate void AddBlockVerificationAnswersGot(List<bool> answersToSend);
    public class BlockChainClient
    {

        private string clientIpAddress;
        private string clientPort;

        private int numberOfExpectedAnsers;
        private int numberOfAddedExpectedAnswers;
        private VerificationAnswersGot AnswersGotMethod;
        private AddBlockVerificationAnswersGot AddBlockVerificationAnswersMethod;
        public SendNewChain sendNewChainMethod;

        private IDictionary<string, bool> verificationAnswers = new Dictionary<string,bool>();
        private List<bool> addedBlocksAnswers = new List<bool>();
        private IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();

        public BlockChainClient(string clientIpAddress, string clientPort)
        {
            this.clientIpAddress = clientIpAddress;
            this.clientPort = clientPort;
        }

        public void Connect(string serverAddress, string port)
        {
            try
            {
                if (!wsDict.ContainsKey("ws://" + serverAddress + ":" + port + "/Blockchain"))
                {
                    WebSocket ws = new WebSocket($"ws://" + serverAddress + ":" + port + "/Blockchain");

                    // Remove StackTrace
                 //   ws.Log.Output = (_, __) => { };

                    // Listen messages from server
                    ws.OnMessage += (sender, e) =>
                    {
                        if(NetworkUtils.SplitPacket(e.Data, 0).Equals("packet_verification"))
                        {
                            SaveTheVerificationAnswer(NetworkUtils.SplitPacket(e.Data, 1), NetworkUtils.SplitPacket(e.Data, 2));
                        }
                        if (NetworkUtils.SplitPacket(e.Data, 0).Equals("packet_block"))
                        {
                            SaveTheAddBlockVerificationAnswer(NetworkUtils.SplitPacket(e.Data, 1));
                        }
                        if (NetworkUtils.SplitPacket(e.Data, 0).Equals("packet_chain"))
                        {
                            sendNewChainMethod(JsonConvert.DeserializeObject<List<Block>>(NetworkUtils.SplitPacket(e.Data, 1)));
                        }
                    };

                    // Listen connection and delete it from dictionary if connection is closed.
                    ws.OnClose += (sender, e) => {
                        WebSocket SenderWebSocket = (WebSocket) sender;
                        Close(SenderWebSocket.Url.ToString());
                    };

                    // Show message when error occured.
                    ws.OnError += (sender, e) => {
                    };

                    ws.Connect();
                    ws.Send("packet_connect" + NetworkUtils.packetSeparator + clientIpAddress + NetworkUtils.packetSeparator + clientPort);
                    wsDict.Add(ws.Url.ToString(), ws);
                }
            } catch(Exception ex)
            {

            }
        }

        #region BlockChain_AddBlock

        public void SendBlock(Block block)
        {
            foreach (var item in wsDict)
            {
                item.Value.Send("packet_block" + NetworkUtils.packetSeparator + JsonConvert.SerializeObject(block));
            }
        }

        public void InitializeAddBlockAnswersCollecting(AddBlockVerificationAnswersGot WhenAnswersGot)
        {
            int expectedAnswers = wsDict.Count;
            numberOfAddedExpectedAnswers = expectedAnswers;
            addedBlocksAnswers = new List<bool>();
            AddBlockVerificationAnswersMethod = WhenAnswersGot;
        }

        private void SaveTheAddBlockVerificationAnswer(string boolStr)
        {
            addedBlocksAnswers.Add(StringToBool(boolStr));
            if (addedBlocksAnswers.Count >= numberOfAddedExpectedAnswers)
            {
                AddBlockVerificationAnswersMethod(addedBlocksAnswers);
            }
        }

        #endregion

        #region BlockChain_UpdateBlockChain

        public void askForVerification(List<Block> blocks)
        {
            foreach (var item in wsDict)
            {
                item.Value.Send("packet_verification" + NetworkUtils.packetSeparator + JsonConvert.SerializeObject(blocks));
            }
        }

        public void InitializeVerificationAnswersCollecting(VerificationAnswersGot WhenAnswersGot)
        {
            int expectedAnswers = wsDict.Count;
            numberOfExpectedAnsers = expectedAnswers;
            verificationAnswers = new Dictionary<string, bool>();
            AnswersGotMethod = WhenAnswersGot;
        }

        private void SaveTheVerificationAnswer(string boolStr, string adressStr)
        {
            verificationAnswers.Add(adressStr, StringToBool(boolStr));
            if(verificationAnswers.Count >= numberOfExpectedAnsers)
            {
                AnswersGotMethod(verificationAnswers);
            }
        }

        public void SendRequestOfChain(string peerAddress)
        {
            Send(peerAddress, "packet_chainrequest");
        }

        #endregion

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

        private bool StringToBool(string str)
        {
            if(str.Equals("true") || str.Equals("True"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
