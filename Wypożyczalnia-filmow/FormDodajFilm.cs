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

        private void FormDodajFilm_Load(object sender, EventArgs e) { }
        private void btnZapisz_Click(object sender, EventArgs e) { }
        private void btnAnuluj_Click(object sender, EventArgs e) { }
    }
}