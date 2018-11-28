using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace MedicinesDatabase
{
    public class MedicinesDB
    {
        private string connectionString = @"Data source=iolab.database.windows.net,1433;
                                                             database=MedicinesDB;
                                                             User id=dbuser;
                                                             Password=Erecepta!;";
          

        public async Task<IEnumerable<Medicine>> SearchMedicine(string name)
        {
            List<Medicine> medicines = new List<Medicine>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    string commandText = "SELECT * FROM medicines WHERE name collate POLISH_CI_AS LIKE '%" + name + "%'";
                    command.CommandText = commandText;
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        medicines.Add(new Medicine(reader["id"].ToString(), reader["name"].ToString(),
                                                   reader["manufacturer"].ToString(), reader["refund_rate"].ToString()));
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

        public async Task<int> IsMedicineInDB(string id)    //returns 1 when id is in DB, 0 when it's not and -1 when exception occurs
        {
            int response = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    string commandText = "SELECT * FROM medicines WHERE id = " + id;
                    command.CommandText = commandText;
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    bool hasRows = reader.HasRows;

                    if (hasRows)
                        response = 1;    
                    else
                        response = 0;

                    reader.Close();
                }
                catch (Exception e)
                {
                   
                }

            }

            return response;
        }

        public async Task<float> GetRateOfRefund(String id)   //returns -1 when exception occurs
        {
            float refundRate = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    string commandText = "SELECT refund_rate FROM medicines WHERE id = " + id;
                    command.CommandText = commandText;
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        refundRate = float.Parse(reader["refund_rate"].ToString());
                    }
                    reader.Close();
                }
                catch (Exception e)
                {

                }
            }

            return refundRate;
        }
    }
}
