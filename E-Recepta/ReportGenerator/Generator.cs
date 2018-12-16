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
    public class Generatorz
    {

        private static BlockchainData blockchainData;
        private static UIData uIData;

        public static void Generate(ReportType reportType, ReportExt reportExt, DateTime begin, DateTime end, int personID, ref BlockChainHandler blockChainHandler)
        {
            WrongDataHandler dataHandler = new WrongDataHandler();
            uIData = new UIData(reportExt, begin, end, personID, reportType);
            blockchainData = new BlockchainData(ref blockChainHandler);
            switch (uIData.ReportType)
            {
                case ReportType.PrescriptionsReport:
                    if (dataHandler.WrongDataNotification(blockchainData.GetPrescriptionListForPatient(uIData.PersonID)))
                        throw new WrongDataHandler("Patient does not have any prescriptions");
                    break;
                case ReportType.SoldMedicamentsReport:
                    if (dataHandler.WrongDataNotification(blockchainData.GetPrescriptionListForPharmacist(uIData.PersonID)))
                        throw new WrongDataHandler("Pharmacist does not have any sold medicaments");
                    break;
                default:
                    throw new NotImplementedException();
            }
            switch (uIData.FileFormat)
            {
                case ReportExt.PDF:
                    GenerateFile generatePDF = new GeneratePDF(uIData.ReportType, blockchainData, uIData.PersonID, uIData.Begin, uIData.End);
                    break;
                case ReportExt.CSV:
                    GenerateFile generateCSV = new GenerateCSV(uIData.ReportType, blockchainData, uIData.PersonID, uIData.Begin, uIData.End);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

