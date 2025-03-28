namespace almacen_lunare
{
    partial class Productwindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Productwindow));
            panel7 = new Panel();
            label17 = new Label();
            minimizebttn = new Button();
            closebttn = new Button();
            dataGridView1 = new DataGridView();
            searchtbx = new TextBox();
            button3 = new Button();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel7
            // 
            panel7.BackColor = SystemColors.ControlLight;
            panel7.Controls.Add(label17);
            panel7.Controls.Add(minimizebttn);
            panel7.Controls.Add(closebttn);
            panel7.Location = new Point(2, 1);
            panel7.Name = "panel7";
            panel7.Size = new Size(933, 55);
            panel7.TabIndex = 7;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label17.ForeColor = Color.FromArgb(44, 44, 44);
            label17.Location = new Point(10, 8);
            label17.Name = "label17";
            label17.Size = new Size(254, 37);
            label17.TabIndex = 12;
            label17.Text = "Lista de Productos";
            // 
            // minimizebttn
            // 
            minimizebttn.BackgroundImage = (Image)resources.GetObject("minimizebttn.BackgroundImage");
            minimizebttn.BackgroundImageLayout = ImageLayout.Stretch;
            minimizebttn.FlatAppearance.BorderSize = 0;
            minimizebttn.FlatStyle = FlatStyle.Flat;
            minimizebttn.Location = new Point(823, 0);
            minimizebttn.Name = "minimizebttn";
            minimizebttn.Size = new Size(57, 55);
            minimizebttn.TabIndex = 11;
            minimizebttn.Text = " ";
            minimizebttn.UseVisualStyleBackColor = true;
            // 
            // closebttn
            // 
            closebttn.BackgroundImage = (Image)resources.GetObject("closebttn.BackgroundImage");
            closebttn.BackgroundImageLayout = ImageLayout.Stretch;
            closebttn.FlatAppearance.BorderSize = 0;
            closebttn.FlatStyle = FlatStyle.Flat;
            closebttn.Location = new Point(876, 0);
            closebttn.Name = "closebttn";
            closebttn.Size = new Size(57, 55);
            closebttn.TabIndex = 7;
            closebttn.UseVisualStyleBackColor = true;
            closebttn.Click += closebttn_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 143);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(911, 472);
            dataGridView1.TabIndex = 8;
            // 
            // searchtbx
            // 
            searchtbx.Font = new Font("Segoe UI", 18F);
            searchtbx.Location = new Point(12, 98);
            searchtbx.Name = "searchtbx";
            searchtbx.Size = new Size(722, 39);
            searchtbx.TabIndex = 9;
            searchtbx.TextChanged += searchtbx_TextChanged;
            // 
            // button3
            // 
            button3.BackColor = Color.DodgerBlue;
            button3.BackgroundImage = (Image)resources.GetObject("button3.BackgroundImage");
            button3.BackgroundImageLayout = ImageLayout.Stretch;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Location = new Point(740, 82);
            button3.Name = "button3";
            button3.Size = new Size(57, 55);
            button3.TabIndex = 12;
            button3.Text = " ";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // Productwindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(935, 642);
            Controls.Add(button3);
            Controls.Add(searchtbx);
            Controls.Add(dataGridView1);
            Controls.Add(panel7);
            Name = "Productwindow";
            Text = "Productwindow";
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel7;
        private Button minimizebttn;
        private Button closebttn;
        private DataGridView dataGridView1;
        private TextBox searchtbx;
        private Label label17;
        private Button button3;
    }
}