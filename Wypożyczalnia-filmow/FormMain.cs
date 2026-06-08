using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Wypożyczalnia_filmow
{
    public partial class FormMain : Form
    {
        private int _klientId = 1;
        private string _klientNazwa = "";

        private System.Windows.Forms.BindingSource bsWypozyczenia = new System.Windows.Forms.BindingSource();

        public FormMain()
        {
            InitializeComponent();
        }

        // ─────────────────────────────────────────────
        // ŁADOWANIE FORMULARZA
        // ─────────────────────────────────────────────

        private void FormMain_Load(object sender, EventArgs e)
        {
            ZaladujKlienta();
            ZaladujFilmy();
            ZaladujWypozyczenia();
            ZaladujZarzadzanie();
        }

        // ─────────────────────────────────────────────
        // KLIENT – warstwa połączeniowa (DataReader)
        // ─────────────────────────────────────────────

        private void ZaladujKlienta()
        {
            string STR_SELECT = "SELECT imie, nazwisko FROM klienci WHERE klientid = @id";

            try
            {
                using (NpgsqlConnection conn = Database.GetConnection())
                using (NpgsqlCommand cmd = new NpgsqlCommand(STR_SELECT, conn))
                {
                    cmd.Parameters.AddWithValue("@id", _klientId);
                    conn.Open();

                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            _klientNazwa = $"{dr.GetString(0)} {dr.GetString(1)}";
                            lblKlient.Text = $"Klient: {_klientNazwa}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────
        // KATALOG FILMÓW – SELECT z widoku v_katalog_filmow
        // ─────────────────────────────────────────────

        private void ZaladujFilmy(string filtr = "")
        {
            string STR_SELECT =
                "SELECT filmid, tytul, gatunek, rokprodukcji, rezyser, dostepnekopie, cenazadzien " +
                "FROM v_katalog_filmow " +
                "WHERE @filtr = '' OR LOWER(tytul)   LIKE '%' || LOWER(@filtr) || '%' " +
                "                  OR LOWER(gatunek) LIKE '%' || LOWER(@filtr) || '%' " +
                "ORDER BY tytul";

            try
            {
                DataSet dsB = new DataSet();

                using (NpgsqlConnection conn = Database.GetConnection())
                using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(STR_SELECT, conn))
                {
                    adp.SelectCommand.Parameters.AddWithValue("@filtr", filtr);
                    adp.Fill(dsB, "filmy");
                }

                bsFilmy.DataSource = dsB.Tables["filmy"];
                dgvFilmy.DataSource = bsFilmy;

                if (dgvFilmy.Columns["filmid"] != null)
                    dgvFilmy.Columns["filmid"].Visible = false;

                if (dgvFilmy.Columns["tytul"] != null) dgvFilmy.Columns["tytul"].HeaderText = "Tytuł";
                if (dgvFilmy.Columns["gatunek"] != null) dgvFilmy.Columns["gatunek"].HeaderText = "Gatunek";
                if (dgvFilmy.Columns["rokprodukcji"] != null) dgvFilmy.Columns["rokprodukcji"].HeaderText = "Rok";
                if (dgvFilmy.Columns["rezyser"] != null) dgvFilmy.Columns["rezyser"].HeaderText = "Reżyser";
                if (dgvFilmy.Columns["dostepnekopie"] != null) dgvFilmy.Columns["dostepnekopie"].HeaderText = "Dostępne kopie";
                if (dgvFilmy.Columns["cenazadzien"] != null) dgvFilmy.Columns["cenazadzien"].HeaderText = "Cena / dzień";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}");
            }
        }

        private void txtSzukaj_TextChanged(object sender, EventArgs e)
        {
            ZaladujFilmy(txtSzukaj.Text.Trim());
        }

        // ─────────────────────────────────────────────
        // WYPOŻYCZANIE – INSERT przez adapter + DataSet
        // ─────────────────────────────────────────────

        private void btnWypozycz_Click(object sender, EventArgs e)
        {
            if (dgvFilmy.CurrentRow == null) return;

            int filmId = Convert.ToInt32(dgvFilmy.CurrentRow.Cells["filmid"].Value);
            string tytul = dgvFilmy.CurrentRow.Cells["tytul"].Value.ToString()!;
            int kopie = Convert.ToInt32(dgvFilmy.CurrentRow.Cells["dostepnekopie"].Value);
            decimal cena = Convert.ToDecimal(dgvFilmy.CurrentRow.Cells["cenazadzien"].Value);

            if (kopie <= 0)
            {
                MessageBox.Show("Brak dostępnych kopii tego filmu.", "Niedostępny",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (LiczbaAktywnychWypozyczen() >= 3)
            {
                MessageBox.Show("Osiągnąłeś limit 3 aktywnych wypożyczeń.", "Limit wypożyczeń",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime terminZwrotu = DateTime.Today.AddDays(7);
            decimal kosztCalkowity = cena * 7;

            var wynik = MessageBox.Show(
                $"Film: {tytul}\n" +
                $"Termin zwrotu: {terminZwrotu:dd.MM.yyyy} (7 dni)\n" +
                $"Cena: {cena:F2} zł/dzień\n" +
                $"Łączny koszt: {kosztCalkowity:F2} zł\n\n" +
                "Czy potwierdzasz wypożyczenie?",
                "Potwierdzenie wypożyczenia",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (wynik != DialogResult.Yes) return;

            string STR_SELECT = "SELECT wypozyczenieid, klientid, filmid, datawypozyczenia, terminzwrotu, status " +
                                 "FROM wypozyczenia WHERE klientid = @klientId";
            string STR_INSERT = "INSERT INTO wypozyczenia (klientid, filmid, datawypozyczenia, terminzwrotu, status) " +
                                  "VALUES (@klientId, @filmId, @data, @termin, @status)";
            string STR_UPD_KOPIE = "UPDATE filmy SET dostepnekopie = dostepnekopie - 1 WHERE filmid = @filmId";

            try
            {
                DataSet dsB = new DataSet();
                DataSet dsF = new DataSet();

                using (NpgsqlConnection conn = Database.GetConnection())
                {
                    using (NpgsqlCommand cmdInsert = new NpgsqlCommand(STR_INSERT, conn))
                    {
                        cmdInsert.Parameters.Add("@klientId", NpgsqlTypes.NpgsqlDbType.Integer, 0, "klientid");
                        cmdInsert.Parameters.Add("@filmId", NpgsqlTypes.NpgsqlDbType.Integer, 0, "filmid");
                        cmdInsert.Parameters.Add("@data", NpgsqlTypes.NpgsqlDbType.Date, 0, "datawypozyczenia");
                        cmdInsert.Parameters.Add("@termin", NpgsqlTypes.NpgsqlDbType.Date, 0, "terminzwrotu");
                        cmdInsert.Parameters.Add("@status", NpgsqlTypes.NpgsqlDbType.Text, 0, "status");

                        using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(STR_SELECT, conn))
                        {
                            adp.SelectCommand.Parameters.AddWithValue("@klientId", _klientId);
                            adp.InsertCommand = cmdInsert;

                            adp.Fill(dsB, "wypozyczenia");

                            DataRow dr = dsB.Tables["wypozyczenia"].NewRow();
                            dr["klientid"] = _klientId;
                            dr["filmid"] = filmId;
                            dr["datawypozyczenia"] = DateOnly.FromDateTime(DateTime.Today);
                            dr["terminzwrotu"] = DateOnly.FromDateTime(terminZwrotu);
                            dr["status"] = "Aktywne";
                            dsB.Tables["wypozyczenia"].Rows.Add(dr);

                            if (dsB.HasChanges())
                                dsF = dsB.GetChanges();

                            if (dsF.HasErrors)
                            {
                                dsB.RejectChanges();
                                MessageBox.Show("Błąd w danych – wypożyczenie anulowane.", "Błąd danych",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            conn.Open();
                            adp.InsertCommand.Connection = conn;
                            adp.Update(dsF, "wypozyczenia");
                        }
                    }

                    // Zmniejszamy licznik dostępnych kopii
                    using (NpgsqlCommand cmdKopie = new NpgsqlCommand(STR_UPD_KOPIE, conn))
                    {
                        cmdKopie.Parameters.AddWithValue("@filmId", filmId);
                        cmdKopie.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Wypożyczenie zostało zarejestrowane!", "Sukces",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ZaladujFilmy(txtSzukaj.Text.Trim());
                ZaladujWypozyczenia();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wypożyczania:\n{ex.Message}", "Błąd",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Warstwa połączeniowa – ExecuteScalar, szybki odczyt pojedynczej wartości
        private int LiczbaAktywnychWypozyczen()
        {
            string STR_SELECT = "SELECT COUNT(*) FROM wypozyczenia WHERE klientid = @id AND status = 'Aktywne'";
            int wynik = 0;

            try
            {
                using (NpgsqlConnection conn = Database.GetConnection())
                using (NpgsqlCommand cmd = new NpgsqlCommand(STR_SELECT, conn))
                {
                    cmd.Parameters.AddWithValue("@id", _klientId);
                    conn.Open();
                    wynik = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}");
            }

            return wynik;
        }

        // ─────────────────────────────────────────────
        // MOJE WYPOŻYCZENIA – SELECT z widoku v_aktywne_wypozyczenia
        // ─────────────────────────────────────────────

        private void ZaladujWypozyczenia()
        {
            string STR_SELECT =
                "SELECT wypozyczenieid, filmid, tytul, datawypozyczenia, terminzwrotu, datazwrotu, status, kara " +
                "FROM v_aktywne_wypozyczenia " +
                "WHERE klientid = @id " +
                "ORDER BY terminzwrotu";

            try
            {
                DataSet dsB = new DataSet();

                using (NpgsqlConnection conn = Database.GetConnection())
                using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(STR_SELECT, conn))
                {
                    adp.SelectCommand.Parameters.AddWithValue("@id", _klientId);
                    adp.Fill(dsB, "wypozyczenia");
                }

                bsWypozyczenia.DataSource = dsB.Tables["wypozyczenia"];
                dgvWypozyczenia.DataSource = bsWypozyczenia;

                if (dgvWypozyczenia.Columns["wypozyczenieid"] != null)
                    dgvWypozyczenia.Columns["wypozyczenieid"].Visible = false;
                if (dgvWypozyczenia.Columns["filmid"] != null)
                    dgvWypozyczenia.Columns["filmid"].Visible = false;

                if (dgvWypozyczenia.Columns["tytul"] != null) dgvWypozyczenia.Columns["tytul"].HeaderText = "Tytuł";
                if (dgvWypozyczenia.Columns["datawypozyczenia"] != null) dgvWypozyczenia.Columns["datawypozyczenia"].HeaderText = "Data wypożyczenia";
                if (dgvWypozyczenia.Columns["terminzwrotu"] != null) dgvWypozyczenia.Columns["terminzwrotu"].HeaderText = "Termin zwrotu";
                if (dgvWypozyczenia.Columns["datazwrotu"] != null) dgvWypozyczenia.Columns["datazwrotu"].HeaderText = "Data zwrotu";
                if (dgvWypozyczenia.Columns["status"] != null) dgvWypozyczenia.Columns["status"].HeaderText = "Status";
                if (dgvWypozyczenia.Columns["kara"] != null) dgvWypozyczenia.Columns["kara"].HeaderText = "Kara (zł)";

                decimal lacznaKara = 0;
                foreach (DataRow row in dsB.Tables["wypozyczenia"].Rows)
                    lacznaKara += Convert.ToDecimal(row["kara"]);

                lblKara.Text = lacznaKara > 0
                    ? $"Łączna kara do zapłaty: {lacznaKara:F2} zł"
                    : "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────
        // ZWROT FILMU – UPDATE przez adapter + DataSet
        // ─────────────────────────────────────────────

        private void btnZwroc_Click(object sender, EventArgs e)
        {
            if (dgvWypozyczenia.CurrentRow == null) return;

            int wypozyczenieId = Convert.ToInt32(dgvWypozyczenia.CurrentRow.Cells["wypozyczenieid"].Value);
            string tytul = dgvWypozyczenia.CurrentRow.Cells["tytul"].Value.ToString()!;
            DateOnly terminDate = (DateOnly)dgvWypozyczenia.CurrentRow.Cells["terminzwrotu"].Value;
            DateTime termin = terminDate.ToDateTime(TimeOnly.MinValue);
            int filmId = Convert.ToInt32(dgvWypozyczenia.CurrentRow.Cells["filmid"].Value);
            int spoznienie = (DateTime.Today - termin).Days;

            string komunikat = $"Zwracasz film: {tytul}\n";
            if (spoznienie > 0)
                komunikat += $"Spóźnienie: {spoznienie} dni\n(kara zostanie naliczona automatycznie)\n";
            komunikat += "\nCzy potwierdzasz zwrot?";

            var wynik = MessageBox.Show(komunikat, "Potwierdzenie zwrotu",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (wynik != DialogResult.Yes) return;

            string STR_SELECT = "SELECT wypozyczenieid, datazwrotu, status FROM wypozyczenia WHERE wypozyczenieid = @id";
            string STR_UPDATE = "UPDATE wypozyczenia SET datazwrotu = @datazwrotu, status = @status WHERE wypozyczenieid = @wypozyczenieid";
            string STR_UPD_KOPIE = "UPDATE filmy SET dostepnekopie = dostepnekopie + 1 WHERE filmid = @filmId";

            try
            {
                DataSet dsB = new DataSet();
                DataSet dsF = new DataSet();

                using (NpgsqlConnection conn = Database.GetConnection())
                {
                    using (NpgsqlCommand cmdUpdate = new NpgsqlCommand(STR_UPDATE, conn))
                    {
                        cmdUpdate.Parameters.Add("@datazwrotu", NpgsqlTypes.NpgsqlDbType.Date, 0, "datazwrotu");
                        cmdUpdate.Parameters.Add("@status", NpgsqlTypes.NpgsqlDbType.Text, 0, "status");
                        cmdUpdate.Parameters.Add("@wypozyczenieid", NpgsqlTypes.NpgsqlDbType.Integer, 0, "wypozyczenieid");

                        using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(STR_SELECT, conn))
                        {
                            adp.SelectCommand.Parameters.AddWithValue("@id", wypozyczenieId);
                            adp.UpdateCommand = cmdUpdate;

                            adp.Fill(dsB, "wypozyczenia");

                            DataRow dr = dsB.Tables["wypozyczenia"].Rows[0];
                            dr["datazwrotu"] = DateOnly.FromDateTime(DateTime.Today);
                            dr["status"] = "Zwrócone";

                            if (dsB.HasChanges())
                                dsF = dsB.GetChanges();

                            if (dsF.HasErrors)
                            {
                                dsB.RejectChanges();
                                MessageBox.Show("Błąd w danych – zwrot anulowany.", "Błąd danych",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            conn.Open();
                            adp.UpdateCommand.Connection = conn;
                            adp.Update(dsF, "wypozyczenia");
                        }
                    }

                    // Przywracamy kopię do katalogu
                    using (NpgsqlCommand cmdKopie = new NpgsqlCommand(STR_UPD_KOPIE, conn))
                    {
                        cmdKopie.Parameters.AddWithValue("@filmId", filmId);
                        cmdKopie.ExecuteNonQuery();
                    }

                    // Naliczamy karę jeśli wystąpiło spóźnienie (RB4)
                    if (spoznienie > 0)
                    {
                        using (NpgsqlCommand cmdCena = new NpgsqlCommand(
                            "SELECT cenazadzien FROM filmy WHERE filmid = @filmId", conn))
                        {
                            cmdCena.Parameters.AddWithValue("@filmId", filmId);
                            decimal cena = Convert.ToDecimal(cmdCena.ExecuteScalar());
                            decimal kwotaKary = cena * spoznienie;

                            using (NpgsqlCommand cmdKara = new NpgsqlCommand(
                                "INSERT INTO kary (wypozyczenieid, kwota, czyoplacona, datanaliczenia) " +
                                "VALUES (@wypozyczenieId, @kwota, false, @data)", conn))
                            {
                                cmdKara.Parameters.AddWithValue("@wypozyczenieId", wypozyczenieId);
                                cmdKara.Parameters.AddWithValue("@kwota", kwotaKary);
                                cmdKara.Parameters.AddWithValue("@data", DateOnly.FromDateTime(DateTime.Today));
                                cmdKara.ExecuteNonQuery();
                            }
                        }
                    }
                }

                string msg = spoznienie > 0
                    ? $"Film zwrócony. Naliczono karę za {spoznienie} dni spóźnienia."
                    : "Film zwrócony. Dziękujemy!";

                MessageBox.Show(msg, "Zwrot zarejestrowany",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ZaladujFilmy(txtSzukaj.Text.Trim());
                ZaladujWypozyczenia();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zwrotu:\n{ex.Message}", "Błąd",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────
        // ZARZĄDZANIE KATALOGIEM – wyświetlanie
        // ─────────────────────────────────────────────

        private void ZaladujZarzadzanie()
        {
            string STR_SELECT =
                "SELECT filmid, tytul, gatunek, rokprodukcji, rezyser, dostepnekopie, cenazadzien " +
                "FROM v_katalog_filmow " +
                "ORDER BY tytul";

            try
            {
                DataSet dsB = new DataSet();

                using (NpgsqlConnection conn = Database.GetConnection())
                using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(STR_SELECT, conn))
                {
                    adp.Fill(dsB, "filmy");
                }

                bsZarzadzanie.DataSource = dsB.Tables["filmy"];
                dgvZarzadzanie.DataSource = bsZarzadzanie;

                if (dgvZarzadzanie.Columns["filmid"] != null)
                    dgvZarzadzanie.Columns["filmid"].Visible = false;

                if (dgvZarzadzanie.Columns["tytul"] != null) dgvZarzadzanie.Columns["tytul"].HeaderText = "Tytuł";
                if (dgvZarzadzanie.Columns["gatunek"] != null) dgvZarzadzanie.Columns["gatunek"].HeaderText = "Gatunek";
                if (dgvZarzadzanie.Columns["rokprodukcji"] != null) dgvZarzadzanie.Columns["rokprodukcji"].HeaderText = "Rok";
                if (dgvZarzadzanie.Columns["rezyser"] != null) dgvZarzadzanie.Columns["rezyser"].HeaderText = "Reżyser";
                if (dgvZarzadzanie.Columns["dostepnekopie"] != null) dgvZarzadzanie.Columns["dostepnekopie"].HeaderText = "Dostępne kopie";
                if (dgvZarzadzanie.Columns["cenazadzien"] != null) dgvZarzadzanie.Columns["cenazadzien"].HeaderText = "Cena / dzień";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────
        // DODAWANIE FILMU – otwiera FormDodajFilm
        // ─────────────────────────────────────────────

        private void btnDodajFilm_Click(object sender, EventArgs e)
        {
            FormDodajFilm formDodaj = new FormDodajFilm();
            formDodaj.ShowDialog(this);

            if (formDodaj.FilmZapisany)
            {
                ZaladujZarzadzanie();
                ZaladujFilmy(txtSzukaj.Text.Trim());
            }
        }

        // ─────────────────────────────────────────────
        // USUWANIE FILMU – DELETE przez adapter + DataSet
        // ─────────────────────────────────────────────

        private void btnUsunFilm_Click(object sender, EventArgs e)
        {
            if (dgvZarzadzanie.CurrentRow == null) return;

            int filmId = Convert.ToInt32(dgvZarzadzanie.CurrentRow.Cells["filmid"].Value);
            string tytul = dgvZarzadzanie.CurrentRow.Cells["tytul"].Value.ToString()!;

            // RB5: nie wolno usunąć filmu z aktywnymi wypożyczeniami
            if (MaAktywneWypozyczenia(filmId))
            {
                MessageBox.Show(
                    $"Film \"{tytul}\" ma aktywne wypożyczenia – nie można go usunąć.",
                    "Operacja niemożliwa",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var wynik = MessageBox.Show(
                $"Czy na pewno chcesz usunąć film:\n\"{tytul}\"?\n\nOperacja jest nieodwracalna.",
                "Potwierdzenie usunięcia",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (wynik != DialogResult.Yes) return;

            string STR_SELECT = "SELECT filmid FROM filmy WHERE filmid = @id";
            string STR_DELETE = "DELETE FROM filmy WHERE filmid = @id";

            try
            {
                DataSet dataSetBase = new DataSet();
                DataSet dataSetFinal = new DataSet();

                using (NpgsqlConnection conn = Database.GetConnection())
                using (NpgsqlCommand cmdDelete = new NpgsqlCommand(STR_DELETE, conn))
                {
                    cmdDelete.Parameters.AddWithValue("@id", filmId);

                    using (NpgsqlDataAdapter adp = new NpgsqlDataAdapter(STR_SELECT, conn))
                    {
                        adp.SelectCommand.Parameters.AddWithValue("@id", filmId);
                        adp.DeleteCommand = cmdDelete;

                        adp.Fill(dataSetBase, "filmy");

                        // Oznaczamy wiersz jako usunięty w DataSet
                        dataSetBase.Tables["filmy"].Rows[0].Delete();

                        if (dataSetBase.HasChanges())
                            dataSetFinal = dataSetBase.GetChanges();

                        if (dataSetFinal.HasErrors)
                        {
                            dataSetBase.RejectChanges();
                            MessageBox.Show("Błąd w danych lokalnych.", "Błąd",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        conn.Open();
                        adp.DeleteCommand.Connection = conn;
                        adp.Update(dataSetFinal, "filmy");
                    }
                }

                MessageBox.Show($"Film \"{tytul}\" został usunięty z katalogu.", "Usunięto",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ZaladujZarzadzanie();
                ZaladujFilmy(txtSzukaj.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas usuwania:\n{ex.Message}", "Błąd",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // RB5: sprawdzamy aktywne wypożyczenia przez zapytanie, nie przez liczbę kopii
        private bool MaAktywneWypozyczenia(int filmId)
        {
            string STR_SELECT = "SELECT COUNT(*) FROM wypozyczenia WHERE filmid = @filmId AND status = 'Aktywne'";

            try
            {
                using (NpgsqlConnection conn = Database.GetConnection())
                using (NpgsqlCommand cmd = new NpgsqlCommand(STR_SELECT, conn))
                {
                    cmd.Parameters.AddWithValue("@filmId", filmId);
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}");
                // W razie błędu blokujemy usunięcie – bezpieczniejsze zachowanie
                return true;
            }
        }
    }
}