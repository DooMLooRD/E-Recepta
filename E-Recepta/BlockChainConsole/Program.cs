using System;
using BlockChain;
using BlockChain.utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

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

            blockChainHandler.InitializeBlockChains();

        //    Console.WriteLine(blockChainHandler.getSizeOfPrescriptions());

            Console.WriteLine("Click to send block");
            Console.ReadKey();
            Console.WriteLine("----------------");

            ObservableCollection<Medicine> medicines = new ObservableCollection<Medicine>();

            Medicine medicine = new Medicine(2, 5);
            Medicine medicine2 = new Medicine(1, 1);

            medicines.Add(medicine);
            medicines.Add(medicine2);

            blockChainHandler.AddPrescription("patientId_123asd", "doctorId_321asd", medicines);

            Console.WriteLine(blockChainHandler.GetNumberOfPrescriptions());

            Console.ReadKey();

            Console.WriteLine("Realize prescription type id:");
            string prescriptionId = Console.ReadLine();
            blockChainHandler.RealizePrescription(prescriptionId, "1");

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
