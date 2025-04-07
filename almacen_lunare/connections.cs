using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace almacen_lunare
{
    public class connections
    {
        static sensibledata sendata = new sensibledata();
        MySqlConnection connection = new MySqlConnection(sendata.connectionstr);
        MySqlDataAdapter adapter;
        public DataTable table;
        MySqlCommand command;

        public void StartConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
            }
            catch (MySqlException ex)
            {
                // Handle specific MySQL errors here
                throw new Exception($"MySQL Connection Error: {ex.Message}");
            }
        }

        public MySqlCommand Query(string sql)
        {
            try
            {
                command = new MySqlCommand(sql, connection);
                command.ExecuteNonQuery();
                return command;
            }
            catch (MySqlException ex)
            {
                // Handle query execution errors
                throw new Exception($"MySQL Query Error: {ex.Message}");
            }
        }

        public MySqlDataAdapter Adapter()
        {
            try
            {
                adapter = new MySqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);
                return adapter;
            }
            catch (MySqlException ex)
            {
                // Handle data adapter errors
                throw new Exception($"MySQL DataAdapter Error: {ex.Message}");
            }
        }

        public void StopConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                // Handle connection closing errors
                throw new Exception($"MySQL Connection Closing Error: {ex.Message}");
            }
        }

        // Dispose pattern for proper cleanup
        public void Dispose()
        {
            StopConnection();
            connection.Dispose();
            adapter?.Dispose();
            command?.Dispose();
        }
    }
}
