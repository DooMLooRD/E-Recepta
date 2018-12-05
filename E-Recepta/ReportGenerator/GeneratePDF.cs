﻿using System;
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
        public GeneratePDF(string fileName, ReportType reportType, BlockchainData blockchainData, int personID)
        {
            this.fileName = fileName;
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
                PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));
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
                throw new NotImplementedException();
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