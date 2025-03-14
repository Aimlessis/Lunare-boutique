using ScottPlot;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Reflection.Metadata;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;

namespace almacen_lunare
{
    public partial class menu : Form
    {
        string connectionstr = @"Data Source=Localhost;Initial Catalog=test;Integrated Security=True;";
        private DataTable productTable = new();
        public menu()
        {

            InitializeComponent();
            Createplot();
            LoadCustomers();
            LoadProducts();
            SetupDataGridView();

        }
        bool Isopen = false;
        private void formsPlot1_Load(object sender, EventArgs e)
        {

            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;

        }

        public void Createplot()
        {
            double[] xs1 = { 1, 2, 3, 4 };
            double[] ys1 = { 5, 10, 7, 13 };
            var bars1 = formsPlot1.Plot.Add.Bars(xs1, ys1);
            bars1.LegendText = "Ganancias";

            double[] xs2 = { 6, 7, 8, 9 };
            double[] ys2 = { 7, 12, 9, 15 };
            var bars2 = formsPlot1.Plot.Add.Bars(xs2, ys2);
            bars2.LegendText = "Perdidas";

            formsPlot1.Plot.ShowLegend(Alignment.UpperLeft);
            formsPlot1.Plot.Axes.Margins(bottom: 0);

            formsPlot1.Refresh();
        }
        private void LoadCustomers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstr))
                {
                    conn.Open();
                    string query = "SELECT CustomerID, Name FROM Customers";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable customerTable = new DataTable();
                    adapter.Fill(customerTable);
                    cboCustomer.DataSource = customerTable;
                    cboCustomer.DisplayMember = "Name";
                    cboCustomer.ValueMember = "CustomerID";
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error de conexión a la base de datos: {ex.Message}\n\nNúmero de error: {ex.Number}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                string query = "SELECT ProductID, Name, Price FROM Products";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.Fill(productTable);
                cboProduct.DataSource = productTable;
                cboProduct.DisplayMember = "Name";
                cboProduct.ValueMember = "ProductID";
            }
        }

        private void SetupDataGridView()
        {
            dataGridView1.Columns.Add("ProductID", "Product ID");
            dataGridView1.Columns.Add("ProductName", "Product Name");
            dataGridView1.Columns.Add("Quantity", "Quantity");
            dataGridView1.Columns.Add("UnitPrice", "Unit Price");
            dataGridView1.Columns.Add("Total", "Total");
        }
        private void UpdateTotalAmount()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Total"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["Total"].Value);
                }
            }
            txtTotalAmount.Text = total.ToString("C");
        }
        private void GeneratePDF(int invoiceId, int width, int height)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Document document = new Document(PageSize.A4);
            string pdfPath = Path.Combine(documentsPath, $"Factura_{invoiceId}.pdf");
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));
            document.Open();

            // Configurar márgenes y tamaño de página
            document.SetMargins(40, 40, 40, 40);

            document.Add(new Paragraph($"Factura #{invoiceId}"));
            document.Add(new Paragraph($"Fecha: {DateTime.Now}"));
            document.Add(new Paragraph($"Cliente: {cboCustomer.Text}"));
            // Nota: Email y teléfono no están disponibles en el DataGridView, se omiten

            document.Add(new Paragraph("\n"));

            PdfPTable table = new PdfPTable(4);
            table.AddCell("Producto");
            table.AddCell("Cantidad");
            table.AddCell("Precio Unitario");
            table.AddCell("Total");

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    table.AddCell(row.Cells["ProductName"].Value.ToString());
                    table.AddCell(row.Cells["Quantity"].Value.ToString());
                    table.AddCell($"${Convert.ToDecimal(row.Cells["UnitPrice"].Value):F2}");
                    table.AddCell($"${Convert.ToDecimal(row.Cells["Total"].Value):F2}");
                }
            }

            document.Add(table);
            document.Add(new Paragraph($"\nMonto Total: {txtTotalAmount.Text}"));

            // Si necesitas agregar una imagen, hazlo así:
            if (File.Exists("logo.png")) // o la ruta a tu imagen
            {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("logo.png");
                logo.ScaleToFit(100f, 100f); // Dimensiones explícitas en puntos
                document.Add(logo);
            }

            document.Close();

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(pdfPath) { UseShellExecute = true });
            MessageBox.Show($"Factura generada exitosamente. PDF guardado en:\n{pdfPath}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
           
        }

        private void cboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProduct.SelectedIndex != -1)
            {
                DataRowView selectedRow = (DataRowView)cboProduct.SelectedItem;
                txtUnitPrice.Text = selectedRow["Price"].ToString();
            }
        }

        private void fac_bttn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0 || dataGridView1.Rows[0].Cells["ProductID"].Value == null)
            {
                MessageBox.Show("Por favor, añada artículos a la factura.");
                return;
            }

            int customerId = (int)cboCustomer.SelectedValue;
            decimal totalAmount = decimal.Parse(txtTotalAmount.Text.Replace("$", "").Replace(",", ""));

            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                SqlTransaction transaction = null;
                try
                {
                    transaction = conn.BeginTransaction();

                    // Insert invoice
                    string invoiceQuery = "INSERT INTO Invoices (CustomerID, InvoiceDate, TotalAmount) VALUES (@CustomerID, @InvoiceDate, @TotalAmount); SELECT SCOPE_IDENTITY();";
                    SqlCommand invoiceCmd = new SqlCommand(invoiceQuery, conn, transaction);
                    invoiceCmd.Parameters.AddWithValue("@CustomerID", customerId);
                    invoiceCmd.Parameters.AddWithValue("@InvoiceDate", DateTime.Now);
                    invoiceCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    int invoiceId = Convert.ToInt32(invoiceCmd.ExecuteScalar());

                    // Insert invoice items
                    string itemQuery = "INSERT INTO InvoiceItems (InvoiceID, ProductID, Quantity, UnitPrice) VALUES (@InvoiceID, @ProductID, @Quantity, @UnitPrice)";
                    SqlCommand itemCmd = new SqlCommand(itemQuery, conn, transaction);
                    itemCmd.Parameters.Add("@InvoiceID", SqlDbType.Int);
                    itemCmd.Parameters.Add("@ProductID", SqlDbType.Int);
                    itemCmd.Parameters.Add("@Quantity", SqlDbType.Int);
                    itemCmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal);

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow && row.Cells["ProductID"].Value != null)
                        {
                            itemCmd.Parameters["@InvoiceID"].Value = invoiceId;
                            itemCmd.Parameters["@ProductID"].Value = Convert.ToInt32(row.Cells["ProductID"].Value);
                            itemCmd.Parameters["@Quantity"].Value = Convert.ToInt32(row.Cells["Quantity"].Value);
                            itemCmd.Parameters["@UnitPrice"].Value = Convert.ToDecimal(row.Cells["UnitPrice"].Value);
                            itemCmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    GeneratePDF(invoiceId, 800, 800);
                    MessageBox.Show("Factura generada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Removemos la llamada a ClearInvoice() para no borrar los datos
                    // ClearInvoice();
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception rollbackEx)
                        {
                            MessageBox.Show("Error al revertir la transacción: " + rollbackEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    MessageBox.Show("Error al generar la factura: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void extendmenu_Click(object sender, EventArgs e)
        {

            if (!Isopen)
            {
                Isopen = true;
                panel6.Enabled = true;
                panel6.Visible = true;
            }
            else
            {
                panel6.Enabled = false;
                panel6.Visible = false;
                Isopen = false;
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAddItem_Click_1(object sender, EventArgs e)
        {
            int productId = (int)cboProduct.SelectedValue;
            string productName = cboProduct.Text;
            int quantity = (int)numQuantity.Value;
            decimal unitPrice = decimal.Parse(txtUnitPrice.Text);
            decimal total = quantity * unitPrice;

            dataGridView1.Rows.Add(productId, productName, quantity, unitPrice, total);
            UpdateTotalAmount();
        }
    }
}
