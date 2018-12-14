using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlockChain;

namespace BlockChainTesterConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start v1");
            BlockChainHandler handler = new BlockChainHandler();
            Task.Run(() => handler.InitializeBlockChains());
            Console.WriteLine("Pressa any button to start the threads tests");
            Console.ReadKey();
            //Adding prescription//
            Medicine medicine = new Medicine(3, 3);
            ObservableCollection<Medicine> collection = new ObservableCollection<Medicine>();
            collection.Add(medicine);
            Task.Run(() => handler.GetAllPrescriptions());
            Task.Run(() => handler.GetAllPrescriptions());
            Task.Run(() => handler.GetAllPrescriptions());
            Task.Run(() => handler.GetAllPrescriptions());
            Task.Run(() => handler.GetAllPrescriptions());
            Console.WriteLine("Press any button to exit...");
            Console.ReadKey();
        }
    }
}
