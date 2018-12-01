using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator
{
    public class Prescription
    {
        private string patientName;
        private string patientAddress;
        private int patientID;
        private string prescriptionDescription;
        private DateTime date;
        private DateTime realizationDate;
        private string doctor;
        private int pharmacistID;

        public Prescription(string patientName, string patientAddress, int patientID, string prescriptionDescription, DateTime date, DateTime realizationDate, string doctor, int pharmacistID)
        {
            this.patientName = patientName;
            this.patientAddress = patientAddress;
            this.patientID = patientID;
            this.prescriptionDescription = prescriptionDescription;
            this.date = date;
            this.realizationDate = realizationDate;
            this.doctor = doctor;
            this.pharmacistID = pharmacistID;
        }

        public string PatientName { get => patientName; set => patientName = value; }
        public string PatientAddress { get => patientAddress; set => patientAddress = value; }
        public int PatientID { get => patientID; set => patientID = value; }
        public string PrescriptionDescription { get => prescriptionDescription; set => prescriptionDescription = value; }
        public DateTime Date { get => date; set => date = value; }
        public DateTime RealizationDate { get => realizationDate; set => realizationDate = value; }
        public string Doctor { get => doctor; set => doctor = value; }
        public int PharmacistID { get => pharmacistID; set => pharmacistID = value; }
    }
}
