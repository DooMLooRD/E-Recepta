using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BlockChain.utils
{
    public class NetworkPing
    {
        private int timeout = 100;

        public List<string> availableIpAddresses = new List<string>();

        static object lockObj = new object();

        public List<Task> RunPingAsync()
        {
            string localIpAddress = NetworkUtils.GetLocalIPAddress().ToString();
            string networkAddress = NetworkUtils.GetNetworkAddress(NetworkUtils.GetLocalIPAddress(), NetworkUtils.GetMask()).ToString();
            string broadcastAddress = NetworkUtils.GetBroadcastAddress(NetworkUtils.GetLocalIPAddress(), NetworkUtils.GetMask()).ToString();

            var tasks = new List<Task>();
            var throttler = new SemaphoreSlim(initialCount: 20);

            List<string> ipList = NetworkUtils.GetAllIPBetween(networkAddress, broadcastAddress);

            foreach (string ip in ipList)
            {
                System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                var task = PingAsync(p, ip);
                tasks.Add(task);
            }

            return tasks;

        }

        public async Task PingAsync(System.Net.NetworkInformation.Ping ping, string ip)
        {
            var reply = await ping.SendPingAsync(ip, timeout);
            
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                lock (lockObj)
                {
                    availableIpAddresses.Add(ip);
                }
            }
        }

    }
}
