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
        private int nFound = 0;

        List<string> availableIpAddresses = new List<string>();

        static object lockObj = new object();

        public async Task<List<string>> RunPingSweep_Async()
        {
            string localIpAddress = NetworkUtils.GetLocalIPAddress().ToString();

            var tasks = new List<Task>();
            var throttler = new SemaphoreSlim(initialCount: 20);

            List<string> ipList = new List<String>();

            StreamReader arpTable = NetworkUtils.GetArpTable();
            Regex regexIp = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            MatchCollection result = regexIp.Matches(arpTable.ReadToEnd());
           
            foreach (Match ipAddress in result)
            {
                ipList.Add(ipAddress.ToString());
            }

            foreach (string ip in ipList)
            {
                System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                var task = PingAndUpdateAsync(p, ip);
                tasks.Add(task);
            }

            return availableIpAddresses;
        }

        private async Task PingAndUpdateAsync(System.Net.NetworkInformation.Ping ping, string ip)
        {
            var reply = await ping.SendPingAsync(ip, timeout);
            
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                lock (lockObj)
                {
                    Console.WriteLine("SUCCESSFULL PING = " + ip);
                    availableIpAddresses.Add(ip);
                }
            }
        }

    }
}
