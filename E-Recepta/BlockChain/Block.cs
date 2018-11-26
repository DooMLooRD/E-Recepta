using System;
using System.Security.Cryptography;
using System.Text;

namespace EPrescription
{
    class Block
    {
        
        private string hash;
        private string previousHash;
        private DateTime timeStamp;
        private Prescription prescription;
        
        public Block(string previousHash, Prescription prescription) {
            this.timeStamp = DateTime.Now;
            this.previousHash = previousHash;
            this.prescription = prescription;

            GenerateHash();
        }

        public string GetHash() {
            return hash;
        }

        public string GetPreviousHash() {
            return previousHash;
        }

        public DateTime GetTimeStamp() {
            return timeStamp;
        }

        public Prescription GetPrescription() {
            return prescription;
        }

        private void GenerateHash() {
        using (SHA256 sha256Hash = SHA256.Create())
            {
                string prescriptionData;

                if (prescription != null) {
                    prescriptionData = prescription.GetDoctorId() + "::" +
                    prescription.GetPatientId() + "::" +
                    prescription.GetPharmacistId() + "::" +
                    prescription.GetPrescriptionInfo();
                } else {
                    prescriptionData = "empty";
                }

                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(
                previousHash + "::" +
                timeStamp + "::" + 
                prescriptionData
                ));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                this.hash = builder.ToString();
            }
        }

    }
}
