namespace Wypożyczalnia_filmow
{
    partial class FormDodajFilm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTytul;
        private System.Windows.Forms.Label lblGatunek;
        private System.Windows.Forms.Label lblRok;
        private System.Windows.Forms.Label lblRezyser;
        private System.Windows.Forms.Label lblKopie;
        private System.Windows.Forms.Label lblCena;

        private System.Windows.Forms.TextBox txtTytul;
        private System.Windows.Forms.ComboBox cmbGatunek;
        private System.Windows.Forms.NumericUpDown numRok;
        private System.Windows.Forms.TextBox txtRezyser;
        private System.Windows.Forms.NumericUpDown numKopie;
        private System.Windows.Forms.NumericUpDown numCena;

        private System.Windows.Forms.Button btnZapisz;
        private System.Windows.Forms.Button btnAnuluj;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTytul = new System.Windows.Forms.Label();
            lblGatunek = new System.Windows.Forms.Label();
            lblRok = new System.Windows.Forms.Label();
            lblRezyser = new System.Windows.Forms.Label();
            lblKopie = new System.Windows.Forms.Label();
            lblCena = new System.Windows.Forms.Label();

            txtTytul = new System.Windows.Forms.TextBox();
            cmbGatunek = new System.Windows.Forms.ComboBox();
            numRok = new System.Windows.Forms.NumericUpDown();
            txtRezyser = new System.Windows.Forms.TextBox();
            numKopie = new System.Windows.Forms.NumericUpDown();
            numCena = new System.Windows.Forms.NumericUpDown();

            btnZapisz = new System.Windows.Forms.Button();
            btnAnuluj = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)numRok).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numKopie).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCena).BeginInit();
            SuspendLayout();

            // ── Etykiety ───────────────────────────────────────────
            int labelX = 20;
            int inputX = 140;
            int w = 260;

            lblTytul.Text = "Tytuł:";
            lblGatunek.Text = "Gatunek:";
            lblRok.Text = "Rok produkcji:";
            lblRezyser.Text = "Reżyser:";
            lblKopie.Text = "Dostępne kopie:";
            lblCena.Text = "Cena za dzień (zł):";

            int[] rows = { 20, 60, 100, 140, 180, 220 };
            System.Windows.Forms.Label[] labels = { lblTytul, lblGatunek, lblRok, lblRezyser, lblKopie, lblCena };
            foreach (var (lbl, y) in System.Linq.Enumerable.Zip(labels, rows, (l, y) => (l, y)))
            {
                lbl.AutoSize = true;
                lbl.Location = new System.Drawing.Point(labelX, y + 3);
                lbl.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            }

            // ── txtTytul ───────────────────────────────────────────
            txtTytul.Location = new System.Drawing.Point(inputX, rows[0]);
            txtTytul.Width = w;
            txtTytul.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtTytul.Name = "txtTytul";

            // ── cmbGatunek ─────────────────────────────────────────
            cmbGatunek.Location = new System.Drawing.Point(inputX, rows[1]);
            cmbGatunek.Width = w;
            cmbGatunek.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            cmbGatunek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbGatunek.Name = "cmbGatunek";

            // ── numRok ─────────────────────────────────────────────
            numRok.Location = new System.Drawing.Point(inputX, rows[2]);
            numRok.Width = w;
            numRok.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            numRok.Minimum = 1900;
            numRok.Maximum = DateTime.Now.Year;
            numRok.Value = DateTime.Now.Year;
            numRok.Name = "numRok";

            // ── txtRezyser ─────────────────────────────────────────
            txtRezyser.Location = new System.Drawing.Point(inputX, rows[3]);
            txtRezyser.Width = w;
            txtRezyser.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtRezyser.Name = "txtRezyser";

            // ── numKopie ───────────────────────────────────────────
            numKopie.Location = new System.Drawing.Point(inputX, rows[4]);
            numKopie.Width = w;
            numKopie.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            numKopie.Minimum = 1;
            numKopie.Maximum = 999;
            numKopie.Value = 1;
            numKopie.Name = "numKopie";

            // ── numCena ────────────────────────────────────────────
            numCena.Location = new System.Drawing.Point(inputX, rows[5]);
            numCena.Width = w;
            numCena.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            numCena.Minimum = 0;
            numCena.Maximum = 9999;
            numCena.DecimalPlaces = 2;
            numCena.Value = 5;
            numCena.Name = "numCena";

            // ── btnZapisz ──────────────────────────────────────────
            btnZapisz.Location = new System.Drawing.Point(inputX, 270);
            btnZapisz.Width = 120;
            btnZapisz.Height = 36;
            btnZapisz.Text = "Zapisz";
            btnZapisz.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnZapisz.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            btnZapisz.ForeColor = System.Drawing.Color.White;
            btnZapisz.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnZapisz.Name = "btnZapisz";
            btnZapisz.Click += new System.EventHandler(btnZapisz_Click);

            // ── btnAnuluj ──────────────────────────────────────────
            btnAnuluj.Location = new System.Drawing.Point(inputX + 130, 270);
            btnAnuluj.Width = 120;
            btnAnuluj.Height = 36;
            btnAnuluj.Text = "Anuluj";
            btnAnuluj.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            btnAnuluj.Name = "btnAnuluj";
            btnAnuluj.Click += new System.EventHandler(btnAnuluj_Click);

            // ── FormDodajFilm ──────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(440, 330);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Dodaj nowy film";
            Name = "FormDodajFilm";

            Controls.Add(lblTytul);
            Controls.Add(txtTytul);
            Controls.Add(lblGatunek);
            Controls.Add(cmbGatunek);
            Controls.Add(lblRok);
            Controls.Add(numRok);
            Controls.Add(lblRezyser);
            Controls.Add(txtRezyser);
            Controls.Add(lblKopie);
            Controls.Add(numKopie);
            Controls.Add(lblCena);
            Controls.Add(numCena);
            Controls.Add(btnZapisz);
            Controls.Add(btnAnuluj);

            Load += new System.EventHandler(FormDodajFilm_Load);

            ((System.ComponentModel.ISupportInitialize)numRok).EndInit();
            ((System.ComponentModel.ISupportInitialize)numKopie).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCena).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}