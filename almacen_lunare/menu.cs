using ScottPlot;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Reflection.Metadata;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;
using MySqlX.XDevAPI.Relational;
using System.Runtime.CompilerServices;
using Label = System.Windows.Forms.Label;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;

namespace almacen_lunare
{
    public partial class menu : Form
    {
        string connectionstr = @"Data Source=Localhost;Initial Catalog=LunareBoutique;Integrated Security=True;";
        private DataTable productTable = new();
        Mysqlcontrol MSC = new Mysqlcontrol();
        public menu()
        {

            InitializeComponent();
            Createplot();
            LoadCustomers();
            LoadProducts();
            SetupDataGridView();
            GenerateOrderDetails(panellogfact, dataGridView1);

            //tabControl1.Appearance = TabAppearance.FlatButtons;
            //tabControl1.ItemSize = new Size(0, 1);
            //tabControl1.SizeMode = TabSizeMode.Fixed;

        }
        bool Isopen = false;
        private void formsPlot1_Load(object sender, EventArgs e)
        {



        }


        private void GenerateOrderDetails(Panel parentPanel, DataGridView targetDataGridView)
        {
            this.Text = "Detalles de Pedido";
            this.Size = new Size(600, 500);

            FlowLayoutPanel containerPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            parentPanel.Controls.Add(containerPanel);

            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();

                // Get all order IDs
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT id FROM detalle_pedidos ORDER BY id ASC", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int orderId = reader.GetInt32(0);

                    // Create panel for each order
                    Panel orderPanel = new Panel
                    {
                        Width = 550,
                        Height = 300,
                        Margin = new Padding(10),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.FromArgb(240, 240, 240)
                    };

                    // Add order ID label
                    Label orderLabel = new Label
                    {
                        Text = $"Pedido #: {orderId}",
                        Location = new Point(10, 10),
                        AutoSize = true
                    };
                    orderPanel.Controls.Add(orderLabel);

                    Button buttondetailfac = new Button
                    {
                        Text = "generar factura"

                    };

                    // Add DataGridView for order items
                    DataGridView dgv = new DataGridView
                    {
                        Width = 530,
                        Height = 250,
                        Location = new Point(10, 40),
                        ReadOnly = true,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                        DataSource = LoadOrderDetails(orderId) // Load items for this order
                    };

                   targetDataGridView.DataSource= dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    orderPanel.Controls.Add(dgv);
                    containerPanel.Controls.Add(orderPanel);
                }
            }
        }

      
           private DataTable LoadOrderDetails(int orderId)
        {
            DataTable itemsTable;
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();

                // Get order header info
                string headerQuery = @"
            SELECT 
                p.id AS PedidoID,
                p.fecha AS Fecha,
                p.monto AS Total,
                p.estado AS Estado,
                CONCAT(p.nombre, ' ', p.apellido) AS Cliente
            FROM pedidos p
            WHERE p.id = @OrderId";

                SqlCommand headerCmd = new SqlCommand(headerQuery, conn);
                headerCmd.Parameters.AddWithValue("@OrderId", orderId);

                DataTable headerTable = new DataTable();
                new SqlDataAdapter(headerCmd).Fill(headerTable);

                // Display header info (optional)
                if (headerTable.Rows.Count > 0)
                {
                    DataRow header = headerTable.Rows[0];
                    //MessageBox.Show($"Cliente: {header["Cliente"]}\nFecha: {header["Fecha"]}\nTotal: {header["Total"]:C}");
                }

                // Get order items
                string itemsQuery = @"
            SELECT 
                dp.producto AS Producto,
                dp.precio AS PrecioUnitario,
                dp.cantidad AS Cantidad,
                (dp.precio * dp.cantidad) AS TotalLinea
            FROM detalle_pedidos dp
            WHERE dp.id_pedido = @OrderId";

                SqlCommand itemsCmd = new SqlCommand(itemsQuery, conn);
                itemsCmd.Parameters.AddWithValue("@OrderId", orderId);

                itemsTable = new DataTable();
                new SqlDataAdapter(itemsCmd).Fill(itemsTable);

              
            }
            return itemsTable;    
        }

        private void buttondetailfac(object sender, EventArgs e)
        {

            

        }

        public void Createplot()
        {
            MSC.Start(connectionstr);
            MSC.Query("SELECT SUM(monto) AS Totalmonto from pedidos");
            decimal result = MSC.DecReader("Totalmonto");

            double[] xs1 = { 1 };
            double[] ys1 = { (double)result };
            MSC.Stop();
            var bars1 = formsPlot1.Plot.Add.Bars(xs1, ys1);
            bars1.LegendText = "Ganancias del Mes";


            formsPlot1.Refresh();
            
        }
        private void LoadCustomers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstr))
                {
                    conn.Open();
                    string query = "SELECT id AS CustomerID, nombre AS Name FROM clientes";
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
                string query = "SELECT id AS ProductID, nombre AS Name, precio AS Price FROM productos";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable productTable = new DataTable();
                adapter.Fill(productTable);

                cboProduct.DataSource = productTable;
                cboProduct.DisplayMember = "Name";
                cboProduct.ValueMember = "ProductID";
            }
        }

        private void SetupDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("id", "ID");
            dataGridView1.Columns.Add("nombre", "Product Name");
            dataGridView1.Columns.Add("precio", "Price");
            dataGridView1.Columns.Add("cantidad", "Quantity"); // Changed from "In Stock" to match usage
            dataGridView1.Columns.Add("total", "Total"); // Added missing total column

        }
        private void UpdateTotalAmount()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["precio"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["precio"].Value);
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
                    table.AddCell(row.Cells["nombre"].Value?.ToString() ?? "N/A");
                    table.AddCell(row.Cells["cantidad"].Value?.ToString() ?? "0");
                    table.AddCell($"${Convert.ToDecimal(row.Cells["precio"].Value ?? 0):F2}");
                    table.AddCell($"${Convert.ToDecimal(row.Cells["total"].Value ?? 0):F2}");
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

        private void rroduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProduct.SelectedIndex != -1)
            {
                DataRowView selectedRow = (DataRowView)cboProduct.SelectedItem;
                txtUnitPrice.Text = selectedRow["precio"].ToString();
            }
        }

        private void fac_bttn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0 || dataGridView1.Rows[0].Cells["id"].Value == null)
            {
                MessageBox.Show("Por favor, añada artículos a la factura.");
                return;
            }

            // Get customer ID from combobox
            int clienteId = (int)cboCustomer.SelectedValue;
            decimal totalAmount = decimal.Parse(txtTotalAmount.Text.Replace("$", "").Replace(",", ""));

            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                SqlTransaction transaction = null;
                try
                {
                    transaction = conn.BeginTransaction();

                    // 1. Insert into pedidos table (your invoices table)
                    string pedidoQuery = @"INSERT INTO pedidos (
                                id_transaccion, 
                                monto, 
                                estado, 
                                fecha,
                                email,
                                nombre,
                                apellido,
                                direccion,
                                ciudad,
                                email_user,
                                proceso
                              ) VALUES (
                                @IdTransaccion, 
                                @Monto, 
                                'Completado', 
                                @Fecha,
                                (SELECT correo FROM clientes WHERE id = @ClienteId),
                                (SELECT nombre FROM clientes WHERE id = @ClienteId),
                                '',
                                '',
                                '',
                                (SELECT correo FROM clientes WHERE id = @ClienteId),
                                '1'
                              );
                              SELECT SCOPE_IDENTITY();";

                    SqlCommand pedidoCmd = new SqlCommand(pedidoQuery, conn, transaction);
                    pedidoCmd.Parameters.AddWithValue("@IdTransaccion", Guid.NewGuid().ToString());
                    pedidoCmd.Parameters.AddWithValue("@Monto", totalAmount);
                    pedidoCmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
                    pedidoCmd.Parameters.AddWithValue("@ClienteId", clienteId);

                    int pedidoId = Convert.ToInt32(pedidoCmd.ExecuteScalar());

                    // 2. Insert into detalle_pedidos (your invoice items)
                    string detalleQuery = @"INSERT INTO detalle_pedidos (
                                  producto, 
                                  precio, 
                                  cantidad, 
                                  id_pedido
                               ) VALUES (
                                  @Producto, 
                                  @Precio, 
                                  @Cantidad, 
                                  @IdPedido
                               )";

                    SqlCommand detalleCmd = new SqlCommand(detalleQuery, conn, transaction);
                    detalleCmd.Parameters.Add("@Producto", SqlDbType.NVarChar, 255);
                    detalleCmd.Parameters.Add("@Precio", SqlDbType.Decimal);
                    detalleCmd.Parameters.Add("@Cantidad", SqlDbType.Int);
                    detalleCmd.Parameters.Add("@IdPedido", SqlDbType.Int);

                    // 3. Process each row in the DataGridView
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow && row.Cells["id"].Value != null)
                        {
                            // Get product name from the grid or you could query it:
                            string productName = row.Cells["nombre"].Value.ToString();

                            detalleCmd.Parameters["@Producto"].Value = productName;
                            detalleCmd.Parameters["@Precio"].Value = Convert.ToDecimal(row.Cells["precio"].Value);
                            detalleCmd.Parameters["@Cantidad"].Value = Convert.ToInt32(row.Cells["cantidad"].Value);
                            detalleCmd.Parameters["@IdPedido"].Value = pedidoId;
                            detalleCmd.ExecuteNonQuery();

                            // 4. Update product stock (optional)
                            string updateStockQuery = "UPDATE productos SET cantidad = cantidad - @Cantidad WHERE id = @ProductId";
                            SqlCommand updateCmd = new SqlCommand(updateStockQuery, conn, transaction);
                            updateCmd.Parameters.AddWithValue("@Cantidad", Convert.ToInt32(row.Cells["cantidad"].Value));
                            updateCmd.Parameters.AddWithValue("@ProductId", Convert.ToInt32(row.Cells["id"].Value));
                            updateCmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    GeneratePDF(pedidoId, 800, 800);
                    MessageBox.Show("Factura generada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    //transaction?.Rollback();
                    MessageBox.Show($"Error al procesar la factura: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Helper method to update product stock
        private void UpdateProductStock(int productId, int quantitySold, SqlConnection conn, SqlTransaction transaction)
        {
            string updateQuery = "UPDATE productos SET cantidad = cantidad - @Quantity WHERE id = @ProductId";
            SqlCommand updateCmd = new SqlCommand(updateQuery, conn, transaction);
            updateCmd.Parameters.AddWithValue("@Quantity", quantitySold);
            updateCmd.Parameters.AddWithValue("@ProductId", productId);
            updateCmd.ExecuteNonQuery();
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
        private void button3_Click(object sender, EventArgs e)
        {
            if(!Isopen)
            {
                Isopen = true;
                panellogfact.Enabled = true;
                panellogfact.Visible = true;
            }
            else
            {
                panellogfact.Enabled = false;
                panellogfact.Visible = false;
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
            decimal quantity = decimal.Parse(txtUnitPrice.Text);
            int unitPrice = (int)numQuantity.Value;

            if (unitPrice != null)
            {
                decimal total = quantity * unitPrice;

                dataGridView1.Rows.Add(productId, productName, quantity, unitPrice, total);
                UpdateTotalAmount();
                return;
            }

            MessageBox.Show("Porfavor Ingrese el precio unitario");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Productwindow PW = new Productwindow();
            PW.Show();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cboCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboProduct_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void panellogfact_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
