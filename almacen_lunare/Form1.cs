using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using almacen_lunare;

namespace Lunare
{
    public partial class Form1 : Form
    {
        private bool showContrasena = false;
        string connectionstr = @"Data Source=DESKTOP-M2RKSNG\SQLSQL;Initial Catalog=test;Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            showContrasena = !showContrasena;
            textContrasena.PasswordChar = showContrasena ? '\0' : '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textNombre.Text.Trim();
            string contra = textContrasena.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(contra))
            {
                MessageBox.Show("Por favor, llena todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using SqlConnection connection = new SqlConnection(connectionstr);
            connection.Open();

            
            string checkUserQuery = "SELECT COUNT(*) FROM usuarios WHERE username = @username";
            using (SqlCommand checkCmd = new SqlCommand(checkUserQuery, connection))
            {
                checkCmd.Parameters.AddWithValue("@username", name);
                int userExists = (int)checkCmd.ExecuteScalar();

                if (userExists > 0)
                {
                    MessageBox.Show("El usuario ya está registrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

           
            string insertQuery = "INSERT INTO usuarios (username, password) VALUES (@username, @password)";
            using SqlCommand insertCmd = new SqlCommand(insertQuery, connection);
            insertCmd.Parameters.AddWithValue("@username", name);
            insertCmd.Parameters.AddWithValue("@password", contra);

            int result = insertCmd.ExecuteNonQuery();
            if (result > 0)
            {
                MessageBox.Show("Registro exitoso. Ahora puedes iniciar sesión.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error al registrar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = textNombre.Text.Trim();
            string contra = textContrasena.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(contra))
            {
                MessageBox.Show("Por favor, llena todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionstr))
            {
                connection.Open();

                string loginQuery = "SELECT COUNT(*) FROM usuarios WHERE username = @username AND password = @password";
                using SqlCommand cmd = new SqlCommand(loginQuery, connection);
                cmd.Parameters.AddWithValue("@username", name);
                cmd.Parameters.AddWithValue("@password", contra);

                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Inicio de sesión exitoso.", "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();

                    
                    menu menuForm = new();
                    menuForm.ShowDialog();

                   
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
