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
        public string fileName;
        public ReportType reportType;
        public BlockchainData blockchainData;
        // zastanowilem sie i chyba tak jak teraz musi byc jak masz inny pomysl przekazania ID to zrob on bedzie wiedzial komu przeslac bo sie wybiera który raport zrobic i niezaleznie czy to bedzie patient czy pharmacist
        public int personID;
        public abstract void GeneratePrescriptionReport();
        public abstract void GenerateSoldMedicamentsReport();
    }
}
