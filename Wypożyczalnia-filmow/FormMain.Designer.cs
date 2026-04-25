namespace Wypożyczalnia_filmow
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        // Kontrolki główne
        private System.Windows.Forms.Label lblKlient;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabKatalog;
        private System.Windows.Forms.TabPage tabMoje;
        private System.Windows.Forms.TabPage tabZarzadzanie;

        // Katalog filmów
        private System.Windows.Forms.TextBox txtSzukaj;
        private System.Windows.Forms.DataGridView dgvFilmy;
        private System.Windows.Forms.BindingSource bsFilmy;
        private System.Windows.Forms.BindingNavigator bnFilmy;
        private System.Windows.Forms.Button btnWypozycz;

        // Moje wypożyczenia
        private System.Windows.Forms.DataGridView dgvWypozyczenia;
        private System.Windows.Forms.Label lblKara;
        private System.Windows.Forms.Button btnZwroc;

        // Zarządzanie katalogiem
        private System.Windows.Forms.DataGridView dgvZarzadzanie;
        private System.Windows.Forms.BindingSource bsZarzadzanie;
        private System.Windows.Forms.BindingNavigator bnZarzadzanie;
        private System.Windows.Forms.Button btnDodajFilm;
        private System.Windows.Forms.Button btnUsunFilm;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ── BindingSources ─────────────────────────────────────
            bsFilmy = new System.Windows.Forms.BindingSource(components);
            bsZarzadzanie = new System.Windows.Forms.BindingSource(components);
            ((System.ComponentModel.ISupportInitialize)bsFilmy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bsZarzadzanie).BeginInit();

            // ── DataGridViews ──────────────────────────────────────
            dgvFilmy = new System.Windows.Forms.DataGridView();
            dgvWypozyczenia = new System.Windows.Forms.DataGridView();
            dgvZarzadzanie = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvFilmy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvWypozyczenia).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvZarzadzanie).BeginInit();

            // ── BindingNavigators ──────────────────────────────────
            bnFilmy = new System.Windows.Forms.BindingNavigator(components);
            bnZarzadzanie = new System.Windows.Forms.BindingNavigator(components);
            ((System.ComponentModel.ISupportInitialize)bnFilmy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bnZarzadzanie).BeginInit();
            bnFilmy.BindingSource = bsFilmy;
            bnZarzadzanie.BindingSource = bsZarzadzanie;

            // ── Pozostałe kontrolki ────────────────────────────────
            lblKlient = new System.Windows.Forms.Label();
            tabMain = new System.Windows.Forms.TabControl();
            tabKatalog = new System.Windows.Forms.TabPage();
            tabMoje = new System.Windows.Forms.TabPage();
            tabZarzadzanie = new System.Windows.Forms.TabPage();
            txtSzukaj = new System.Windows.Forms.TextBox();
            btnWypozycz = new System.Windows.Forms.Button();
            lblKara = new System.Windows.Forms.Label();
            btnZwroc = new System.Windows.Forms.Button();
            btnDodajFilm = new System.Windows.Forms.Button();
            btnUsunFilm = new System.Windows.Forms.Button();

            tabMain.SuspendLayout();
            tabKatalog.SuspendLayout();
            tabMoje.SuspendLayout();
            tabZarzadzanie.SuspendLayout();
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
            tabMain.Controls.Add(tabZarzadzanie);

            // ══════════════════════════════════════════
            // TAB: KATALOG FILMÓW
            // ══════════════════════════════════════════
            tabKatalog.Text = "Katalog filmów";
            tabKatalog.Name = "tabKatalog";
            tabKatalog.Padding = new System.Windows.Forms.Padding(8);
            tabKatalog.Controls.Add(dgvFilmy);      // Fill – dodany jako pierwszy
            tabKatalog.Controls.Add(btnWypozycz);
            tabKatalog.Controls.Add(bnFilmy);
            tabKatalog.Controls.Add(txtSzukaj);

            txtSzukaj.Dock = System.Windows.Forms.DockStyle.Top;
            txtSzukaj.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSzukaj.PlaceholderText = "Szukaj po tytule lub gatunku...";
            txtSzukaj.Name = "txtSzukaj";
            txtSzukaj.Height = 30;
            txtSzukaj.TextChanged += new System.EventHandler(txtSzukaj_TextChanged);

            dgvFilmy.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvFilmy.Name = "dgvFilmy";
            dgvFilmy.ReadOnly = true;
            dgvFilmy.AllowUserToAddRows = false;
            dgvFilmy.AllowUserToDeleteRows = false;
            dgvFilmy.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvFilmy.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvFilmy.Font = new System.Drawing.Font("Segoe UI", 9.5F);

            bnFilmy.Dock = System.Windows.Forms.DockStyle.Bottom;
            bnFilmy.Name = "bnFilmy";

            btnWypozycz.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnWypozycz.Name = "btnWypozycz";
            btnWypozycz.Text = "Wypożycz zaznaczony";
            btnWypozycz.Height = 36;
            btnWypozycz.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnWypozycz.Click += new System.EventHandler(btnWypozycz_Click);

            // ══════════════════════════════════════════
            // TAB: MOJE WYPOŻYCZENIA
            // ══════════════════════════════════════════
            tabMoje.Text = "Moje wypożyczenia";
            tabMoje.Name = "tabMoje";
            tabMoje.Padding = new System.Windows.Forms.Padding(8);
            tabMoje.Controls.Add(dgvWypozyczenia);  // Fill – dodany jako pierwszy
            tabMoje.Controls.Add(btnZwroc);
            tabMoje.Controls.Add(lblKara);

            dgvWypozyczenia.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvWypozyczenia.Name = "dgvWypozyczenia";
            dgvWypozyczenia.ReadOnly = true;
            dgvWypozyczenia.AllowUserToAddRows = false;
            dgvWypozyczenia.AllowUserToDeleteRows = false;
            dgvWypozyczenia.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvWypozyczenia.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvWypozyczenia.Font = new System.Drawing.Font("Segoe UI", 9.5F);

            lblKara.Dock = System.Windows.Forms.DockStyle.Bottom;
            lblKara.Name = "lblKara";
            lblKara.Text = "";
            lblKara.ForeColor = System.Drawing.Color.Red;
            lblKara.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblKara.Height = 28;
            lblKara.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            btnZwroc.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnZwroc.Name = "btnZwroc";
            btnZwroc.Text = "Zwróć zaznaczony";
            btnZwroc.Height = 36;
            btnZwroc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnZwroc.Click += new System.EventHandler(btnZwroc_Click);

            // ══════════════════════════════════════════
            // TAB: ZARZĄDZANIE KATALOGIEM
            // ══════════════════════════════════════════
            tabZarzadzanie.Text = "Zarządzanie katalogiem";
            tabZarzadzanie.Name = "tabZarzadzanie";
            tabZarzadzanie.Padding = new System.Windows.Forms.Padding(8);
            tabZarzadzanie.Controls.Add(dgvZarzadzanie);    // Fill – dodany jako pierwszy
            tabZarzadzanie.Controls.Add(btnUsunFilm);
            tabZarzadzanie.Controls.Add(btnDodajFilm);
            tabZarzadzanie.Controls.Add(bnZarzadzanie);

            dgvZarzadzanie.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvZarzadzanie.Name = "dgvZarzadzanie";
            dgvZarzadzanie.ReadOnly = true;
            dgvZarzadzanie.AllowUserToAddRows = false;
            dgvZarzadzanie.AllowUserToDeleteRows = false;
            dgvZarzadzanie.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvZarzadzanie.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvZarzadzanie.Font = new System.Drawing.Font("Segoe UI", 9.5F);

            bnZarzadzanie.Dock = System.Windows.Forms.DockStyle.Bottom;
            bnZarzadzanie.Name = "bnZarzadzanie";

            btnDodajFilm.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnDodajFilm.Name = "btnDodajFilm";
            btnDodajFilm.Text = "Dodaj film";
            btnDodajFilm.Height = 36;
            btnDodajFilm.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnDodajFilm.Click += new System.EventHandler(btnDodajFilm_Click);

            btnUsunFilm.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnUsunFilm.Name = "btnUsunFilm";
            btnUsunFilm.Text = "Usuń zaznaczony film";
            btnUsunFilm.Height = 36;
            btnUsunFilm.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnUsunFilm.ForeColor = System.Drawing.Color.DarkRed;
            btnUsunFilm.Click += new System.EventHandler(btnUsunFilm_Click);

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
            ((System.ComponentModel.ISupportInitialize)bsZarzadzanie).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvFilmy).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvWypozyczenia).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvZarzadzanie).EndInit();
            ((System.ComponentModel.ISupportInitialize)bnFilmy).EndInit();
            ((System.ComponentModel.ISupportInitialize)bnZarzadzanie).EndInit();
            tabMain.ResumeLayout(false);
            tabKatalog.ResumeLayout(false);
            tabMoje.ResumeLayout(false);
            tabZarzadzanie.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}