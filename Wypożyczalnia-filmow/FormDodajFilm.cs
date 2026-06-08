using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Wypożyczalnia_filmow
{
    public partial class FormDodajFilm : Form
    {
        public bool FilmZapisany { get; private set; } = false;

        public FormDodajFilm()
        {
            InitializeComponent();
        }

        // ─────────────────────────────────────────────
        // ŁADOWANIE – wypełnienie ComboBox gatunkami z bazy
        // ─────────────────────────────────────────────

        private void FormDodajFilm_Load(object sender, EventArgs e)
        {
            ZaladujGatunki();
        }

        private void ZaladujGatunki()
        {
            string STR_SELECT = "SELECT gatunekid, nazwa FROM gatunki ORDER BY nazwa";

            try
            {
                DataSet dsB = new DataSet();

                using (NpgsqlConnection conn = Database.GetConnection())
                using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(STR_SELECT, conn))
                {
                    adp.Fill(dsB, "gatunki");
                }

                cmbGatunek.DataSource = dsB.Tables["gatunki"];
                cmbGatunek.DisplayMember = "nazwa";
                cmbGatunek.ValueMember = "gatunekid";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd ładowania gatunków:\n{ex.Message}");
            }
        }

        // ─────────────────────────────────────────────
        // ZAPIS – INSERT przez adapter + DataSet
        // ─────────────────────────────────────────────

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTytul.Text))
            {
                MessageBox.Show("Tytuł nie może być pusty.", "Walidacja",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbGatunek.SelectedValue == null)
            {
                MessageBox.Show("Wybierz gatunek.", "Walidacja",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string STR_SELECT = "SELECT filmid, tytul, gatunekid, rokprodukcji, rezyser, dostepnekopie, cenazadzien " +
                                 "FROM filmy WHERE 1=0";
            string STR_INSERT = "INSERT INTO filmy (tytul, gatunekid, rokprodukcji, rezyser, dostepnekopie, cenazadzien) " +
                                 "VALUES (@tytul, @gatunekid, @rok, @rezyser, @kopie, @cena)";

            try
            {
                DataSet dsB = new DataSet();
                DataSet dsF = new DataSet();

                using (NpgsqlConnection conn = Database.GetConnection())
                using (NpgsqlCommand cmdInsert = new NpgsqlCommand(STR_INSERT, conn))
                {
                    cmdInsert.Parameters.Add("@tytul", NpgsqlTypes.NpgsqlDbType.Varchar, 200, "tytul");
                    cmdInsert.Parameters.Add("@gatunekid", NpgsqlTypes.NpgsqlDbType.Integer, 0, "gatunekid");
                    cmdInsert.Parameters.Add("@rok", NpgsqlTypes.NpgsqlDbType.Integer, 0, "rokprodukcji");
                    cmdInsert.Parameters.Add("@rezyser", NpgsqlTypes.NpgsqlDbType.Varchar, 200, "rezyser");
                    cmdInsert.Parameters.Add("@kopie", NpgsqlTypes.NpgsqlDbType.Integer, 0, "dostepnekopie");
                    cmdInsert.Parameters.Add("@cena", NpgsqlTypes.NpgsqlDbType.Numeric, 0, "cenazadzien");

                    using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(STR_SELECT, conn))
                    {
                        adp.InsertCommand = cmdInsert;

                        adp.Fill(dsB, "filmy");

                        DataRow dr = dsB.Tables["filmy"].NewRow();
                        dr["tytul"] = txtTytul.Text.Trim();
                        dr["gatunekid"] = Convert.ToInt32(cmbGatunek.SelectedValue);
                        dr["rokprodukcji"] = Convert.ToInt32(numRok.Value);
                        dr["rezyser"] = txtRezyser.Text.Trim();
                        dr["dostepnekopie"] = Convert.ToInt32(numKopie.Value);
                        dr["cenazadzien"] = numCena.Value;
                        dsB.Tables["filmy"].Rows.Add(dr);

                        if (dsB.HasChanges())
                            dsF = dsB.GetChanges();

                        if (dsF.HasErrors)
                        {
                            dsB.RejectChanges();
                            MessageBox.Show("Błąd w danych – film nie został zapisany.", "Błąd danych",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        conn.Open();
                        adp.InsertCommand.Connection = conn;
                        adp.Update(dsF, "filmy");
                    }
                }

                FilmZapisany = true;

                MessageBox.Show($"Film \"{txtTytul.Text.Trim()}\" został dodany do katalogu.", "Sukces",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisu:\n{ex.Message}", "Błąd",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────
        // ANULUJ – zamknij bez zapisu
        // ─────────────────────────────────────────────

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}