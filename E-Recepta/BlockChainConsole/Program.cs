﻿using System;
using BlockChain;
using BlockChain.utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.IO;
using System.Text.RegularExpressions;

namespace BlockChainConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // BlockChain id representation:
            // 1 - prescriptions
            // 2 - realized prescriptions

           
            Console.WriteLine(NetworkUtils.GetLocalIPAddress());
            Console.WriteLine(NetworkUtils.GetMask());
            Console.WriteLine(NetworkUtils.GetBroadcastAddress(NetworkUtils.GetLocalIPAddress(), NetworkUtils.GetMask()));
            Console.WriteLine(NetworkUtils.GetNetworkAddress(NetworkUtils.GetLocalIPAddress(), NetworkUtils.GetMask()));

            BlockChainHandler blockChainHandler = new BlockChainHandler();
            blockChainHandler.initializeBlockChains();

        //    Console.WriteLine(blockChainHandler.getSizeOfPrescriptions());

            Console.WriteLine("Click to send block");
            Console.ReadKey();
            Console.WriteLine("----------------");
            blockChainHandler.addPrescription("patientId_123asd", "doctorId_321asd", "testJsonData");

            Console.WriteLine(blockChainHandler.getSizeOfPrescriptions());

            Console.WriteLine("Click to exit.");
            Console.ReadKey();

            /*   BlockChain prescriptions = new BlockChain(1);
              Prescription prescription = new Prescription("patientId_123asd",
              "doctorId_123asd", DateTime.Now, "testJsonData");
              prescriptions.Add(prescription);
              prescription = new Prescription("patientId_321asd",
              "doctorId_321asd", DateTime.Now, "testJsonData");
              prescriptions.Add(prescription);
              foreach(Block ps1 in prescriptions.GetAll()) {
                  Console.WriteLine("PreviousHash:" + ps1.GetPreviousHash());
                  Console.WriteLine("Hash:" + ps1.GetHash());
                  Console.WriteLine("TimeStamp: " + ps1.GetTimeStamp());
                  Console.WriteLine("DoctorId: " + ps1.GetPrescription().GetDoctorId());
                  Console.WriteLine("PatientId: " + ps1.GetPrescription().GetPatientId());
                  Console.WriteLine("Data: " + ps1.GetPrescription().GetPrescriptionInfo());
              }
              */
        }
    }
}
