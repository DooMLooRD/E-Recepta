using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    interface IBlockChainHandler
    {

        bool AddPrescription(string patientId, string doctorId, ObservableCollection<Medicine> medicines);
        bool RealizePrescription(string prescriptionId, string pharmacistId);

        ObservableCollection<Prescription> GetAllPrescriptionsByPatient(string patientId);
        ObservableCollection<Prescription> GetAllPrescriptionsByPharmacist(string pharmacistId);
        ObservableCollection<Prescription> GetAllPrescriptionsByDoctor(string doctorId);
        ObservableCollection<Prescription> GetAllPrescriptions();

        ObservableCollection<Prescription> GetAllRealizedPrescriptionsByPatient(string patientId);
        ObservableCollection<Prescription> GetAllRealizedPrescriptionsByPharmacist(string pharmacistId);
        ObservableCollection<Prescription> GetAllRealizedPrescriptionsByDoctor(string doctorId);
        ObservableCollection<Prescription> GetAllRealizedPrescriptions();

    }
}
