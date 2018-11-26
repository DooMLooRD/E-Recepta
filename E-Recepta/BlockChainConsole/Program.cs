using System;
using BlockChain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // BlockChain id representation:
            // 1 - prescriptions
            // 2 - realized prescriptions

            BlockChainNetworkManager blockChainNetworkManager = new BlockChainNetworkManager();
            blockChainNetworkManager.Start();
            blockChainNetworkManager.Connect("ws://127.0.0.1:6066/Blockchain");
            Console.WriteLine("test");
            Console.Read();

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
