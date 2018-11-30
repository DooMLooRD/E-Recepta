using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BlockChain
{
    public class Block
    {

        [JsonProperty]
        private string hash;

        [JsonProperty]
        private string previousHash;

        [JsonProperty]
        private DateTime timeStamp;

        [JsonProperty]
        private Prescription prescription;
        
        public Block(string previousHash, Prescription prescription) {
            this.timeStamp = DateTime.Now;
            this.previousHash = previousHash;
            this.prescription = prescription;
            this.hash = GenerateHash();
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

        public string GenerateHash() {
        using (SHA256 sha256Hash = SHA256.Create())
            {
                string prescriptionData;

                if (prescription == null && previousHash == null)
                {
                    return "4e7b64b4d5b5a298a1d2b0ce105f9d44510fcd65c8672acec968c84e456c7691";
                }

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
                return builder.ToString();
            }
        }

    }
}
