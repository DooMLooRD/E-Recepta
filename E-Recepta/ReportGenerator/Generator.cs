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
        
        private static BlockchainData blockchainData = new BlockchainData(prescriptions);
        #endregion

        public static void Generate(ReportType reportType, ReportExt reportExt, DateTime begin, DateTime end, int personID, String path)
        {
            WrongDataHandler dataHandler = new WrongDataHandler();
            blockchainData.AddPrescriptionToList(prescription1);
            blockchainData.AddPrescriptionToList(prescription2);
            UIData uIData = new UIData(reportExt,begin, end, personID, reportType, path);
            switch (uIData.FileFormat)
            {
                case ReportExt.PDF:
                    //ten wyjątek trzeba złapać i wyslac ui
                    dataHandler.WrongDataNotification(blockchainData.GetPrescriptionListForPatient(uIData.PersonID));
                    GenerateFile generatePDF = new GeneratePDF("report.pdf", uIData.ReportType, blockchainData, uIData.PersonID);
                    break;
                case ReportExt.CSV:
                    GenerateFile generateCSV = new GenerateCSV("report.csv", uIData.ReportType, blockchainData, uIData.PersonID);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

