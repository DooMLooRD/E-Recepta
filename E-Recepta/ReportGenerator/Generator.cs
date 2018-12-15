using BlockChain;
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

        private static BlockchainData blockchainData;
        private static UIData uIData;
        #endregion

        public static void Generate(ReportType reportType, ReportExt reportExt, DateTime begin, DateTime end, int personID, ref BlockChainHandler blockChainHandler)
        {
            WrongDataHandler dataHandler = new WrongDataHandler();
            uIData = new UIData(reportExt, begin, end, personID, reportType);
            blockchainData = new BlockchainData(ref blockChainHandler);
            switch (uIData.FileFormat)
            {
                case ReportExt.PDF:
                    dataHandler.WrongDataNotification(blockchainData.GetPrescriptionListForPatient(uIData.PersonID));
                    GenerateFile generatePDF = new GeneratePDF(uIData.ReportType, blockchainData, uIData.PersonID);
                    break;
                case ReportExt.CSV:
                    GenerateFile generateCSV = new GenerateCSV(uIData.ReportType, blockchainData, uIData.PersonID);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

