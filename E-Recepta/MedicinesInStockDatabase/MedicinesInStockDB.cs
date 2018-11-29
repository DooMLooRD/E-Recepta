using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MedicinesInStockDatabase
{
    public class MedicinesInStockDB
    {
        private string connectionString = @"Data source=iolab.database.windows.net,1433;
                                                             database=MedicinesInStockDB;
                                                             User id=dbuser;
                                                             Password=Erecepta!;";

        public async Task<IEnumerable<MedicineInStock>> SearchMedicine(string name, string manufacturer, string pharmacyId)
        {
            List<MedicineInStock> medicines = new List<MedicineInStock>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    string commandText = "SELECT m.id, m.name, m.manufacturer, mis.amount, mis.cost " +
                                         "FROM medicines_in_stock mis, medicines m " +
                                         "WHERE mis.medicine_id = m.id " +
                                         "AND mis.pharmacy_id = " + pharmacyId +
                                         " AND name collate POLISH_CI_AS LIKE '%" + name + "%'" +
                                         " AND manufacturer collate POLISH_CI_AS LIKE '%" + manufacturer + "%';";
                    command.CommandText = commandText;
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        medicines.Add(new MedicineInStock(reader["id"].ToString(), reader["name"].ToString(),
                                                   reader["manufacturer"].ToString(), reader["amount"].ToString(),
                                                   reader["cost"].ToString(), pharmacyId));
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    medicines = null;
                }
            }

            return medicines;
        }

        public async Task<int> SellMedicine(string medicineId, string amount, string pharmacyId)
        {

            int result = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    string commandText = "UPDATE medicines_in_stock SET amount = amount - " + amount + 
                                            " WHERE pharmacy_id = " + pharmacyId + 
                                            " AND medicine_id = " + medicineId + 
                                            " AND amount >= " + amount;
                    command.CommandText = commandText;
                    result = await command.ExecuteNonQueryAsync();

                    
                }
                catch (Exception e)
                {
                    
                }
            }

            return result;
        }
    }
}
