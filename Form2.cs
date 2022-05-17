using System;
using System.IO;
using System.Windows.Forms;

namespace BiKasa
{
    public partial class YeniKlasorEkrani : Form
    {
        private string[] dosyalar;

        public YeniKlasorEkrani()
        {
            InitializeComponent();
        }

        private void Iptal_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
        private void Ekle_Click(object sender, EventArgs e)
        {
            if (klasorAdi.Text == "")           //Klasör adının boş olup olmadığını kontrol eder
                MessageBox.Show("Klasör Adını Giriniz.", "Dikkat");
            else if (dosyalar == null || dosyalar.Length == 0)          //Seçilen klasörün boş olup olmadığını kontrol eder
                MessageBox.Show("Fotoğrafların Bulunduğu Klasörün Konumunu Giriniz", "Dikkat");
            else if (klasorKonum.Text.Contains("\\") ||         //Konumda tarihte ve etikelerde '\' karakterini kontrol eder
                     klasorTarih.Text.Contains("\\") || 
                     etiket1.Text.Contains("\\") ||
                     etiket2.Text.Contains("\\") ||
                     etiket3.Text.Contains("\\") ||
                     etiket4.Text.Contains("\\") ||
                     etiket5.Text.Contains("\\"))
                MessageBox.Show("Lütfen '\\' Karakterini Kullanmayınız.", "Dikkat");
            else            
            {
                bool kontrol = true;
                foreach (Klasor klasor in AnaProgram.Klasorler)         //Klasör adının diğer klasörlerle aynı olup olmadığını kontrol eder
                    if (klasorAdi.Text == klasor.Ad)
                    {
                        MessageBox.Show("Bu Klasör Adi Kullanılmakta.", "Dikkat");
                        kontrol = false;
                        break;
                    }
                if (kontrol)            //Klasör adı eşsiz iise çalışır
                {
                    try
                    {
                        Klasor klasor = new Klasor();           //Klasor değişkeni oluşturup bilgilerini rame kaydeder
                        klasor.Ad = klasorAdi.Text;
                        klasor.Konum = klasorKonum.Text;
                        klasor.Etiket = String.Format("{0}\\{1}\\{2}\\{3}\\{4}", etiket1.Text, etiket2.Text, etiket3.Text, etiket4.Text, etiket5.Text);
                        klasor.Tarih = klasorTarih.Text;
                        klasor.TxtBulunduguYol = @"C:\BiKasa\" + klasorAdi.Text + ".txt";
                        int j = 1;
                        for (int i = 0; i < dosyalar.Length; i++)           //Seçilen klasördeki fotoğrafları klasor içindeki Fotoğraflar listesine ekler
                        {
                            if (dosyalar[i].EndsWith(".jpg") ||         //Uzantıları kontrol eder
                                dosyalar[i].EndsWith(".jpeg") ||
                                dosyalar[i].EndsWith(".png") ||
                                dosyalar[i].EndsWith(".gif") ||
                                dosyalar[i].EndsWith(".tif") ||
                                dosyalar[i].EndsWith(".tiff") ||
                                dosyalar[i].EndsWith(".bmp"))
                            {
                                Resim tempResim = new Resim();          //Resim değişkeni oluşturup bilgilerini rame kaydeder
                                tempResim.BulunduguYol = dosyalar[i];
                                tempResim.Ad = String.Format("[{0}]-{1}", klasorAdi.Text, j);
                                j++;
                                tempResim.Konum = klasor.Konum;
                                tempResim.Etiket = klasor.Etiket;
                                tempResim.Uzantisi = dosyalar[i].Substring(dosyalar[i].IndexOf('.'));
                                tempResim.BulunduguKlasorAdi = klasorAdi.Text;
                                tempResim.Tarih = klasor.Tarih;
                                AnaProgram.kayit.DurumDegistir(new Fotograf());             //DosyaKayit türünü fotoğrafa çevirir
                                tempResim.BulunduguYol = AnaProgram.kayit.Kaydet(tempResim, @"C:\BiKasa\"); //Resmi ve bilgilerini BiKasa klasörüne kaydeder
                                tempResim.TxtBulunduguYol = tempResim.BulunduguYol.Substring(0, tempResim.BulunduguYol.LastIndexOf(".") + 1) + "txt";

                                klasor.Fotograflar.Add(tempResim);
                            }
                        }
                        AnaProgram.kayit.DurumDegistir(new TxtDosyasi());           //DosyaKayit türünü txtye çevirir
                        klasor.TxtBulunduguYol = AnaProgram.kayit.Kaydet(klasor.Fotograflar[0], klasor.TxtBulunduguYol); //Klasör bilgilerini BiKasa klasörüne kaydeder
                        AnaProgram.Klasorler.Add(klasor);
                        AnaProgram.yeniKlasorKontrol = true;
                        this.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Dosyalar Kaydedilirken Bir Sorun Oluştu", "Dikkat");
                    }
                }
                
            }

        }
        private void Gozat_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)    //Klasör seçmek için diyalog oluşturur ve seçilen klasördeki dosyaları dosyalar string arrayine kaydeder
            {
                dosyalar = Directory.GetFiles(fbd.SelectedPath);
                if (dosyalar.Length == 0)               //Klasör boş ise önceden uyrır
                    MessageBox.Show("Klasör Boş", "Dikkat");
            }

        }
    }
}
