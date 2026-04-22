namespace Wypożyczalnia_filmow
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        // Kontrolki
        private System.Windows.Forms.Label lblKlient;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabKatalog;
        private System.Windows.Forms.TabPage tabMoje;

        // Katalog
        private System.Windows.Forms.TextBox txtSzukaj;
        private System.Windows.Forms.DataGridView dgvFilmy;
        private System.Windows.Forms.BindingSource bsFilmy;
        private System.Windows.Forms.BindingNavigator bnFilmy;
        private System.Windows.Forms.Button btnWypozycz;

        // Moje wypożyczenia
        private System.Windows.Forms.DataGridView dgvWypozyczenia;
        private System.Windows.Forms.Label lblKara;
        private System.Windows.Forms.Button btnZwroc;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ── BindingSource ──────────────────────────────────────
            bsFilmy = new System.Windows.Forms.BindingSource(components);
            ((System.ComponentModel.ISupportInitialize)bsFilmy).BeginInit();

            // ── DataGridViews ──────────────────────────────────────
            dgvFilmy = new System.Windows.Forms.DataGridView();
            dgvWypozyczenia = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvFilmy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvWypozyczenia).BeginInit();

            // ── BindingNavigator ───────────────────────────────────
            bnFilmy = new System.Windows.Forms.BindingNavigator(components);
            ((System.ComponentModel.ISupportInitialize)bnFilmy).BeginInit();
            bnFilmy.BindingSource = bsFilmy;

            // ── Pozostałe kontrolki ────────────────────────────────
            lblKlient = new System.Windows.Forms.Label();
            tabMain = new System.Windows.Forms.TabControl();
            tabKatalog = new System.Windows.Forms.TabPage();
            tabMoje = new System.Windows.Forms.TabPage();
            txtSzukaj = new System.Windows.Forms.TextBox();
            btnWypozycz = new System.Windows.Forms.Button();
            lblKara = new System.Windows.Forms.Label();
            btnZwroc = new System.Windows.Forms.Button();

            tabMain.SuspendLayout();
            tabKatalog.SuspendLayout();
            tabMoje.SuspendLayout();
            SuspendLayout();

            // ── lblKlient ──────────────────────────────────────────
            lblKlient.AutoSize = true;
            lblKlient.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblKlient.Location = new System.Drawing.Point(12, 9);
            lblKlient.Name = "lblKlient";
            lblKlient.Text = "Klient: ";

            // ── tabMain ────────────────────────────────────────────
            tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tabMain.Location = new System.Drawing.Point(0, 30);
            tabMain.Name = "tabMain";
            tabMain.Controls.Add(tabKatalog);
            tabMain.Controls.Add(tabMoje);

            // ── tabKatalog ─────────────────────────────────────────
            tabKatalog.Text = "Katalog filmów";
            tabKatalog.Name = "tabKatalog";
            tabKatalog.Padding = new System.Windows.Forms.Padding(8);
            tabKatalog.Controls.Add(btnWypozycz);
            tabKatalog.Controls.Add(bnFilmy);
            tabKatalog.Controls.Add(dgvFilmy);
            tabKatalog.Controls.Add(txtSzukaj);

            // ── txtSzukaj ──────────────────────────────────────────
            txtSzukaj.Dock = System.Windows.Forms.DockStyle.Top;
            txtSzukaj.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSzukaj.PlaceholderText = "Szukaj po tytule lub gatunku...";
            txtSzukaj.Name = "txtSzukaj";
            txtSzukaj.Height = 30;
            txtSzukaj.TextChanged += new System.EventHandler(txtSzukaj_TextChanged);

            // ── dgvFilmy ───────────────────────────────────────────
            dgvFilmy.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvFilmy.Name = "dgvFilmy";
            dgvFilmy.ReadOnly = true;
            dgvFilmy.AllowUserToAddRows = false;
            dgvFilmy.AllowUserToDeleteRows = false;
            dgvFilmy.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvFilmy.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvFilmy.Font = new System.Drawing.Font("Segoe UI", 9.5F);

            // ── bnFilmy ────────────────────────────────────────────
            bnFilmy.Dock = System.Windows.Forms.DockStyle.Bottom;
            bnFilmy.Name = "bnFilmy";

            // ── btnWypozycz ────────────────────────────────────────
            btnWypozycz.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnWypozycz.Name = "btnWypozycz";
            btnWypozycz.Text = "Wypożycz zaznaczony";
            btnWypozycz.Height = 36;
            btnWypozycz.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnWypozycz.Click += new System.EventHandler(btnWypozycz_Click);

            // ── tabMoje ────────────────────────────────────────────
            tabMoje.Text = "Moje wypożyczenia";
            tabMoje.Name = "tabMoje";
            tabMoje.Padding = new System.Windows.Forms.Padding(8);
            tabMoje.Controls.Add(btnZwroc);
            tabMoje.Controls.Add(lblKara);
            tabMoje.Controls.Add(dgvWypozyczenia);

            // ── dgvWypozyczenia ────────────────────────────────────
            dgvWypozyczenia.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvWypozyczenia.Name = "dgvWypozyczenia";
            dgvWypozyczenia.ReadOnly = true;
            dgvWypozyczenia.AllowUserToAddRows = false;
            dgvWypozyczenia.AllowUserToDeleteRows = false;
            dgvWypozyczenia.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvWypozyczenia.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvWypozyczenia.Font = new System.Drawing.Font("Segoe UI", 9.5F);

            // ── lblKara ────────────────────────────────────────────
            lblKara.Dock = System.Windows.Forms.DockStyle.Bottom;
            lblKara.Name = "lblKara";
            lblKara.Text = "";
            lblKara.ForeColor = System.Drawing.Color.Red;
            lblKara.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblKara.Height = 28;
            lblKara.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── btnZwroc ───────────────────────────────────────────
            btnZwroc.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnZwroc.Name = "btnZwroc";
            btnZwroc.Text = "Zwróć zaznaczony";
            btnZwroc.Height = 36;
            btnZwroc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnZwroc.Click += new System.EventHandler(btnZwroc_Click);

            // ── FormMain ───────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(900, 600);
            MinimumSize = new System.Drawing.Size(700, 480);
            Name = "FormMain";
            Text = "Wypożyczalnia filmów";
            Controls.Add(tabMain);
            Controls.Add(lblKlient);
            Load += new System.EventHandler(FormMain_Load);

            ((System.ComponentModel.ISupportInitialize)bsFilmy).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvFilmy).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvWypozyczenia).EndInit();
            ((System.ComponentModel.ISupportInitialize)bnFilmy).EndInit();
            tabMain.ResumeLayout(false);
            tabKatalog.ResumeLayout(false);
            tabMoje.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}