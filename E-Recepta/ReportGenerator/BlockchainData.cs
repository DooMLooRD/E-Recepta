using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator
{
    public class BlockchainData
    {
        private List<Prescription> prescriptions;

        public BlockchainData(List<Prescription> prescriptions)
        {
            this.prescriptions = prescriptions;
        }

        public List<Prescription> GetPrescriptionListForPatient(int patientID)
        {
            return prescriptions.FindAll(x => x.PatientID == patientID);
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
