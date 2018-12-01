using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.utils
{
    public class NetworkUtils
    {


        public static string packetSeparator = "?bc_sep?";

        public static IPAddress GetLocalIPAddress()
        {
            return FindNetworkProperty("ipaddress");
        }

        public static IPAddress GetMask()
        {
            return FindNetworkProperty("mask");
        }

        public static List<string> FindAvailableHosts(string ipaddress, string mask)
        {
            List<string> availableHosts = new List<string>();

            return availableHosts;
        }

        public static string SplitPacket(string packet, int index)
        {
            return packet.Split(new string[] { packetSeparator }, StringSplitOptions.None)[index];
        }

        private static IPAddress FindNetworkProperty(string type)
        {
            NetworkInterfaceType networkInterfaceType = NetworkInterfaceType.Ethernet;

            IPAddress output = new IPAddress(0);
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == networkInterfaceType && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (type == "ipaddress")
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                output = ip.Address;
                            }
                        }
                        if (type == "mask")
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                output = ip.IPv4Mask;
                            }
                        }

                    }
                }
            }

            return output;
        }

        public static IPAddress GetBroadcastAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }

        public static IPAddress GetNetworkAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(broadcastAddress);
        }

        public static List<string> GetAllIPBetween(string firstIP, string lastIP)
        {

            List<string> ipList = new List<string>();

            ipList.Add(firstIP);

            int[] numbesplitNetworkIP = firstIP.Split('.').Select(Int32.Parse).ToArray();

            StringBuilder createdLastIp = new StringBuilder();
            while (!lastIP.Equals(createdLastIp.ToString()))
            {
                createdLastIp.Clear();

                if (numbesplitNetworkIP[3] < 255)
                {
                    numbesplitNetworkIP[3]++;
                }
                else if (numbesplitNetworkIP[2] < 255)
                {
                    numbesplitNetworkIP[3] = 0;
                    numbesplitNetworkIP[2]++;
                }
                else if (numbesplitNetworkIP[1] < 255)
                {
                    numbesplitNetworkIP[2] = 0;
                    numbesplitNetworkIP[1]++;
                }
                else if (numbesplitNetworkIP[0] < 255)
                {
                    numbesplitNetworkIP[1] = 0;
                    numbesplitNetworkIP[0]++;
                }

                createdLastIp.Append(numbesplitNetworkIP[0]);
                createdLastIp.Append(".");
                createdLastIp.Append(numbesplitNetworkIP[1]);
                createdLastIp.Append(".");
                createdLastIp.Append(numbesplitNetworkIP[2]);
                createdLastIp.Append(".");
                createdLastIp.Append(numbesplitNetworkIP[3]);

                ipList.Add(createdLastIp.ToString());
            }

            return ipList;
        }

        public static StreamReader GetArpTable()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "arp";
            startInfo.Arguments = "-a";

            Process process = Process.Start(startInfo);

            return process.StandardOutput;
        }
    }
}
