using BlockChain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator
{
    public class GenerateCSV : GenerateFile
    {
        public GenerateCSV(ReportType reportType, BlockchainData blockchainData, int personID)
        {
            this.reportType = reportType;
            this.blockchainData = blockchainData;
            this.personID = personID;
            switch (reportType)
            {
                case ReportType.PrescriptionsReport:
                    GeneratePrescriptionReport();
                    break;
                case ReportType.SoldMedicamentsReport:
                    GenerateSoldMedicamentsReport();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void GeneratePrescriptionReport()
        {
            var csv = new StringBuilder();
            try
            {
                List<Prescription> data = new List<Prescription>();
                data = blockchainData.GetPrescriptionListForPatient(this.personID);
                csv.AppendLine("PatientID: " + data[0].PatientID.ToString());
                csv.AppendLine("Name: " + data[0].PatientName);
                csv.AppendLine("Address: " + data[0].PatientAddress);
                var first = String.Empty;
                var second = String.Empty;
                var third = String.Empty;
                var fourth = String.Empty;
                var fifth = String.Empty;
                var newLine = String.Empty;
                foreach (Prescription prescription in data)
                {
                    first = prescription.PrescriptionDescription;
                    second = prescription.Date.ToString();
                    third = prescription.RealizationDate.ToString();
                    fourth = prescription.Doctor;
                    fifth = prescription.PharmacistID.ToString();
                    newLine = string.Format("{0},{1},{2},{3},{4}", first, second, third, fourth, fifth);
                    csv.AppendLine(newLine);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                String t = DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + " " + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".csv";
                File.WriteAllText(t, csv.ToString());
            }
        }

        public override void GenerateSoldMedicamentsReport()
        {
            throw new NotImplementedException();
        }
    }
}
