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
        private string pharmacyId;

        public MedicinesInStockDB(string pharmacyId)
        {
            this.pharmacyId = pharmacyId;
        }

        public async Task<IEnumerable<MedicineInStock>> SearchMedicineInStock(string name, string manufacturer)
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

        public async Task<int> SellMedicine(string medicineId, string amount)    //returns 1 when amount was updated, 0 when it was not and -1 if exception occured
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

        public async Task<IEnumerable<Medicine>> SearchMedicine(string name, string manufacturer)  //may be used to get global MedicineId
        {
            List<Medicine> medicines = new List<Medicine>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    string commandText = "SELECT * FROM medicines WHERE name collate POLISH_CI_AS LIKE '%" + name + "%'" +
                                         " AND manufacturer collate POLISH_CI_AS LIKE '%" + manufacturer + "%';";
                    command.CommandText = commandText;
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        medicines.Add(new Medicine(reader["id"].ToString(), reader["name"].ToString(),
                                                   reader["manufacturer"].ToString()));
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

        public async Task<int> AddMedicineToStock(string medicineId, string amount, string cost)  // returns 1 when added , 0 when not, -1 when exception occurs
        {
            int result = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    string commandText = "INSERT INTO medicines_in_stock VALUES ("
                        + medicineId + ", " + amount + ", " + cost + ", " + pharmacyId + ");";

                    command.CommandText = commandText;
                    result = await command.ExecuteNonQueryAsync();


                }
                catch (Exception e)
                {

                }
            }

            return result;
        }

        public async Task<int> RemoveMedicineFromStock(string medicineId) // returns 1 when removed , 0 when not, -1 when exception occurs
        {
            int result = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    string commandText = "DELETE FROM medicines_in_stock WHERE medicine_id = " + medicineId +
                        " AND pharmacy_id = " + pharmacyId;

                    command.CommandText = commandText;
                    result = await command.ExecuteNonQueryAsync();


                }
                catch (Exception e)
                {

                }
            }

            return result;
        }

        public async Task<int> ChangeMedicineAmountInStock(string medicineId, string amountChange)    //returns 1 when amount was updated, 0 when it was not and -1 if exception occured
        {

            int result = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    string commandText = "UPDATE medicines_in_stock SET amount = amount + " + amountChange +
                                            " WHERE pharmacy_id = " + pharmacyId +
                                            " AND medicine_id = " + medicineId;
                    command.CommandText = commandText;
                    result = await command.ExecuteNonQueryAsync();


                }
                catch (Exception e)
                {

                }
            }

            return result;
        }

        public async Task<IEnumerable<Pharmacy>> SearchPharmaciesWhereMedicineIsAvailable(string medicineId)
        {
            List<Pharmacy> pharmacies = new List<Pharmacy>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    string commandText = "SELECT p.id, p.street, p.city FROM pharmacies p, medicines_in_stock m WHERE m.pharmacy_id != " + 
                        pharmacyId + " AND m.medicine_id = " + medicineId + " AND m.pharmacy_id = p.id";
                    command.CommandText = commandText;
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        pharmacies.Add(new Pharmacy(reader["id"].ToString(), reader["street"].ToString(),
                                                   reader["city"].ToString()));
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    pharmacies = null;
                }
            }

            return pharmacies;
        }
    }
}
