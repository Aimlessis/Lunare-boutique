using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace almacen_lunare
{
    public partial class Productwindow : Form
    {
        public Productwindow()
        {
            InitializeComponent();
            Getdata("select*from productos");
        }
        string connectionString = @"Data Source=Localhost;Initial Catalog=LunareBoutique;Integrated Security=True;";

        Mysqlcontrol MSC = new Mysqlcontrol();

        private void Getdata(string query)
        {
            MSC.Start(connectionString);
            MSC.Query(query);
            SqlDataAdapter adapter = new(MSC.command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            MSC.Stop();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string promp = searchtbx.Text;
            Getdata("select*from productos where LIKE '" + promp + "';");
        }
        private void searchtbx_TextChanged(object sender, EventArgs e)
        {

        }

        private void closebttn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
