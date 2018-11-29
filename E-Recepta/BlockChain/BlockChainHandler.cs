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
        private BlockChainClient blockChainClient_prescriptions;

        private BlockChainServer blockChainServer_realized;
        private BlockChainClient blockChainClient_realized;

        private BlockChain prescriptions;
        private BlockChain realizedPrescriptions;

        public async void initializeBlockChains()
        {
            getOnlineHosts();

            prescriptionBlockChainInit();
            realizedPrescriptionBlockChainInit();
        }

        private async void getOnlineHosts()
        {
            availableIpAddresses = new List<string>();

            NetworkPing networkPing = new NetworkPing();
            availableIpAddresses = await networkPing.RunPingSweep_Async();
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




    }


    }
}
