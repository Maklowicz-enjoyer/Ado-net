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


        private void FormMain_Load(object sender, EventArgs e)
        {        }
        private void ZaladujKlienta(){
        }

        
        private void ZaladujFilmy(string filtr = "")
        {
           
        }

        private void txtSzukaj_TextChanged(object sender, EventArgs e)
        {

        }
        private void btnWypozycz_Click(object sender, EventArgs e)
        {
        }

        private void btnZwroc_Click(object sender, EventArgs e)
        {
          
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        { }
           

        private void ZaladujZarzadzanie()
        {
            
        }

        private void btnDodajFilm_Click(object sender, EventArgs e)
        {

        }

        private void btnUsunFilm_Click(object sender, EventArgs e)
        {
           
        }
    }
}