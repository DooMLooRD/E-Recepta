using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ReportGenerator
{
    public class GeneratePDF : GenerateFile
    {
        public GeneratePDF(ReportType reportType, BlockchainData blockchainData, int personID)
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
            Document doc = new Document(PageSize.A4);
            try
            {
                List<Prescription> data = new List<Prescription>();
                data = blockchainData.GetPrescriptionListForPatient(this.personID);
                String t = DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + " " + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".pdf";
                PdfWriter.GetInstance(doc, new FileStream(t, FileMode.Create));
                doc.Open();
                doc.Add(new Paragraph("PatientID: " + data[0].PatientID.ToString()));
                doc.Add(new Paragraph("Name: " + data[0].PatientName));
                doc.Add(new Paragraph("Address: " + data[0].PatientAddress));
                foreach (Prescription prescription in data)
                {
                    doc.Add(new Paragraph("Description: " + prescription.PrescriptionDescription));
                    doc.Add(new Paragraph("Date: " + prescription.Date.ToString()));
                    doc.Add(new Paragraph("Realization date: " + prescription.RealizationDate.ToString()));
                    doc.Add(new Paragraph("Doctor: " + prescription.Doctor));
                    doc.Add(new Paragraph("PharmacistID: " + prescription.PharmacistID.ToString()));
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                doc.Close();
            }
        }

        public override void GenerateSoldMedicamentsReport()
        {
            throw new NotImplementedException();
        }
    }
}
