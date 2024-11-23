using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace AppEntriPemasukanPengeluaran
{
    public partial class FrmNasabah : Form
    {
        public FrmNasabah()
        {
            InitializeComponent();
            InisialisasiListView();
        }

        private void FrmNasabah_Load(object sender, EventArgs e)
        {
            LoadNasabahData();
            LoadTransaksiData();
        }

        // Atur kolom ListView
        private void InisialisasiListView()
        {
            lvwHistoriPemasukanPengeluaran.View = View.Details;
            lvwHistoriPemasukanPengeluaran.FullRowSelect = true;
            lvwHistoriPemasukanPengeluaran.GridLines = true;

            lvwHistoriPemasukanPengeluaran.Columns.Add("No.", 40, HorizontalAlignment.Center);
            lvwHistoriPemasukanPengeluaran.Columns.Add("Tanggal", 100, HorizontalAlignment.Center);
            lvwHistoriPemasukanPengeluaran.Columns.Add("Debit", 100, HorizontalAlignment.Right);
            lvwHistoriPemasukanPengeluaran.Columns.Add("Kredit", 100, HorizontalAlignment.Right);
        }

        // Load data nasabah
        private void LoadNasabahData()
        {
            using (var connection = new SQLiteConnection(Config.ConnectionString))
            {
                connection.Open();
                string query = "SELECT NoRekening, NamaNasabah FROM Nasabah WHERE NoRekening = @NoRekening";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NoRekening", "23.11.5755");

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtNoRekening.Text = reader["NoRekening"].ToString();
                            txtNasabah.Text = reader["NamaNasabah"].ToString();
                        }
                    }
                }
            }
        }

        // Load data transaksi
        private void LoadTransaksiData()
        {
            lvwHistoriPemasukanPengeluaran.Items.Clear();

            using (var connection = new SQLiteConnection(Config.ConnectionString))
            {
                connection.Open();
                string query = "SELECT Tanggal, Debit, Kredit FROM Transaksi WHERE NoRekening = @NoRekening";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NoRekening", txtNoRekening.Text);

                    using (var reader = command.ExecuteReader())
                    {
                        int no = 1;
                        while (reader.Read())
                        {
                            var item = new ListViewItem(no.ToString());
                            item.SubItems.Add(DateTime.Parse(reader["Tanggal"].ToString()).ToString("dd/MM/yyyy"));
                            item.SubItems.Add(Convert.ToDecimal(reader["Debit"]).ToString("N0"));
                            item.SubItems.Add(Convert.ToDecimal(reader["Kredit"]).ToString("N0"));
                            lvwHistoriPemasukanPengeluaran.Items.Add(item);
                            no++;
                        }
                    }
                }
            }
        }

        // Event tombol Input Pemasukan/Pengeluaran
        private void btnInputPemasukanPengeluaran_Click(object sender, EventArgs e)
        {
            var formEntri = new FrmEntriPemasukanPengeluaran("Input Transaksi", txtNoRekening.Text, txtNasabah.Text);
            formEntri.FormClosed += (s, args) => LoadTransaksiData(); // Refresh data setelah form ditutup
            formEntri.ShowDialog();
        }
    }
}
