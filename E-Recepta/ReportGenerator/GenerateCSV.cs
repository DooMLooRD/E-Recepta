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
        public GenerateCSV(string fileName, ReportType reportType)
        {
            this.fileName = fileName;
            this.reportType = reportType;
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
            throw new NotImplementedException();
        }

        public override void GenerateSoldMedicamentsReport()
        {
            throw new NotImplementedException();
        }
    }
}
