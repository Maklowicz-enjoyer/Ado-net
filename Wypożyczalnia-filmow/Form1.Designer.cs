namespace Wypożyczalnia_filmow
{
    partial class FormMain
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
            components = new System.ComponentModel.Container();
            tabControl1 = new TabControl();
            tabPageMovieRental = new TabPage();
            tabPageMyRental = new TabPage();
            textBox1 = new TextBox();
            dataGridView1 = new DataGridView();
            bindingSource1 = new BindingSource(components);
            tabControl1.SuspendLayout();
            tabPageMovieRental.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageMovieRental);
            tabControl1.Controls.Add(tabPageMyRental);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(954, 693);
            tabControl1.TabIndex = 0;
            // 
            // tabPageMovieRental
            // 
            tabPageMovieRental.BackColor = Color.Gray;
            tabPageMovieRental.Controls.Add(dataGridView1);
            tabPageMovieRental.Controls.Add(textBox1);
            tabPageMovieRental.Location = new Point(4, 24);
            tabPageMovieRental.Name = "tabPageMovieRental";
            tabPageMovieRental.Padding = new Padding(3);
            tabPageMovieRental.Size = new Size(946, 665);
            tabPageMovieRental.TabIndex = 0;
            tabPageMovieRental.Text = "Katalog Filmów";
            // 
            // tabPageMyRental
            // 
            tabPageMyRental.BackColor = Color.DimGray;
            tabPageMyRental.ForeColor = SystemColors.ControlLightLight;
            tabPageMyRental.Location = new Point(4, 24);
            tabPageMyRental.Name = "tabPageMyRental";
            tabPageMyRental.Padding = new Padding(3);
            tabPageMyRental.Size = new Size(946, 665);
            tabPageMyRental.TabIndex = 1;
            tabPageMyRental.Text = "Moje wypożyczenia";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(0, 6);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Szukaj po tytule ...";
            textBox1.Size = new Size(926, 23);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 46);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(940, 412);
            dataGridView1.TabIndex = 2;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(954, 693);
            Controls.Add(tabControl1);
            Name = "FormMain";
            Text = "Wypożyczalnia filmów";
            tabControl1.ResumeLayout(false);
            tabPageMovieRental.ResumeLayout(false);
            tabPageMovieRental.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPageMovieRental;
        private TabPage tabPageMyRental;
        private DataGridView dataGridView1;
        private TextBox textBox1;
        private BindingSource bindingSource1;
    }
}
