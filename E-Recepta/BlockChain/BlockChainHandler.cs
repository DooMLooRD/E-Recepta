﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockChain.utils;

namespace BlockChain
{
    public class BlockChainHandler : IBlockChainHandler
    {
        List<string> availableIpAddresses;

        private BlockChainServer blockChainServer_prescriptions;
        private BlockChainServer blockChainServer_realized;

        private BlockChainClient blockChainClient_prescriptions;
        private BlockChainClient blockChainClient_realized;

        private BlockChain prescriptions;
        private BlockChain realizedPrescriptions;

        public async void InitializeBlockChains()
        {
            GetOnlineHosts();

            PrescriptionBlockChainInit();
            RealizedPrescriptionBlockChainInit();
        }

        public bool AddPrescription(string patientId, string doctorId, ObservableCollection<Medicine> medicines)
        {

            if (prescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {

                Prescription prescription = new Prescription(patientId,
              doctorId, DateTime.Now, DateTime.Now, medicines);

            prescriptions.Add(prescription);
            return true;

            } else
            {
                Console.WriteLine("At least 2 peers connected are required to use this method.");
            }

            return false;
        }

        public ObservableCollection<Prescription> GetAllPrescriptionsByPatient(string patientId)
        {
            if (prescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {
                ObservableCollection<Prescription> prescriptionsCollection = new ObservableCollection<Prescription>();

                List<Block> blocks = prescriptions.GetAll();

                foreach (Block block in blocks)
                {
                    if (block.GetPrescription().patientId.Equals(patientId))
                    {
                        prescriptionsCollection.Add(block.GetPrescription());
                    }
                }

                return prescriptionsCollection;
            }

            return null;
        }

        public ObservableCollection<Prescription> GetAllPrescriptionsByPharmacist(string pharmacistId)
        {
            if (prescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {
                ObservableCollection<Prescription> prescriptionsCollection = new ObservableCollection<Prescription>();

                List<Block> blocks = prescriptions.GetAll();

                foreach (Block block in blocks)
                {
                    if(block.GetPrescription().pharmacistId.Equals(pharmacistId))
                    {
                        prescriptionsCollection.Add(block.GetPrescription());
                    }
                }

                return prescriptionsCollection;
            }

            return null;
        }

        public ObservableCollection<Prescription> GetAllPrescriptionsByDoctor(string doctorId)
        {
            if (prescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {
                ObservableCollection<Prescription> prescriptionsCollection = new ObservableCollection<Prescription>();

                List<Block> blocks = prescriptions.GetAll();

                foreach (Block block in blocks)
                {
                    if (block.GetPrescription().doctorId.Equals(doctorId))
                    {
                        prescriptionsCollection.Add(block.GetPrescription());
                    }
                }

                return prescriptionsCollection;
            }

            return null;
        }

        public ObservableCollection<Prescription> GetAllRealizedPrescriptionsByPatient(string patientId)
        {
            if (realizedPrescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {
                ObservableCollection<Prescription> realizedPrescriptionsCollection = new ObservableCollection<Prescription>();

                List<Block> blocks = realizedPrescriptions.GetAll();

                foreach (Block block in blocks)
                {
                    if (block.GetPrescription().patientId.Equals(patientId))
                    {
                        realizedPrescriptionsCollection.Add(block.GetPrescription());
                    }
                }

                return realizedPrescriptionsCollection;

            }

            return null;
        }

        public ObservableCollection<Prescription> GetAllRealizedPrescriptionsByPharmacist(string pharmacistId)
        {
            if (realizedPrescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {
                ObservableCollection<Prescription> realizedPrescriptionsCollection = new ObservableCollection<Prescription>();

                List<Block> blocks = realizedPrescriptions.GetAll();

                foreach (Block block in blocks)
                {
                    if (block.GetPrescription().pharmacistId.Equals(pharmacistId))
                    {
                        realizedPrescriptionsCollection.Add(block.GetPrescription());
                    }
                }

                return realizedPrescriptionsCollection;
            }

            return null;
        }

        public ObservableCollection<Prescription> GetAllRealizedPrescriptionsByDoctor(string doctorId)
        {
            if (realizedPrescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {
                ObservableCollection<Prescription> realizedPrescriptionsCollection = new ObservableCollection<Prescription>();

                List<Block> blocks = realizedPrescriptions.GetAll();

                foreach (Block block in blocks)
                {
                    if (block.GetPrescription().doctorId.Equals(doctorId))
                    {
                        realizedPrescriptionsCollection.Add(block.GetPrescription());
                    }
                }

                return realizedPrescriptionsCollection;
            }

            return null;
        }

        public bool RealizePrescription(string prescriptionId, string pharmacistId)
        {
            Console.WriteLine("Realization...");

            if (realizedPrescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {

                if (realizedPrescriptions.Find(prescriptionId) == null)
                {
                    Block block = prescriptions.Find(prescriptionId);

                    if(block != null)
                    {
                        block.GetPrescription().pharmacistId = pharmacistId;

                        realizedPrescriptions.Add(block.GetPrescription());

                        return true;
                    }

                }

                Console.WriteLine("Prescription is already realized or is not existing.");
                
                return false;

            }
            else
            {
                Console.WriteLine("At least 2 peers connected are required to use this method.");

                return false;
            }
        }

        public ObservableCollection<Prescription> GetAllPrescriptions()
        {
            if (prescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {

                ObservableCollection<Prescription> prescriptionsCollection = new ObservableCollection<Prescription>();

                List<Block> blocks = prescriptions.GetAll();

                foreach (Block block in blocks)
                {
                    prescriptionsCollection.Add(block.GetPrescription());
                }

                return prescriptionsCollection;

            }

            return null;
        }

        public ObservableCollection<Prescription> GetAllRealizedPrescriptions()
        {
            if (realizedPrescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {

                ObservableCollection<Prescription> realizedPrescriptionsCollection = new ObservableCollection<Prescription>();

                List<Block> blocks = realizedPrescriptions.GetAll();

                foreach (Block block in blocks)
                {
                    realizedPrescriptionsCollection.Add(block.GetPrescription());
                }

                return realizedPrescriptionsCollection;

            }

            return null;
        }


        public int GetNumberOfPrescriptions()
        {
            if (prescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {
                return prescriptions.GetSize() - 1;
            }
            return 0;
        }

        public int GetNumberOfRealizedPrescriptions()
        {
            if (realizedPrescriptions.blockChainClient.GetNumberOfConnectedPeers() >= 2)
            {
                return realizedPrescriptions.GetSize() - 1;
            }
            return 0;
        }

        private async void GetOnlineHosts()
        {
            availableIpAddresses = new List<string>();

            NetworkPing networkPing = new NetworkPing();
            availableIpAddresses = await networkPing.RunPingAsync();
        }

        private async void PrescriptionBlockChainInit()
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

        private async void RealizedPrescriptionBlockChainInit()
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