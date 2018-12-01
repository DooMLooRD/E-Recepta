using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReportGenerator
{
    public class Generator
    {
        //do testowania
        #region Testy
        private static DateTime date1 = new DateTime(2018, 7, 20);
        private static DateTime date2 = new DateTime(2018, 8, 10);
        private static Prescription prescription1 = new Prescription("Pacjent1", "Ulica", 1, "Opis1", date1.Date, DateTime.Now, "Doktor", 10);
        private static Prescription prescription2 = new Prescription("Pacjent2", "Ulica", 2, "Opis2", date2.Date, DateTime.Now, "Doktor", 10);
        private static List<Prescription> prescriptions = new List<Prescription>();
        //path jest niezaimplementowany
        private static UIData uIData = new UIData(ReportExt.PDF, DateTime.Now, DateTime.Now, 1, ReportType.PrescriptionsReport, "path", 10);
        private static BlockchainData blockchainData = new BlockchainData(prescriptions);
        #endregion

        public static void Generate()
        {
            WrongDataHandler dataHandler = new WrongDataHandler();
            blockchainData.AddPrescriptionToList(prescription1);
            blockchainData.AddPrescriptionToList(prescription2);
            switch (uIData.FileFormat)
            {
                case ReportExt.PDF:
                    //ten wyjątek trzeba złapać i wyslac ui
                    dataHandler.WrongDataNotification(blockchainData.GetPrescriptionListForPatient(uIData.PatientID));
                    GenerateFile generatePDF = new GeneratePDF("report.pdf", uIData.ReportType, blockchainData, uIData.PatientID);
                    break;
                case ReportExt.CSV:
                    GenerateFile generateCSV = new GenerateCSV("report.csv", uIData.ReportType);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

