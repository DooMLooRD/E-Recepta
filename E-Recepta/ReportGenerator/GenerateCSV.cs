using BlockChain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDatabaseAPI.Service;

namespace ReportGenerator
{
    public class GenerateCSV : GenerateFile
    {
        public GenerateCSV(ReportType reportType, BlockchainData blockchainData, int personID, DateTime begin, DateTime end)
        {
            this.reportType = reportType;
            this.blockchainData = blockchainData;
            this.personID = personID;
            this.begin = begin;
            this.end = end;
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
            var csv = new StringBuilder();
            try
            {
                List<Prescription> data = new List<Prescription>();
                data = blockchainData.GetPrescriptionListForPatient(this.personID);
                csv.AppendLine("PatientID: " + data[0].PatientID.ToString());
                csv.AppendLine("Name: " + data[0].PatientName);
                csv.AppendLine(begin.Date.Month + "/" + begin.Date.Day + "/" + begin.Date.Year + " - " + end.Date.Month + "/" + end.Date.Day + "/" + end.Date.Year);
                var first = String.Empty;
                var second = String.Empty;
                var third = String.Empty;
                var fourth = String.Empty;
                var fifth = String.Empty;
                var newLine = String.Empty;
                foreach (Prescription prescription in data)
                {
                    if (prescription.Date.Date >= begin.Date && prescription.Date.Date <= end.Date)
                    {
                        fifth = prescription.PrescriptionDescription;
                        first = prescription.Date.ToString();
                        second = prescription.RealizationDate.ToString();
                        fourth = prescription.Doctor;
                        newLine = string.Format("{0},{1},{2},{3}", first, second, fourth, fifth);
                        csv.AppendLine(newLine);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                String t = DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + " " + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".csv";
                File.WriteAllText(t, csv.ToString());
            }
        }

        public override void GenerateSoldMedicamentsReport()
        {
            var csv = new StringBuilder();
            try
            {
                List<Prescription> data = new List<Prescription>();
                var dict = new Dictionary<string, int>();
                String[] s;
                String[] a;
                UserService userService = new UserService();
                data = blockchainData.GetPrescriptionListForPharmacist(this.personID);
                csv.AppendLine("PharmacistID: " + data[0].PharmacistID.ToString());
                csv.AppendLine("Name: " + userService.GetUser(data[0].PharmacistID).Result.Name + " " + userService.GetUser(data[0].PharmacistID).Result.LastName);
                csv.AppendLine(begin.Date.Month + "/" + begin.Date.Day + "/" + begin.Date.Year + " - " + end.Date.Month + "/" + end.Date.Day + "/" + end.Date.Year);

                foreach (Prescription prescription in data)
                {
                    if (prescription.Date.Date >= begin.Date && prescription.Date.Date <= end.Date)
                    {
                        s = prescription.PrescriptionDescription.Split(',');
                        foreach (String str in s)
                        {
                            a = str.Split('*');
                            if (dict.ContainsKey(a[0]))
                                dict[a[0]] += int.Parse(a[1]);
                            else
                                dict[a[0]] = int.Parse(a[1]);
                        }
                    }
                    
                }
                var first = String.Empty;
                var second = String.Empty;
                var newLine = String.Empty;
                foreach (var d in dict.OrderByDescending(key => key.Value))
                {
                    first = d.Value.ToString();
                    second = d.Key;
                    newLine = string.Format("{0},{1}", first, second);
                    csv.AppendLine(newLine);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                String t = DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + " " + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".csv";
                File.WriteAllText(t, csv.ToString());
            }
        }
    }
}
