using Org.BouncyCastle.Asn1.Cmp;
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

        sensibledata sendata = new sensibledata();  
        connections conn = new connections();
        

        private void button1_Click(object sender, EventArgs e)
        {
            string promp = textBox1.Text;
            Getdata("select*from productos where nombre" + promp);
        }

        private void Getdata(string query) 
        {
            conn.StartConnection();
            conn.Query(query);
            conn.Adapter();
            dataGridView1.DataSource = conn.table;
            conn.Dispose();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
