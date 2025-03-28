using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlX.XDevAPI.Common;

namespace almacen_lunare
{
    internal class Mysqlcontrol
    {

        public SqlCommand command;
        SqlDataReader reader;
        SqlConnection connection;

        public void Start(string sql) 
        {
            connection = new(sql);
            connection.Open();

        }

        public SqlCommand Query(string sql)
        {
            command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();
            
            return command;
        }

        public decimal DecReader(string columnname)
        {
            decimal result = 0;
            reader = command.ExecuteReader();
            if (reader != null)
            {
                while (reader.Read())
                {
                    result = (decimal)reader[columnname];
                    // Or reader.GetString(0), reader.GetInt32(0), etc.

                }

            return result;

            }

            

            return 0;
        }
        public string StrReader(string columnname)
        {
            string result = string.Empty;
            reader = command.ExecuteReader();
            if (reader != null)
            {
                while (reader.Read())
                {
                    foreach (char item in reader[columnname].ToString())
                    {
                        result = item.ToString();
                        
                    }
                    // Or reader.GetString(0), reader.GetInt32(0), etc.

                }

                return result;

            }



            return string.Empty;
        }
        public void Stop() 
        {
            connection.Close();
        }

    }
}
