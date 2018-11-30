﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public void addPrescription(string patientId, string doctorId, ObservableCollection<Medicine> medicines)
        {

            if (prescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {

                Prescription prescription = new Prescription(patientId,
              doctorId, DateTime.Now, DateTime.Now, medicines);

            prescriptions.Add(prescription);

            } else
            {
                Console.WriteLine("At least 2 peers connected are required to use this method.");
            }
        }

        public bool realizePrescription(string prescriptionId, string pharmacistId)
        {
            Console.WriteLine("Realization...");

            if (realizedPrescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {

                if (realizedPrescriptions.Find(prescriptionId) != null)
                {
                    Block block = prescriptions.Find(prescriptionId);

                    if(block != null)
                    {
                        block.GetPrescription().pharmacistId = pharmacistId;

                        realizedPrescriptions.Add(block.GetPrescription());

                        return true;
                    }

                }

                Console.WriteLine("Prescription is already realized.");
                
                return false;

            }
            else
            {
                Console.WriteLine("At least 2 peers connected are required to use this method.");

                return false;
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

            prescriptions = new BlockChain(blockChainClient_prescriptions, "prescriptionsBlockChain");

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
                Console.WriteLine("Updating prescriptionBlockChain...");
                prescriptions.UpdateBlockChain();
            } else
            {
                Console.WriteLine("At least 2 peers connected are required to use this method.");
            }

        }

        private async void realizedPrescriptionBlockChainInit()
        {
            blockChainClient_realized = new BlockChainClient(NetworkUtils.GetLocalIPAddress().ToString(), "6067");

            realizedPrescriptions = new BlockChain(blockChainClient_realized, "realizedPrescriptionsBlockChain");

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
                Console.WriteLine("Updating realizedPrescriptionBlockChain...");
                realizedPrescriptions.UpdateBlockChain();
            }
            else
            {
                Console.WriteLine("At least 2 peers connected are required to use this method.");
            }

        }


    }
}
