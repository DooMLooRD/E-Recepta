using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockChain.utils;

namespace BlockChain
{
    public class BlockChainHandler
    {
        List<string> availableIpAddresses;

        private BlockChainServer blockChainServer_prescriptions;
        private BlockChainServer blockChainServer_realized;

        private BlockChainClient blockChainClient_prescriptions;
        private BlockChainClient blockChainClient_realized;

        private BlockChain prescriptions;
        private BlockChain realizedPrescriptions;

        public async void initializeBlockChains()
        {
            getOnlineHosts();

            prescriptionBlockChainInit();
            realizedPrescriptionBlockChainInit();
        }

        public void addPrescription(string patientId, string doctorId, string prescriptionInfo)
        {

            if (prescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {
                prescriptions.UpdateBlockChain();
            

            Prescription prescription = new Prescription(patientId,
              doctorId, DateTime.Now, prescriptionInfo);

            prescriptions.Add(prescription);

            } else
            {
                Console.WriteLine("At least 2 peers connected are required to use this method.");
            }
        }

        public int getSizeOfPrescriptions()
        {
            return prescriptions.GetSize();
        }

        private async void getOnlineHosts()
        {
            availableIpAddresses = new List<string>();

            NetworkPing networkPing = new NetworkPing();
            availableIpAddresses = await networkPing.RunPingAsync();
        }

        private async void prescriptionBlockChainInit()
        {
            blockChainClient_prescriptions = new BlockChainClient(NetworkUtils.GetLocalIPAddress().ToString(), "6066");

            prescriptions = new BlockChain(blockChainClient_prescriptions);

            blockChainServer_prescriptions = new BlockChainServer(prescriptions, NetworkUtils.GetLocalIPAddress().ToString(), "6066");
            blockChainServer_prescriptions.Start();

            foreach(string ipAddress in availableIpAddresses)
            {
                    if (!ipAddress.Equals(NetworkUtils.GetLocalIPAddress().ToString()))
                    {

                        blockChainClient_prescriptions.Connect(ipAddress, "6066");
                    }
            }

            if (prescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {
                prescriptions.UpdateBlockChain();
            } else
            {
                Console.WriteLine("At least 2 peers connected are required to use this method.");
            }

        }

        private async void realizedPrescriptionBlockChainInit()
        {
            blockChainClient_realized = new BlockChainClient(NetworkUtils.GetLocalIPAddress().ToString(), "6067");

            realizedPrescriptions = new BlockChain(blockChainClient_realized);

            blockChainServer_realized = new BlockChainServer(realizedPrescriptions, NetworkUtils.GetLocalIPAddress().ToString(), "6067");
            blockChainServer_realized.Start();

            foreach (string ipAddress in availableIpAddresses)
            {

                if (!ipAddress.Equals(NetworkUtils.GetLocalIPAddress().ToString()))
                {
                    blockChainClient_realized.Connect(ipAddress, "6067");
                }
            }

            if(realizedPrescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {
                realizedPrescriptions.UpdateBlockChain();
            }
            else
            {
                Console.WriteLine("At least 2 peers connected are required to use this method.");
            }

        }


    }
}
