using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace BlockChain
{
    public class Prescription
    {
        [JsonProperty]
        public string prescriptionId { get; set; }

        [JsonProperty]
        public string patientId { get; set; }
  
        [JsonProperty]
        public string doctorId { get; set; }

        [JsonProperty]
        public string pharmacistId { get; set; }

        [JsonProperty]
        public DateTime Date { get; set; }

        [JsonProperty]
        public DateTime ValidSince { get; set; }

        [JsonProperty]
        public ObservableCollection<Medicine> medicines { get; set; }

        public Prescription(string patientId, string doctorId, DateTime date, DateTime validSince, ObservableCollection<Medicine> medicines)
        {
            this.patientId = patientId;
            this.doctorId = doctorId;
            this.Date = date;
            this.ValidSince = validSince;
            this.medicines = medicines;
        }

        public void addMedicine(Medicine medicine)
        {
            medicines.Add(medicine);
        }
    }
}
