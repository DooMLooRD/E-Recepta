using BlockChain;
using MedicinesDatabase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDatabaseAPI.Service;

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
            String medicines = String.Empty;
            UserService userService = new UserService();
            MedicinesDB medicinesDB = new MedicinesDB();
            var user = userService.GetUser(patientID);
            ObservableCollection<BlockChain.Prescription> p = BlockChainHandler.GetAllPrescriptionsByPatient(patientID.ToString());
            List<Prescription> output = new List<Prescription>();
            foreach (var prescription in p)
            {
                medicines = String.Empty;
                foreach (var med in prescription.medicines)
                {
                    medicines += medicinesDB.SearchMedicineById(med.id.ToString()).Result.ToList()[0].Name + " x " + med.amount + ", ";
                }
                medicines = medicines.Remove(medicines.Length - 1);
                output.Add(new Prescription(user.Result.Name + " " + user.Result.LastName, patientID, medicines, prescription.Date, prescription.ValidSince, userService.GetUser(int.Parse(prescription.doctorId)).Result.Name + " " + userService.GetUser(int.Parse(prescription.doctorId)).Result.LastName, 0));

            }
            return output;
        }

        public List<Prescription> GetPrescriptionListForPharmacist(int pharmacistID)
        {
            String medicines = String.Empty;
            UserService userService = new UserService();
            MedicinesDB medicinesDB = new MedicinesDB();
            ObservableCollection<BlockChain.Prescription> p = BlockChainHandler.GetAllRealizedPrescriptionsByPharmacist(pharmacistID.ToString());
            List<Prescription> output = new List<Prescription>();
            foreach (var prescription in p)
            {
                medicines = String.Empty;
                foreach (var med in prescription.medicines)
                {
                    medicines += medicinesDB.SearchMedicineById(med.id.ToString()).Result.ToList()[0].Name + "*" + med.amount + ",";
                }
                medicines = medicines.Remove(medicines.Length - 1);
                output.Add(new Prescription(userService.GetUser(int.Parse(prescription.patientId)).Result.Name + " " + userService.GetUser(int.Parse(prescription.patientId)).Result.LastName, int.Parse(prescription.patientId), medicines, prescription.Date,prescription.ValidSince, userService.GetUser(int.Parse(prescription.doctorId)).Result.Name + " " + userService.GetUser(int.Parse(prescription.doctorId)).Result.LastName, pharmacistID));
            }
            return output;
        }
    }
}
