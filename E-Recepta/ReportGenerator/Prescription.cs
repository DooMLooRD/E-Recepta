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
        private int patientID;
        private string prescriptionDescription;
        private DateTime date;
        private DateTime validDate;
        private string doctor;
        private int pharmacistID;

        public Prescription(string patientName, int patientID, string prescriptionDescription, DateTime date, DateTime validDate, string doctor, int pharmacistID)
        {
            this.patientName = patientName;
            this.patientID = patientID;
            this.prescriptionDescription = prescriptionDescription;
            this.date = date;
            this.validDate = validDate;
            this.doctor = doctor;
            this.pharmacistID = pharmacistID;
        }

        public string PatientName { get => patientName; set => patientName = value; }
        public int PatientID { get => patientID; set => patientID = value; }
        public string PrescriptionDescription { get => prescriptionDescription; set => prescriptionDescription = value; }
        public DateTime Date { get => date; set => date = value; }
        public DateTime RealizationDate { get => validDate; set => validDate = value; }
        public string Doctor { get => doctor; set => doctor = value; }
        public int PharmacistID { get => pharmacistID; set => pharmacistID = value; }
    }
}
