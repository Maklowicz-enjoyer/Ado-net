using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Wypożyczalnia_filmow
{
    public partial class FormMain : Form
    {
        // ID zalogowanego klienta – zmień na mechanizm logowania gdy będziesz gotowy
        private int _klientId = 1;
        private string _klientNazwa = "";

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
        }

        // ─────────────────────────────────────────────
        // KLIENT
        // ─────────────────────────────────────────────

        private void ZaladujKlienta()
        {
            string sql = @"SELECT Imie, Nazwisko FROM Klienci WHERE KlientID = @id";

            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", _klientId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                _klientNazwa = $"{reader["Imie"]} {reader["Nazwisko"]}";
                lblKlient.Text = $"Klient: {_klientNazwa}";
            }
        }

        // ─────────────────────────────────────────────
        // KATALOG FILMÓW
        // ─────────────────────────────────────────────

        private void ZaladujFilmy(string filtr = "")
        {
            string sql = @"
                SELECT f.FilmID, f.Tytul, g.Nazwa AS Gatunek, f.RokProdukcji,
                       f.Rezyser, f.DostepneKopie, f.CenaZaDzien
                FROM Filmy f
                JOIN Gatunki g ON f.GatunekID = g.GatunekID
                WHERE (@filtr = '' OR LOWER(f.Tytul) LIKE '%' || LOWER(@filtr) || '%'
                               OR LOWER(g.Nazwa) LIKE '%' || LOWER(@filtr) || '%')
                ORDER BY f.Tytul";

            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@filtr", filtr);

            var adapter = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);

            // Podpinamy do BindingSource
            bsFilmy.DataSource = dt;
            dgvFilmy.DataSource = bsFilmy;

            // Ukryj kolumnę ID (potrzebna w kodzie, nie dla użytkownika)
            if (dgvFilmy.Columns["FilmID"] != null)
                dgvFilmy.Columns["FilmID"].Visible = false;

            // Nazwy kolumn po polsku
            if (dgvFilmy.Columns["Tytul"] != null)       dgvFilmy.Columns["Tytul"].HeaderText       = "Tytuł";
            if (dgvFilmy.Columns["Gatunek"] != null)     dgvFilmy.Columns["Gatunek"].HeaderText     = "Gatunek";
            if (dgvFilmy.Columns["RokProdukcji"] != null)dgvFilmy.Columns["RokProdukcji"].HeaderText= "Rok";
            if (dgvFilmy.Columns["Rezyser"] != null)     dgvFilmy.Columns["Rezyser"].HeaderText     = "Reżyser";
            if (dgvFilmy.Columns["DostepneKopie"] != null)dgvFilmy.Columns["DostepneKopie"].HeaderText = "Dostępne kopie";
            if (dgvFilmy.Columns["CenaZaDzien"] != null) dgvFilmy.Columns["CenaZaDzien"].HeaderText = "Cena / dzień";
        }

        private void txtSzukaj_TextChanged(object sender, EventArgs e)
        {
            ZaladujFilmy(txtSzukaj.Text.Trim());
        }

        // ─────────────────────────────────────────────
        // WYPOŻYCZANIE
        // ─────────────────────────────────────────────

        private void btnWypozycz_Click(object sender, EventArgs e)
        {
            if (dgvFilmy.CurrentRow == null) return;

            int filmId     = Convert.ToInt32(dgvFilmy.CurrentRow.Cells["FilmID"].Value);
            string tytul   = dgvFilmy.CurrentRow.Cells["Tytul"].Value.ToString()!;
            int kopie      = Convert.ToInt32(dgvFilmy.CurrentRow.Cells["DostepneKopie"].Value);
            decimal cena   = Convert.ToDecimal(dgvFilmy.CurrentRow.Cells["CenaZaDzien"].Value);

            if (kopie <= 0)
            {
                MessageBox.Show("Brak dostępnych kopii tego filmu.", "Niedostępny",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Sprawdź czy klient nie ma za dużo aktywnych wypożyczeń (limit 3)
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

            using var conn = Database.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction();

            try
            {
                // Dodaj wypożyczenie
                string sqlWypozyczenie = @"
                    INSERT INTO Wypozyczenia (KlientID, FilmID, DataWypozyczenia, TerminZwrotu, Status)
                    VALUES (@klientId, @filmId, @data, @termin, 'Aktywne')";

                using var cmdW = new NpgsqlCommand(sqlWypozyczenie, conn, trans);
                cmdW.Parameters.AddWithValue("@klientId", _klientId);
                cmdW.Parameters.AddWithValue("@filmId", filmId);
                cmdW.Parameters.AddWithValue("@data", DateTime.Today);
                cmdW.Parameters.AddWithValue("@termin", terminZwrotu);
                cmdW.ExecuteNonQuery();

                // Zmniejsz liczbę dostępnych kopii
                string sqlKopie = @"
                    UPDATE Filmy SET DostepneKopie = DostepneKopie - 1
                    WHERE FilmID = @filmId";

                using var cmdK = new NpgsqlCommand(sqlKopie, conn, trans);
                cmdK.Parameters.AddWithValue("@filmId", filmId);
                cmdK.ExecuteNonQuery();

                trans.Commit();

                MessageBox.Show("Wypożyczenie zostało zarejestrowane!", "Sukces",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ZaladujFilmy(txtSzukaj.Text.Trim());
                ZaladujWypozyczenia();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                MessageBox.Show($"Błąd podczas wypożyczania:\n{ex.Message}", "Błąd",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int LiczbaAktywnychWypozyczen()
        {
            string sql = @"
                SELECT COUNT(*) FROM Wypozyczenia
                WHERE KlientID = @id AND Status = 'Aktywne'";

            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", _klientId);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // ─────────────────────────────────────────────
        // MOJE WYPOŻYCZENIA
        // ─────────────────────────────────────────────

        private void ZaladujWypozyczenia()
        {
            string sql = @"
                SELECT w.WypozyczenieID, f.Tytul,
                       w.DataWypozyczenia, w.TerminZwrotu,
                       w.DataZwrotu, w.Status,
                       COALESCE(SUM(k.Kwota), 0) AS Kara
                FROM Wypozyczenia w
                JOIN Filmy f ON w.FilmID = f.FilmID
                LEFT JOIN Kary k ON w.WypozyczenieID = k.WypozyczenieID AND k.CzyOplacona = false
                WHERE w.KlientID = @id AND w.Status = 'Aktywne'
                GROUP BY w.WypozyczenieID, f.Tytul, w.DataWypozyczenia, w.TerminZwrotu,
                         w.DataZwrotu, w.Status
                ORDER BY w.TerminZwrotu";

            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", _klientId);

            var adapter = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);

            dgvWypozyczenia.DataSource = dt;

            if (dgvWypozyczenia.Columns["WypozyczenieID"] != null)
                dgvWypozyczenia.Columns["WypozyczenieID"].Visible = false;

            if (dgvWypozyczenia.Columns["Tytul"] != null)           dgvWypozyczenia.Columns["Tytul"].HeaderText           = "Tytuł";
            if (dgvWypozyczenia.Columns["DataWypozyczenia"] != null) dgvWypozyczenia.Columns["DataWypozyczenia"].HeaderText = "Data wypożyczenia";
            if (dgvWypozyczenia.Columns["TerminZwrotu"] != null)     dgvWypozyczenia.Columns["TerminZwrotu"].HeaderText     = "Termin zwrotu";
            if (dgvWypozyczenia.Columns["DataZwrotu"] != null)       dgvWypozyczenia.Columns["DataZwrotu"].HeaderText       = "Data zwrotu";
            if (dgvWypozyczenia.Columns["Status"] != null)           dgvWypozyczenia.Columns["Status"].HeaderText           = "Status";
            if (dgvWypozyczenia.Columns["Kara"] != null)             dgvWypozyczenia.Columns["Kara"].HeaderText             = "Kara (zł)";

            // Pokaż łączną karę
            decimal lacznaKara = 0;
            foreach (DataRow row in dt.Rows)
                lacznaKara += Convert.ToDecimal(row["Kara"]);

            lblKara.Text = lacznaKara > 0
                ? $"Łączna kara do zapłaty: {lacznaKara:F2} zł"
                : "";
        }

        // ─────────────────────────────────────────────
        // ZWROT FILMU
        // ─────────────────────────────────────────────

        private void btnZwroc_Click(object sender, EventArgs e)
        {
            if (dgvWypozyczenia.CurrentRow == null) return;

            int wypozyczenieId = Convert.ToInt32(dgvWypozyczenia.CurrentRow.Cells["WypozyczenieID"].Value);
            string tytul       = dgvWypozyczenia.CurrentRow.Cells["Tytul"].Value.ToString()!;
            DateTime termin = ((DateOnly)dgvWypozyczenia.CurrentRow.Cells["TerminZwrotu"].Value).ToDateTime(TimeOnly.MinValue);

            int spoznienie = (DateTime.Today - termin).Days;
            string komunikat = $"Zwracasz film: {tytul}\n";

            if (spoznienie > 0)
                komunikat += $"Spóźnienie: {spoznienie} dni\n(kara zostanie naliczona automatycznie)\n";

            komunikat += "\nCzy potwierdzasz zwrot?";

            var wynik = MessageBox.Show(komunikat, "Potwierdzenie zwrotu",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (wynik != DialogResult.Yes) return;

            using var conn = Database.GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction();

            try
            {
                // Pobierz FilmID żeby zwiększyć kopie
                int filmId;
                using (var cmdF = new NpgsqlCommand(
                    "SELECT FilmID FROM Wypozyczenia WHERE WypozyczenieID = @id", conn, trans))
                {
                    cmdF.Parameters.AddWithValue("@id", wypozyczenieId);
                    filmId = Convert.ToInt32(cmdF.ExecuteScalar());
                }

                // Zaktualizuj wypożyczenie
                string sqlZwrot = @"
                    UPDATE Wypozyczenia
                    SET DataZwrotu = @data, Status = 'Zwrócone'
                    WHERE WypozyczenieID = @id";

                using var cmdZ = new NpgsqlCommand(sqlZwrot, conn, trans);
                cmdZ.Parameters.AddWithValue("@data", DateTime.Today);
                cmdZ.Parameters.AddWithValue("@id", wypozyczenieId);
                cmdZ.ExecuteNonQuery();

                // Zwiększ dostępne kopie
                using var cmdK = new NpgsqlCommand(
                    "UPDATE Filmy SET DostepneKopie = DostepneKopie + 1 WHERE FilmID = @filmId",
                    conn, trans);
                cmdK.Parameters.AddWithValue("@filmId", filmId);
                cmdK.ExecuteNonQuery();

                // Nalicz karę jeśli po terminie
                if (spoznienie > 0)
                {
                    // Pobierz CenaZaDzien
                    decimal cena;
                    using (var cmdC = new NpgsqlCommand(
                        "SELECT CenaZaDzien FROM Filmy WHERE FilmID = @filmId", conn, trans))
                    {
                        cmdC.Parameters.AddWithValue("@filmId", filmId);
                        cena = Convert.ToDecimal(cmdC.ExecuteScalar());
                    }

                    decimal kwotaKary = cena * spoznienie;

                    string sqlKara = @"
                        INSERT INTO Kary (WypozyczenieID, Kwota, CzyOplacona, DataNaliczenia)
                        VALUES (@wypozyczenieId, @kwota, false, @data)";

                    using var cmdKara = new NpgsqlCommand(sqlKara, conn, trans);
                    cmdKara.Parameters.AddWithValue("@wypozyczenieId", wypozyczenieId);
                    cmdKara.Parameters.AddWithValue("@kwota", kwotaKary);
                    cmdKara.Parameters.AddWithValue("@data", DateTime.Today);
                    cmdKara.ExecuteNonQuery();
                }

                trans.Commit();

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
                trans.Rollback();
                MessageBox.Show($"Błąd podczas zwrotu:\n{ex.Message}", "Błąd",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}