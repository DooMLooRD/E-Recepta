using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator
{
    public class UIData
    {
        private ReportExt fileFormat;
        private DateTime begin;
        private DateTime end;
        private int patientID;
        private ReportType reportType;
        private string path;
        private int pharmacistID;

        public UIData(ReportExt fileFormat, DateTime begin, DateTime end, int patientID, ReportType reportType, string path, int pharmacistID)
        {
            this.fileFormat = fileFormat;
            this.begin = begin;
            this.end = end;
            this.patientID = patientID;
            this.reportType = reportType;
            this.path = path;
            this.pharmacistID = pharmacistID;
        }
        public ReportExt FileFormat { get => fileFormat; set => fileFormat = value; }
        public DateTime Begin { get => begin; set => begin = value; }
        public DateTime End { get => end; set => end = value; }
        public int PatientID { get => patientID; set => patientID = value; }
        public ReportType ReportType { get => reportType; set => reportType = value; }
        public string Path { get => path; set => path = value; }
        public int PharmacistID { get => pharmacistID; set => pharmacistID = value; }
    }
}
