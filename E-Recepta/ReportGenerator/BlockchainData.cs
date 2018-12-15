using BlockChain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator
{
    public class BlockchainData
    {
        private List<Prescription> prescriptions;
        private BlockChainHandler BlockChainHandler;

        public BlockchainData(ref BlockChainHandler blockChainHandler)
        {
            this.prescriptions = new List<Prescription>();
            this.BlockChainHandler = blockChainHandler;
        }

        public List<Prescription> GetPrescriptionListForPatient(int patientID)
        {
            ObservableCollection<BlockChain.Prescription> p = BlockChainHandler.GetAllPrescriptionsByPatient("16");
            List<Prescription> output = new List<Prescription>();
            foreach (var prescription in p)
                output.Add(new Prescription("Pacjent1", "Ulica", patientID, "Opis1", DateTime.Now, DateTime.Now, "Doktor", 10));
            return output;
        }

        public List<Prescription> GetPrescriptionListForPharmacist(int pharmacistID)
        {
            return prescriptions.FindAll(x => x.PharmacistID == pharmacistID);
        }
        // do testow
        public void AddPrescriptionToList(Prescription prescription)
        {
            this.prescriptions.Add(prescription);
        }
    }
}
