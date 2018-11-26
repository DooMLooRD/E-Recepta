using System;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

public class BlockChainNetworkManager : WebSocketBehavior
{
    IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();
    bool chainSynched = false;  
    WebSocketServer wss = null;  
  
    public void Start()  
    {  
        wss = new WebSocketServer($"ws://127.0.0.1:6066");  
        wss.AddWebSocketService<BlockChainNetworkManager>("/Blockchain");  
        wss.Start();  
        Console.WriteLine($"Started server at ws://127.0.0.1:6066");  
    }  
  
    protected override void OnMessage(MessageEventArgs e)  
    {
               Console.WriteLine(e.Data);  
               Send("{1} BlockChain");  
    }

    public void Connect(string url)
    {
        if (!wsDict.ContainsKey(url))
        {
            WebSocket ws = new WebSocket(url);
            ws.OnMessage += (sender, e) =>
            {
                Console.WriteLine(e.Data);
            };
            ws.Connect();
            ws.Send("Hi Server");
            wsDict.Add(url, ws);
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

    public void Close()
    {
        foreach (var item in wsDict)
        {
            item.Value.Close();
        }
    }
}  