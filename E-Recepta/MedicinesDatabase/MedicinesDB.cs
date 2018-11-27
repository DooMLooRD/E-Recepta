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
        private SqlConnection connection;
        public MedicinesDB()
        {
            
        }       

        public bool Connect()
        {
            try
            {
                connection = new SqlConnection(@"Data source=LOCALHOST\SQLEXPRESS;
                                                             database=MedicinesDB;
                                                             User id=sa;
                                                             Password=Erecepta;");
                connection.Open();
            }

            catch (SqlException e)
            {
                return false;
            }

            return true;
        }

        public bool CloseConnection()
        {
            try
            {
                connection.Close();
            }
            catch (SqlException e)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<Medicine> SearchMedicine(string name)
        {
            SqlCommand command = connection.CreateCommand();
            string commandText = "SELECT * FROM medicines WHERE name LIKE '%" + name + "%'";
            command.CommandText = commandText;
            SqlDataReader reader = command.ExecuteReader();

            List<Medicine> medicines = new List<Medicine>();

            while (reader.Read())
            {
                medicines.Add(new Medicine(reader["id"].ToString(), reader["name"].ToString(),
                                           reader["manufacturer"].ToString(), reader["refund_rate"].ToString()));
            }
            reader.Close();

            return medicines;
        }

        public bool IsMedicineInDB(string id)
        {
            SqlCommand command = connection.CreateCommand();
            string commandText = "SELECT * FROM medicines WHERE id = " + id;
            command.CommandText = commandText;
            SqlDataReader reader = command.ExecuteReader();

            bool hasRows = reader.HasRows;
            reader.Close();

            return hasRows;
        }

        public float GetRateOfRefund(String id)
        {
            SqlCommand command = connection.CreateCommand();
            string commandText = "SELECT * FROM medicines WHERE id = " + id;
            command.CommandText = commandText;
            SqlDataReader reader = command.ExecuteReader();

            float refundRate = -1;

            while (reader.Read())
            {
                refundRate = float.Parse(reader["refund_rate"].ToString());
            }
            reader.Close();

            return refundRate;
        }
    }
}
