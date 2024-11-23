using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace AppEntriPemasukanPengeluaran
{
    public partial class FrmEntriPemasukanPengeluaran : Form
    {
        // Constructor default
        public FrmEntriPemasukanPengeluaran()
        {
            InitializeComponent();
            cmbDebitKredit.SelectedIndex = 0; // Default pilihan pertama
        }

        // Constructor dengan parameter
        public FrmEntriPemasukanPengeluaran(string header, string noRekening, string nasabah)
            : this() // Panggil constructor default
        {
            this.Text = header;
            lblHeader.Text = header;

            txtNoRekening.Text = noRekening;
            txtNasabah.Text = nasabah;
        }

        // Event tombol Simpan
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtNominal.Text, out decimal nominal))
            {
                MessageBox.Show("Nominal tidak valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string jenis = cmbDebitKredit.SelectedItem.ToString();
            decimal debit = jenis == "Debit" ? nominal : 0;
            decimal kredit = jenis == "Kredit" ? nominal : 0;

            using (var connection = new SQLiteConnection(Config.ConnectionString))
            {
                connection.Open();

                string query = "INSERT INTO Transaksi (NoRekening, Tanggal, Debit, Kredit) VALUES (@NoRekening, @Tanggal, @Debit, @Kredit)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NoRekening", txtNoRekening.Text);
                    command.Parameters.AddWithValue("@Tanggal", dtpTanggal.Value.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@Debit", debit);
                    command.Parameters.AddWithValue("@Kredit", kredit);

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Transaksi berhasil disimpan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        // Event tombol Selesai
        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
