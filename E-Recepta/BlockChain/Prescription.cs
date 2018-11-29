using System;

namespace BlockChain
{
    public class Prescription
    {
        private string prescriptionId;
        private string patientId;
        private string doctorId;
        private string pharmacistId;
        private DateTime date;
        /*
            PrescriptionInfo is a string in JSON format.
        */
        private string prescriptionInfo;

        public Prescription(string patientId, string doctorId, DateTime date, string prescriptionInfo) {
            this.patientId = patientId;
            this.doctorId = doctorId;
            this.date = date;
            this.prescriptionInfo = prescriptionInfo;
        }

        public string GetPrescriptionId() {
            return prescriptionId;
        }

        public string GetPatientId() {
            return patientId;
        }

        public string GetDoctorId() {
            return doctorId;
        }

        public string GetPharmacistId() {
            return pharmacistId;
        }

        public DateTime GetDate() {
            return date;
        }

        public string GetPrescriptionInfo() {
            return prescriptionInfo;
        }

        public void setPharmacistId(string pharmacistId) {
            this.pharmacistId = pharmacistId;
        }

    }
}
