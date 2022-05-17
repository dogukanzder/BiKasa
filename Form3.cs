using System;
using System.Windows.Forms;

namespace BiKasa
{
    public partial class KonumEtiketDuzenle : Form
    {
        public string Konum = "";
        public string Etiket1 = "";
        public string Etiket2 = "";
        public string Etiket3 = "";
        public string Etiket4 = "";
        public string Etiket5 = "";

        public KonumEtiketDuzenle()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            konum.Text = Konum;
            etiket1.Text = Etiket1;
            etiket2.Text = Etiket2;
            etiket3.Text = Etiket3;
            etiket4.Text = Etiket4;
            etiket5.Text = Etiket5;
        }
        private void Iptal_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Kaydet_Click(object sender, EventArgs e)
        {
            if (Konum == konum.Text &&          //Değişiklik varmı diye kontrol eder
                Etiket1 == etiket1.Text &&
                Etiket2 == etiket2.Text &&
                Etiket3 == etiket3.Text &&
                Etiket4 == etiket4.Text &&
                Etiket5 == etiket5.Text)
                MessageBox.Show("Kaydedilecek Değişiklik Yok", "Dikkat");
            else if (konum.Text.Contains("\\") ||           //'\' karakteri varmı diye kontrol eder
                     etiket1.Text.Contains("\\") ||
                     etiket2.Text.Contains("\\") ||
                     etiket3.Text.Contains("\\") ||
                     etiket4.Text.Contains("\\") ||
                     etiket5.Text.Contains("\\"))
                MessageBox.Show("Lütfen '\\' Karakterini Kullanmayınız.", "Dikkat");
            else            //Yeni etiket ve konumları public değişkene kaydeder
            {
                Konum = konum.Text;
                Etiket1 = etiket1.Text;
                Etiket2 = etiket2.Text;
                Etiket3 = etiket3.Text;
                Etiket4 = etiket4.Text;
                Etiket5 = etiket5.Text;
                AnaProgram.konumEtiketDuzenleKontrol = true;
                this.Close();
            }
        }
    }
}
