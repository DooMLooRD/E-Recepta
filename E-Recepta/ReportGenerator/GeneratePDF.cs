using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data;
using System.Diagnostics;
using UserDatabaseAPI.Service;

namespace ReportGenerator
{
    public class GeneratePDF : GenerateFile
    {
        public GeneratePDF(ReportType reportType, BlockchainData blockchainData, int personID, DateTime begin, DateTime end)
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
        private DataTable MakeDataTable(List<Prescription> data)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Date");
            table.Columns.Add("Valid date");
            table.Columns.Add("Doctor");
            table.Columns.Add("Description");

            foreach (Prescription prescription in data)
            {
                if (prescription.Date.Date >= begin.Date && prescription.Date.Date <= end.Date)
                {
                    table.Rows.Add(prescription.Date.ToString(), prescription.RealizationDate.ToString(), prescription.Doctor, prescription.PrescriptionDescription.ToString());
                }  
            }
            return table;
        }

        private DataTable MakeSoldTable(Dictionary<string, int> data)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Amount");
            table.Columns.Add("Name");

            foreach (var d in data.OrderByDescending(key => key.Value))
            {
                table.Rows.Add(d.Value.ToString(), d.Key);
            }
            return table;
        }

        public override void GeneratePrescriptionReport()
        {
            Document doc = new Document(PageSize.A4);
            try
            {
                List<Prescription> data = new List<Prescription>(blockchainData.GetPrescriptionListForPatient(this.personID));
                DataTable dtblTable = new DataTable();
                dtblTable = MakeDataTable(data);
                String t = DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + " " + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".pdf";
                PdfWriter.GetInstance(doc, new FileStream(t, FileMode.Create));

                doc.Open();
                //var bfntHead = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED, 16);
                BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.CACHED);
                Font fntHead = new Font(bfntHead, 16, 1, BaseColor.BLACK);
                Font fntE = new Font(bfntHead, 20, 1, BaseColor.BLACK);
                Paragraph prgHeading = new Paragraph();
                prgHeading.Alignment = Element.ALIGN_CENTER;
                prgHeading.Add(new Chunk("E-Recepty\n\n", fntE));
                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                doc.Add(line);
                prgHeading.Add(new Chunk("Prescription report for ", fntHead));
                prgHeading.Add(new Chunk(data[0].PatientName + " ID: " + data[0].PatientID.ToString() + "\n", fntHead));
                //prgHeading.Add(new Chunk("Address: " + data[0].PatientAddress, fntHead));
                doc.Add(prgHeading);

                Paragraph prgGeneration = new Paragraph();
                BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED);
                Font fntAuthor = new Font(btnAuthor, 10, 2, BaseColor.GRAY);
                prgGeneration.Alignment = Element.ALIGN_RIGHT;
                prgGeneration.Add(new Chunk(begin.Date.Month + "/" + begin.Date.Day + "/" + begin.Date.Year + " - " + end.Date.Month + "/" + end.Date.Day + "/" + end.Date.Year, fntAuthor));
                prgGeneration.Add(new Chunk("\nDate: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), fntAuthor));
                doc.Add(prgGeneration);

                Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                doc.Add(p);
                doc.Add(new Chunk("\n", fntHead));

                PdfPTable table = new PdfPTable(dtblTable.Columns.Count);
                BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED);
                Font fntColumnHeader = new Font(btnColumnHeader, 10, 1, BaseColor.WHITE);
                for (int i = 0; i < dtblTable.Columns.Count; i++)
                {
                    PdfPCell cell = new PdfPCell();
                    cell.BackgroundColor = BaseColor.DARK_GRAY;
                    cell.AddElement(new Chunk(dtblTable.Columns[i].ColumnName.ToUpper(), fntColumnHeader));
                    table.AddCell(cell);
                }

                for (int i = 0; i < dtblTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dtblTable.Columns.Count; j++)
                    {
                        table.AddCell(dtblTable.Rows[i][j].ToString());
                    }
                }
                doc.Add(table);
            }
            catch (Exception ex)
            {
                //throw new NotImplementedException();
            }
            finally
            {
                doc.Close();
            }
        }

        public override void GenerateSoldMedicamentsReport()
        {
            Document doc = new Document(PageSize.A4);
            try
            {
                List<Prescription> data = new List<Prescription>();
                var dict = new Dictionary<string, int>();
                String[] s;
                String[] a;
                UserService userService = new UserService();
                data = blockchainData.GetPrescriptionListForPharmacist(this.personID);
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

                DataTable dtblTable = new DataTable();
                dtblTable = MakeSoldTable(dict);
                String t = DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + " " + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".pdf";
                PdfWriter.GetInstance(doc, new FileStream(t, FileMode.Create));

                doc.Open();
                //var bfntHead = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED, 16);
                BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.CACHED);
                Font fntHead = new Font(bfntHead, 16, 1, BaseColor.BLACK);
                Font fntE = new Font(bfntHead, 20, 1, BaseColor.BLACK);
                Paragraph prgHeading = new Paragraph();
                prgHeading.Alignment = Element.ALIGN_CENTER;
                prgHeading.Add(new Chunk("E-Recepty\n\n", fntE));
                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                doc.Add(line);
                prgHeading.Add(new Chunk("Sold medicaments report for ", fntHead));
                prgHeading.Add(new Chunk(userService.GetUser(data[0].PharmacistID).Result.Name + " " + userService.GetUser(data[0].PharmacistID).Result.LastName + " ID: " + data[0].PharmacistID.ToString() + "\n", fntHead));
                //prgHeading.Add(new Chunk("Address: " + data[0].PatientAddress, fntHead));
                doc.Add(prgHeading);

                Paragraph prgGeneration = new Paragraph();
                BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED);
                Font fntAuthor = new Font(btnAuthor, 10, 2, BaseColor.GRAY);
                prgGeneration.Alignment = Element.ALIGN_RIGHT;
                prgGeneration.Add(new Chunk(begin.Date.Month + "/" + begin.Date.Day + "/" + begin.Date.Year + " - " + end.Date.Month + "/" + end.Date.Day + "/" + end.Date.Year, fntAuthor));
                prgGeneration.Add(new Chunk("\nDate: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), fntAuthor));
                doc.Add(prgGeneration);

                Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                doc.Add(p);
                doc.Add(new Chunk("\n", fntHead));

                PdfPTable table = new PdfPTable(dtblTable.Columns.Count);
                BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED);
                Font fntColumnHeader = new Font(btnColumnHeader, 10, 1, BaseColor.WHITE);
                for (int i = 0; i < dtblTable.Columns.Count; i++)
                {
                    PdfPCell cell = new PdfPCell();
                    cell.BackgroundColor = BaseColor.DARK_GRAY;
                    cell.AddElement(new Chunk(dtblTable.Columns[i].ColumnName.ToUpper(), fntColumnHeader));
                    table.AddCell(cell);
                }

                for (int i = 0; i < dtblTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dtblTable.Columns.Count; j++)
                    {
                        table.AddCell(dtblTable.Rows[i][j].ToString());
                    }
                }
                doc.Add(table);
            }
            catch (Exception ex)
            {
                //throw new NotImplementedException();
            }
            finally
            {
                doc.Close();
            }
        }
    }
}
