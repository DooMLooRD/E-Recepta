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
        private int personID;
        private ReportType reportType;
        private string path;

        public UIData(ReportExt fileFormat, DateTime begin, DateTime end, int personID, ReportType reportType, string path)
        {
            this.fileFormat = fileFormat;
            this.begin = begin;
            this.end = end;
            this.personID = personID;
            this.reportType = reportType;
            this.path = path;
        }
        public ReportExt FileFormat { get => fileFormat; set => fileFormat = value; }
        public DateTime Begin { get => begin; set => begin = value; }
        public DateTime End { get => end; set => end = value; }
        public int PersonID { get => personID; set => personID = value; }
        public ReportType ReportType { get => reportType; set => reportType = value; }
        public string Path { get => path; set => path = value; }
    }
}
