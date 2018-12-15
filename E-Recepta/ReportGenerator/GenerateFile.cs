using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ReportGenerator
{
    public abstract class GenerateFile
    {
        public ReportType reportType;
        public BlockchainData blockchainData;
        public int personID;
        public abstract void GeneratePrescriptionReport();
        public abstract void GenerateSoldMedicamentsReport();
    }
}
