namespace Lunare
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            textNombre = new TextBox();
            textContrasena = new TextBox();
            textCorreo = new TextBox();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // textNombre
            // 
            textNombre.BackColor = Color.White;
            textNombre.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textNombre.ForeColor = Color.DimGray;
            textNombre.Location = new Point(462, 104);
            textNombre.Margin = new Padding(2);
            textNombre.Multiline = true;
            textNombre.Name = "textNombre";
            textNombre.Size = new Size(244, 27);
            textNombre.TabIndex = 0;
            textNombre.Text = "Nombre";
          
            // 
            // textContrasena
            // 
            textContrasena.BackColor = Color.White;
            textContrasena.Font = new Font("Times New Roman", 14.1428576F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textContrasena.ForeColor = Color.DimGray;
            textContrasena.Location = new Point(462, 218);
            textContrasena.Margin = new Padding(2);
            textContrasena.Multiline = true;
            textContrasena.Name = "textContrasena";
            textContrasena.PasswordChar = '.';
            textContrasena.Size = new Size(244, 27);
            textContrasena.TabIndex = 6;
            textContrasena.Text = "contraseña";
           
            // 
            // textCorreo
            // 
            textCorreo.BackColor = Color.White;
            textCorreo.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textCorreo.ForeColor = Color.DimGray;
            textCorreo.Location = new Point(462, 161);
            textCorreo.Margin = new Padding(2);
            textCorreo.Multiline = true;
            textCorreo.Name = "textCorreo";
            textCorreo.Size = new Size(244, 27);
            textCorreo.TabIndex = 3;
            textCorreo.Text = "Correo";
            
            // 
            // button1
            // 
            button1.Font = new Font("Times New Roman", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.DimGray;
            button1.Location = new Point(518, 276);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(144, 25);
            button1.TabIndex = 9;
            button1.Text = "Acceder";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(4, 45, 98);
            button2.Font = new Font("Times New Roman", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.White;
            button2.Location = new Point(90, 248);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(149, 32);
            button2.TabIndex = 10;
            button2.Text = " iniciar sesión";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(808, 394);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textContrasena);
            Controls.Add(textCorreo);
            Controls.Add(textNombre);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(2);
            MaximizeBox = false;
            Name = "Form1";
            Text = "Form1";
           
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textNombre;
        private TextBox textContrasena;
        private TextBox textCorreo;
        private Button button1;
        private Button button2;
    }
}
