using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

//Doğukan Özder
//2018280060
//BiKasa

namespace BiKasa
{
    public partial class AnaProgram : Form
    {
        static public bool yeniKlasorKontrol = false;               //Yeni klasör gelip gelmediğini kontrol eder
        static public bool konumEtiketDuzenleKontrol = false;       //Konum veya etiketlerin değişip değişmediğini kontrol eder
        static public List<Klasor> Klasorler = new List<Klasor>();  //Programdaki klasör ve resim bilgilerinin tutulduğu liste
        static public DosyaKayit kayit = new DosyaKayit();          //Dosya kayıt işlemleri için
        private Dosya dosya = new Dosya();                          //Dosya classı için

        public AnaProgram()
        {
            InitializeComponent();
        }

        private void YeniKlasor_Click(object sender, EventArgs e)
        {
            YeniKlasorEkrani yeniKlasor = new YeniKlasorEkrani();   //Yeni klasör formunu açar

            yeniKlasor.Closed += delegate
            {
                if (yeniKlasorKontrol)          //Yeni klasör olup olmadığını kontrol eder
                {
                    klasorTreeMap.Nodes.Add(Klasorler[Klasorler.Count - 1].Ad);    //Yeni klasörü TreeMape ekler
                    yeniKlasorKontrol = false;
                }
            };
            yeniKlasor.ShowDialog();
        }

        private void KlasorTreeMap_AfterSelect(object sender, TreeViewEventArgs e)
        {
            fotografListeKutusu.Items.Clear();  //ListBoxı temizler
            
            if (resimKutusu.Image != null)      //PictureBoxı temizler
                resimKutusu.Image.Dispose();
            resimKutusu.Image = null;

            foreach (Klasor klasor in Klasorler)
            {
                if (klasor.Ad == klasorTreeMap.SelectedNode.Text)   //Seçilen klasörü bulur bilgilerini gösterir resimleri listBoxa ekler
                {
                    konum.Text = klasor.Konum;
                    tarih.Text = klasor.Tarih;
                    etiket.Text = klasor.Etiket;
                    for (int i = 0; i < klasor.Fotograflar.Count; i++)
                        fotografListeKutusu.Items.Add(klasor.Fotograflar[i].Ad);
                    break;
                }
            }
        }

        private void FotografListeKutusu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fotografListeKutusu.SelectedIndex >= 0)         //ListBoxın seçilip seçilmediğini kontrol eder
            {
                if (resimKutusu.Image != null)                  //pictureBoxı temizler
                    resimKutusu.Image.Dispose();
                resimKutusu.Image = null;

                string tempString = fotografListeKutusu.Items[fotografListeKutusu.SelectedIndex].ToString();
                foreach (Klasor klasor in Klasorler)
                {
                    foreach (Resim resim in klasor.Fotograflar)
                        if (resim.Ad == tempString)             //Seçilen resimi bulur pictureBoxta açar ve bilgilerini gösterir
                        {
                            if (File.Exists(resim.BulunduguYol))
                                resimKutusu.Image = Image.FromFile(resim.BulunduguYol);
                            konum.Text = resim.Konum;
                            tarih.Text = resim.Tarih;
                            etiket.Text = resim.Etiket;
                            break;
                        }
                }
            }
        }
        
        private void AnaProgram_Load(object sender, EventArgs e)
        {
            oncekiResim.ImageLocation = "onceki.png";           //Butonlar aslında pictureBox
            sonrakiResim.ImageLocation = "sonraki.png";         //Iconları pictureBoxlara koyar
            solaDondur.ImageLocation = "solaCevir.png";
            sagaDondur.ImageLocation = "sagaCevir.png";
            negatifeCevir.ImageLocation = "negatif.jpg";
            siyahBeyazCevir.ImageLocation = "siyahBeyaz.png";
            aynaCevir.ImageLocation = "ayna.png";
            RGCevir.ImageLocation = "rToG.png";
            RBCevir.ImageLocation = "rToB.jpg";
            GBCevir.ImageLocation = "gToB.jpg";
            KlasorAra.ImageLocation = "klasorAra.jpg";
            ResimAra.ImageLocation = "resimAra.jpg";
            etiketIcon.ImageLocation = "etiket.png";
            konumIcon.ImageLocation = "konum.png";
            tarihIcon.ImageLocation = "tarih.png";

            Random rnd = new Random();
            meyveIcon.ImageLocation = rnd.Next(4) switch        //Program her açıldığında rastgele logo koyar
            {
                0 => "armut.jpg",
                1 => "kiraz.jpg",
                2 => "domates.jpg",
                _ => "elma.png",
            };

            dosya.Yukle();              //BiKasa klasöründen fotoğraf ve kalsör bilgilerini alır
            if (Klasorler.Count > 0)
                foreach (Klasor klasor in Klasorler)    //TreeMape klasör isimlerini ekler
                    klasorTreeMap.Nodes.Add(klasor.Ad);
        }

        private void OncekiResim_Click(object sender, EventArgs e)
        {
            if (fotografListeKutusu.SelectedIndex > 0)      //ListBox seçilimi diye kontrol eder ve önceki indexe gider
                fotografListeKutusu.SetSelected(fotografListeKutusu.SelectedIndex - 1, true);
            else
                MessageBox.Show("Önceki Resim Yok", "Dikkat");
        }

        private void SonrakiResim_Click(object sender, EventArgs e)
        {
            if (fotografListeKutusu.SelectedIndex >= 0 && fotografListeKutusu.SelectedIndex < fotografListeKutusu.Items.Count - 1)  //ListBox seçilimi diye kontrol eder ve sonraki indexe gider
                fotografListeKutusu.SetSelected(fotografListeKutusu.SelectedIndex + 1, true);
            else
                MessageBox.Show("Sonraki Resim Yok", "Dikkat");
        }

        private void SolaDondur_Click(object sender, EventArgs e)
        {
            if (resimKutusu.Image != null && fotografListeKutusu.SelectedIndex >= 0)    //pictureBoxı kontrol edip resmi sola çevirir
            {
                Bitmap resim = new Bitmap(resimKutusu.Image);
                resim.RotateFlip(RotateFlipType.Rotate270FlipNone);
                resimKutusu.Image.Dispose();
                resimKutusu.Image = resim.Clone(
                new Rectangle(0, 0, resim.Width, resim.Height),
                System.Drawing.Imaging.PixelFormat.DontCare);
                resim.Dispose();
            }
            else
                MessageBox.Show("Seçili Resim Yok", "Dikkat");
        }

        private void SagaDondur_Click(object sender, EventArgs e)
        {
            if (resimKutusu.Image != null && fotografListeKutusu.SelectedIndex >= 0)    //PictureBoxı kontrol edip resmi sağa döndürür
            {
                Bitmap resim = new Bitmap(resimKutusu.Image);
                resim.RotateFlip(RotateFlipType.Rotate90FlipNone);
                resimKutusu.Image.Dispose();
                resimKutusu.Image = resim.Clone(
                new Rectangle(0, 0, resim.Width, resim.Height),
                System.Drawing.Imaging.PixelFormat.DontCare);
                resim.Dispose();
            }
            else
                MessageBox.Show("Seçili Resim Yok", "Dikkat");
        }

        private void NegatifeCevir_Click(object sender, EventArgs e)
        {
            if (resimKutusu.Image != null && fotografListeKutusu.SelectedIndex >= 0)    //PictureBoxı kontrol edip resmi negatife çevirir
            {
                Bitmap resim = new Bitmap(resimKutusu.Image);

                for (int i = 0; i < resim.Width; i++)
                {
                    for (int j = 0; j < resim.Height; j++)
                    {
                        Color p = resim.GetPixel(i, j);

                        int a = p.A;
                        int r = p.R;
                        int g = p.G;
                        int b = p.B;

                        r = 255 - r;
                        g = 255 - g;
                        b = 255 - b;

                        resim.SetPixel(i, j, Color.FromArgb(a, r, g, b));
                    }
                }
                
                resimKutusu.Image.Dispose();
                resimKutusu.Image = resim.Clone(
                new Rectangle(0, 0, resim.Width, resim.Height),
                System.Drawing.Imaging.PixelFormat.DontCare);
                resim.Dispose();
            }
            else
                MessageBox.Show("Seçili Resim Yok", "Dikkat");
        }

        private void SiyahBeyazCevir_Click(object sender, EventArgs e)
        {
            if (resimKutusu.Image != null && fotografListeKutusu.SelectedIndex >= 0) //PictureBoxı kontrol edip resmi siyah beyaza çevirir 
            {
                Bitmap resim = new Bitmap(resimKutusu.Image);
                int rgb;
                Color c;

                for (int i = 0; i < resim.Width; i++)
                {
                    for (int j = 0; j < resim.Height; j++)
                    {
                        c = resim.GetPixel(i, j);
                        rgb = (int)Math.Round(.299 * c.R + .587 * c.G + .114 * c.B);
                        resim.SetPixel(i, j, Color.FromArgb(rgb, rgb, rgb));
                    }
                }

                resimKutusu.Image.Dispose();
                resimKutusu.Image = resim.Clone(
                new Rectangle(0, 0, resim.Width, resim.Height),
                System.Drawing.Imaging.PixelFormat.DontCare);
                resim.Dispose();
            }
            else
                MessageBox.Show("Seçili Resim Yok", "Dikkat");
        }

        private void AynaCevir_Click(object sender, EventArgs e)
        {
            if (resimKutusu.Image != null && fotografListeKutusu.SelectedIndex >= 0)    //PictureBoxı kontrol edip resmi ayna görüntüsüne çevirir
            {
                Bitmap resim = new Bitmap(resimKutusu.Image);
                                
                resim.RotateFlip(RotateFlipType.RotateNoneFlipX);

                resimKutusu.Image.Dispose();
                resimKutusu.Image = resim.Clone(
                new Rectangle(0, 0, resim.Width, resim.Height),
                System.Drawing.Imaging.PixelFormat.DontCare);
                resim.Dispose();
            }
            else
                MessageBox.Show("Seçili Resim Yok", "Dikkat");
        }

        private void RGCevir_Click(object sender, EventArgs e)
        {
            if (resimKutusu.Image != null && fotografListeKutusu.SelectedIndex >= 0)    //PictureBoxı kontrol edip RGB yi GRB ye çevirir
            {
                Bitmap resim = new Bitmap(resimKutusu.Image);

                for (int i = 0; i < resim.Width; i++)
                {
                    for (int j = 0; j < resim.Height; j++)
                    {
                        Color p = resim.GetPixel(i, j);
                        
                        int r = p.R;
                        int g = p.G;
                        int b = p.B;
                        int temp = g;
                        g = r;
                        r = temp;

                        resim.SetPixel(i, j, Color.FromArgb(r, g, b));
                    }
                }

                resimKutusu.Image.Dispose();
                resimKutusu.Image = resim.Clone(
                new Rectangle(0, 0, resim.Width, resim.Height),
                System.Drawing.Imaging.PixelFormat.DontCare);
                resim.Dispose();
            }
            else
                MessageBox.Show("Seçili Resim Yok", "Dikkat");
        }

        private void RBCevir_Click(object sender, EventArgs e)
        {
            if (resimKutusu.Image != null && fotografListeKutusu.SelectedIndex >= 0)    //PictureBoxı kontrol edip RGB yi BGR ye çevirir
            {
                Bitmap resim = new Bitmap(resimKutusu.Image);

                for (int i = 0; i < resim.Width; i++)
                {
                    for (int j = 0; j < resim.Height; j++)
                    {
                        Color p = resim.GetPixel(i, j);

                        int r = p.R;
                        int g = p.G;
                        int b = p.B;
                        int temp = b;
                        b = r;
                        r = temp;

                        resim.SetPixel(i, j, Color.FromArgb(r, g, b));
                    }
                }

                resimKutusu.Image.Dispose();
                resimKutusu.Image = resim.Clone(
                new Rectangle(0, 0, resim.Width, resim.Height),
                System.Drawing.Imaging.PixelFormat.DontCare);
                resim.Dispose();
            }
            else
                MessageBox.Show("Seçili Resim Yok", "Dikkat");
        }

        private void GBCevir_Click(object sender, EventArgs e)
        {
            if (resimKutusu.Image != null && fotografListeKutusu.SelectedIndex >= 0)    //PictureBoxı kontrol edip RGB yi RBG ye çevirir
            {
                Bitmap resim = new Bitmap(resimKutusu.Image);

                for (int i = 0; i < resim.Width; i++)
                {
                    for (int j = 0; j < resim.Height; j++)
                    {
                        Color p = resim.GetPixel(i, j);

                        int r = p.R;
                        int g = p.G;
                        int b = p.B;
                        int temp = g;
                        g = b;
                        b = temp;

                        resim.SetPixel(i, j, Color.FromArgb(r, g, b));
                    }
                }

                resimKutusu.Image.Dispose();
                resimKutusu.Image = resim.Clone(
                new Rectangle(0, 0, resim.Width, resim.Height),
                System.Drawing.Imaging.PixelFormat.DontCare);
                resim.Dispose();
            }
            else
                MessageBox.Show("Seçili Resim Yok", "Dikkat");
        }

        private void ResmiKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (resimKutusu.Image != null && fotografListeKutusu.SelectedIndex >= 0)    //PictureBoxı ve listBoxı kontrol eder
                {
                    MessageBoxButtons dugmeler = MessageBoxButtons.YesNo;
                    DialogResult sonuc = MessageBox.Show("Resim Üzerine Kaydedilecektir. Eminmisiniz?", "Resmi Kaydet", dugmeler);
                    if (sonuc == DialogResult.Yes)         //Kullanıcıya eminmisin diye sorar eminse devam eder ve resmi üzerine yazar
                    {
                        Bitmap bitmap = new Bitmap(resimKutusu.Image);
                        Image fotograf = bitmap.Clone(
                        new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        System.Drawing.Imaging.PixelFormat.DontCare);

                        string tempString = fotografListeKutusu.Items[fotografListeKutusu.SelectedIndex].ToString();
                        resimKutusu.Image.Dispose();
                        resimKutusu.Image = null;
                        foreach (Klasor klasor in Klasorler)
                            foreach (Resim resim in klasor.Fotograflar)
                                if (resim.Ad == tempString)             //Seçilen resmi bulur eski resmi silip yenisini kaydeder
                                {
                                    File.Delete(resim.BulunduguYol);
                                    fotograf.Save(resim.BulunduguYol);
                                    fotograf.Dispose();
                                    bitmap.Dispose();
                                    resimKutusu.Image = Image.FromFile(resim.BulunduguYol);
                                    konum.Text = null;
                                    tarih.Text = null;
                                    etiket.Text = "Resim Kaydedildi.";
                                    break;
                                }
                    }
                }
                else
                    MessageBox.Show("Seçili Resim Yok", "Dikkat");
            }
            catch
            {
                MessageBox.Show("Resim Kaydedilemedi", "Dikkat");
            }
            
        }

        private void ResmiSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (resimKutusu.Image != null && fotografListeKutusu.SelectedIndex >= 0)     //PictureBoxı ve listBoxı kontrol eder
                {
                    MessageBoxButtons dugmeler = MessageBoxButtons.YesNo;
                    DialogResult sonuc = MessageBox.Show("Resim Silinecektir. Eminmisiniz?", "Resmi Sil", dugmeler);
                    if (sonuc == DialogResult.Yes)         //Kullanıcıya eminmisin diye sorar eminse devam eder ve resmi siler
                    {
                        string tempString = fotografListeKutusu.Items[fotografListeKutusu.SelectedIndex].ToString();
                        resimKutusu.Image.Dispose();
                        resimKutusu.Image = null;
                        foreach (Klasor klasor in Klasorler)
                            foreach (Resim resim in klasor.Fotograflar)
                                if (resim.Ad == tempString)         //Seçilen resmi bulup resmi ve bilgilerini siler
                                {
                                    File.Delete(resim.BulunduguYol);
                                    File.Delete(resim.BulunduguYol.Substring(0, resim.BulunduguYol.LastIndexOf(".")) + ".txt");
                                    fotografListeKutusu.Items.Remove(resim.Ad);
                                    klasor.Fotograflar.Remove(resim);
                                    konum.Text = null;
                                    tarih.Text = null;
                                    etiket.Text = "Resim Silindi.";
                                    break;
                                }
                    }
                }
                else
                    MessageBox.Show("Seçili Resim Yok", "Dikkat");
            }
            catch
            {
                MessageBox.Show("Resim Silinemedi", "Dikkat");
            }
        }

        private void KlasoruSil_Click(object sender, EventArgs e)
        {
            if (klasorTreeMap.SelectedNode != null && klasorTreeMap.SelectedNode.Index >= 0)    //TreeMapi kontrol eder
            {
                if (resimKutusu.Image != null)  //pictureBox doluysa temizler
                    resimKutusu.Image.Dispose();
                resimKutusu.Image = null;

                foreach (Klasor klasor in Klasorler)
                    if (klasor.Ad == klasorTreeMap.SelectedNode.Text)   //Seçilen klasörü bulur klasör bilgilerini içindeki resimleri ve resim bilgilerini siler
                    {
                        foreach (Resim resim in klasor.Fotograflar)
                        {
                            File.Delete(resim.BulunduguYol);
                            File.Delete(resim.TxtBulunduguYol);
                        }
                        File.Delete(klasor.TxtBulunduguYol);
                        fotografListeKutusu.Items.Clear();
                        Klasorler.Remove(klasor);
                        klasorTreeMap.SelectedNode.Remove();
                        konum.Text = null;
                        tarih.Text = null;
                        etiket.Text = "Klasör Silindi.";
                        break;
                    }
            }
            else
                MessageBox.Show("Seçili Klasör Yok", "Dikkat");
        }

        private void ResmiFarkliKaydet_Click(object sender, EventArgs e)
        {
            if (resimKutusu.Image != null && fotografListeKutusu.SelectedIndex >= 0)    //PictureBoxı ve listBoxı kontrol eder
            {
                SaveFileDialog sfdlg = new SaveFileDialog();
                sfdlg.Filter = "Image Dosyaları (*.jpg, *.png, *.bmp) | *.jpg;*.png;*.bmp";  
                sfdlg.OverwritePrompt = true;
                sfdlg.InitialDirectory = @"C:\";
                ImageFormat format = ImageFormat.Jpeg;
                if (sfdlg.ShowDialog() == DialogResult.OK)  //Kaydelicek yeri ve ismini kullanıcıdan diyalog ile alır
                {
                    string ext = System.IO.Path.GetExtension(sfdlg.FileName).ToLower();
                    switch (ext)
                    {
                        case ".png":
                            format = ImageFormat.Png;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                    }
                    resimKutusu.Image.Save(sfdlg.FileName, format);     //Resmi istenilen yere istenilen adda kaydeder
                    konum.Text = null;
                    etiket.Text = "Resim Kaydedildi.";
                }
            }
            else
                MessageBox.Show("Seçili Resim Yok", "Dikkat");
        }

        private void KonumEtiketDuzenle_Click(object sender, EventArgs e)
        {
            if (resimKutusu.Image != null && fotografListeKutusu.SelectedIndex >= 0) //Resim seçilip seçilmediğini kontrol eder
            {
                string tempString = fotografListeKutusu.Items[fotografListeKutusu.SelectedIndex].ToString();
                foreach (Klasor klasor in Klasorler)
                    foreach (Resim resim in klasor.Fotograflar)
                        if (resim.Ad == tempString)     //Seçilen resmi bulur ve konum etiket düzenle formunu açıp değişimleri kaydeder
                        {
                            KonumEtiketDuzenle ked = new KonumEtiketDuzenle();

                            ked.Konum = resim.Konum;
                            string tempEtiket = resim.Etiket;
                            ked.Etiket1 = tempEtiket.Substring(0, tempEtiket.IndexOf("\\"));
                            tempEtiket = tempEtiket.Substring(tempEtiket.IndexOf("\\") + 1);
                            ked.Etiket2 = tempEtiket.Substring(0, tempEtiket.IndexOf("\\"));
                            tempEtiket = tempEtiket.Substring(tempEtiket.IndexOf("\\") + 1);
                            ked.Etiket3 = tempEtiket.Substring(0, tempEtiket.IndexOf("\\"));
                            tempEtiket = tempEtiket.Substring(tempEtiket.IndexOf("\\") + 1);
                            ked.Etiket4 = tempEtiket.Substring(0, tempEtiket.IndexOf("\\"));
                            tempEtiket = tempEtiket.Substring(tempEtiket.IndexOf("\\") + 1);
                            ked.Etiket5 = tempEtiket;

                            ked.Closed += delegate
                            {
                                if (konumEtiketDuzenleKontrol)
                                {
                                    resim.Konum = ked.Konum;
                                    resim.Etiket = String.Format("{0}\\{1}\\{2}\\{3}\\{4}", ked.Etiket1, ked.Etiket2, ked.Etiket3, ked.Etiket4, ked.Etiket5);
                                    konum.Text = resim.Konum;
                                    etiket.Text = resim.Etiket;

                                    kayit.DurumDegistir(new TxtDosyasi());
                                    kayit.Kaydet(resim, resim.TxtBulunduguYol);
                                    konumEtiketDuzenleKontrol = false;
                                }
                            };
                            ked.ShowDialog();
                        }
                            
            }
            else if (klasorTreeMap.SelectedNode != null)
            {
                foreach (Klasor klasor in Klasorler)
                    if (klasor.Ad == klasorTreeMap.SelectedNode.Text)   //Seçilen klasörü bulur ve konum etiket düzenle formunu açıp değişenleri kaydeder
                    {
                        KonumEtiketDuzenle ked = new KonumEtiketDuzenle();

                        ked.Konum = klasor.Konum;
                        string tempEtiket = klasor.Etiket;
                        ked.Etiket1 = tempEtiket.Substring(0, tempEtiket.IndexOf("\\"));
                        tempEtiket = tempEtiket.Substring(tempEtiket.IndexOf("\\") + 1);
                        ked.Etiket2 = tempEtiket.Substring(0, tempEtiket.IndexOf("\\"));
                        tempEtiket = tempEtiket.Substring(tempEtiket.IndexOf("\\") + 1);
                        ked.Etiket3 = tempEtiket.Substring(0, tempEtiket.IndexOf("\\"));
                        tempEtiket = tempEtiket.Substring(tempEtiket.IndexOf("\\") + 1);
                        ked.Etiket4 = tempEtiket.Substring(0, tempEtiket.IndexOf("\\"));
                        tempEtiket = tempEtiket.Substring(tempEtiket.IndexOf("\\") + 1);
                        ked.Etiket5 = tempEtiket;

                        ked.Closed += delegate
                        {
                            if (konumEtiketDuzenleKontrol)
                            {
                                klasor.Konum = ked.Konum;
                                klasor.Etiket = String.Format("{0}\\{1}\\{2}\\{3}\\{4}", ked.Etiket1, ked.Etiket2, ked.Etiket3, ked.Etiket4, ked.Etiket5);
                                Resim resim = new Resim();
                                resim.Konum = klasor.Konum;
                                resim.Etiket = klasor.Etiket;
                                konum.Text = klasor.Konum;
                                etiket.Text = klasor.Etiket;

                                kayit.DurumDegistir(new TxtDosyasi());
                                kayit.Kaydet(resim, klasor.TxtBulunduguYol);
                                konumEtiketDuzenleKontrol = false;
                            }
                        };
                        ked.ShowDialog();
                    }
            }
            else
                MessageBox.Show("Seçili Resim Veya Klasör Yok", "Dikkat");
        }

        private void KlasorAra_Click(object sender, EventArgs e)
        {
            if (resimKutusu.Image != null)          //PictureBox doluysa temizler
                resimKutusu.Image.Dispose();
            resimKutusu.Image = null;

            konum.Text = null;
            etiket.Text = null;
            tarih.Text = null;
            klasorTreeMap.Nodes.Clear();
            fotografListeKutusu.Items.Clear();
            aramaCubugu.Text = aramaCubugu.Text.ToLower();
            if (aramaCubugu.Text == "")         //Arama çubuğu boş ise tüm klasörleri gösterir
            {
                if (Klasorler.Count > 0)
                    foreach (Klasor klasor in Klasorler)
                        klasorTreeMap.Nodes.Add(klasor.Ad);
            }
            else                                //Arama çubundaki metni veya karakteri tüm klasörlerin adı, konumu, etiketi veya tarihi içeriyorsa treeMape ekler
                foreach (Klasor klasor in Klasorler)
                    if (klasor.Ad.ToLower().Contains(aramaCubugu.Text) || 
                        klasor.Konum.ToLower().Contains(aramaCubugu.Text) || 
                        klasor.Etiket.ToLower().Contains(aramaCubugu.Text) || 
                        klasor.Tarih.ToLower().Contains(aramaCubugu.Text))
                        klasorTreeMap.Nodes.Add(klasor.Ad);
        }

        private void ResimAra_Click(object sender, EventArgs e)
        {
            if (resimKutusu.Image != null)          //PictureBox doluysa temizler
                resimKutusu.Image.Dispose();
            resimKutusu.Image = null;

            konum.Text = null;
            etiket.Text = null;
            tarih.Text = null;
            fotografListeKutusu.Items.Clear();
            aramaCubugu.Text = aramaCubugu.Text.ToLower();
            if (aramaCubugu.Text != null)   //Arama çubundaki metni veya karakteri tüm resmin adı, konumu, etiketi veya tarihi içeriyorsa listBoxa ekler
                foreach (Klasor klasor in Klasorler)
                    foreach (Resim resim in klasor.Fotograflar)
                        if (resim.Ad.ToLower().Contains(aramaCubugu.Text) || 
                            resim.Konum.ToLower().Contains(aramaCubugu.Text) || 
                            resim.Etiket.ToLower().Contains(aramaCubugu.Text) ||
                            resim.Tarih.ToLower().Contains(aramaCubugu.Text))
                            fotografListeKutusu.Items.Add(resim.Ad);
        }

        private void ResmiAlbumeKaydet_Click(object sender, EventArgs e)
        {
            if (resimKutusu.Image != null && fotografListeKutusu.SelectedIndex >= 0)    //PictureBoxı ve listBoxı kontrol eder
            {
                string tempString = fotografListeKutusu.Items[fotografListeKutusu.SelectedIndex].ToString();
                foreach (Klasor klasor in Klasorler)
                    foreach (Resim resim in klasor.Fotograflar)
                        if (resim.Ad == tempString)     //Seçilen resmin bilgilerini bulur farklı adda bilgilerini ve resmi kaydeder
                        {
                            Resim tempResim = new Resim();
                            tempResim.Konum = resim.Konum;
                            tempResim.Tarih = resim.Tarih;
                            tempResim.Etiket = resim.Etiket;
                            tempResim.Uzantisi = resim.Uzantisi;
                            tempResim.BulunduguKlasorAdi = resim.BulunduguKlasorAdi;
                            tempResim.Ad = String.Format("{0}-{1}", resim.Ad, klasor.Fotograflar.Count);
                            tempResim.BulunduguYol = @"C:\BiKasa\temp.jpg";
                            tempResim.TxtBulunduguYol = tempResim.BulunduguYol.Substring(0, tempResim.BulunduguYol.LastIndexOf(".")) + ".txt";
                            resimKutusu.Image.Save(tempResim.BulunduguYol);
                            kayit.DurumDegistir(new Fotograf());
                            tempResim.BulunduguYol = kayit.Kaydet(tempResim, @"C:\BiKasa\");
                            File.Delete(@"C:\BiKasa\temp.jpg");
                            klasor.Fotograflar.Add(tempResim);
                            fotografListeKutusu.Items.Add(tempResim.Ad);
                            break;
                        }

                konum.Text = null;
                tarih.Text = null;
                etiket.Text = "Resim Kaydedildi.";
            }
            else
                MessageBox.Show("Seçili Resim Yok", "Dikkat");
        }

        private void AramaCubugu_MouseClick(object sender, MouseEventArgs e)
        {
            if (aramaCubugu.Text == "Arama Çubuğu")     //Arama çubuğuna ilk kez basıldığında içini temizler
                aramaCubugu.Text = "";
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Programı Resetlemek İstediğinize Emin Misiniz?\nKayıtlı Tüm Dosyalar Silinecektir.", "Factory Reset", buttons);
            if (result == DialogResult.Yes)         //Kullanıcıya emin misin diye sorup BiKasa klasörünü siler
            {
                if (resimKutusu.Image != null)
                    resimKutusu.Image.Dispose();
                resimKutusu.Image = null;
                konum.Text = "";
                tarih.Text = "";
                etiket.Text = "";
                klasorTreeMap.Nodes.Clear();
                fotografListeKutusu.Items.Clear();
                if (Directory.Exists(@"C:\BiKasa"))
                    Directory.Delete(@"C:\BiKasa", true);
            }
        }
    }

    public class Resim
    {
        public string Ad { get; set;}
        public string Uzantisi { get; set; }
        public string BulunduguKlasorAdi { get; set; }
        public string Konum { get; set; }
        public string BulunduguYol { get; set; }
        public string TxtBulunduguYol { get; set; }
        public string Etiket { get; set; }
        public string Tarih { get; set; }

    }

    public class Klasor
    {
        public List<Resim> Fotograflar = new List<Resim>();
        public string Ad { get; set; }
        public string Konum { get; set; }
        public string Etiket { get; set; }
        public string Tarih { get; set; }
        public string TxtBulunduguYol { get; set; }

    }

    public interface IDosyaDurum //State DP Arayüz
    {
        string Kaydet(Resim resim, string yol);
    }
    public class Fotograf : IDosyaDurum //State DP Durum 1 Concrete Nesnesi
    {
        public string Kaydet(Resim resim, string yol) //Gönderilen resmi ve resmin bilgilerini txtye kaydeder
        {
            if (!Directory.Exists(yol))
                Directory.CreateDirectory(yol);

            if (!File.Exists(yol + resim.Ad + resim.Uzantisi))
            {
                Image fotograf = Image.FromFile(resim.BulunduguYol);
                fotograf.Save(yol + resim.Ad + resim.Uzantisi);
                resim.BulunduguYol = yol + resim.Ad + resim.Uzantisi;
                FileStream fs = new FileStream(yol + resim.Ad + ".txt", FileMode.Create, FileAccess.Write, FileShare.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(resim.Konum + "\\" + resim.Tarih + "\\" + resim.Etiket);
                sw.Close();
                fs.Close();
                fotograf.Dispose();
            }
            
            return resim.BulunduguYol;
        }
    }
    public class TxtDosyasi : IDosyaDurum //State DP Durum 2 Concrete Nesnesi
    {
        public string Kaydet(Resim resim, string yol)   //Gönderilen resmin sadece bilgilerini txtye kaydeder
        {
            FileStream fs = new FileStream(yol, FileMode.Create, FileAccess.Write, FileShare.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(resim.Konum + "\\" + resim.Tarih + "\\" + resim.Etiket);
            sw.Close();
            fs.Close();
            
            return yol;
        }
    }
    public class DosyaKayit //State DP Context Nesnesi
    {
        private IDosyaDurum dosyaDurum;

        public DosyaKayit()
        {
            dosyaDurum = new Fotograf();
        }

        public string Kaydet(Resim resim, string yol)
        {
            return dosyaDurum.Kaydet(resim, yol);
        }

        public void DurumDegistir(IDosyaDurum yeniDurum)
        {
            dosyaDurum = yeniDurum;
        }
    }

    public class Dosya
    {
        //Singleton DP
        private static Dosya instance = null;
        public static Dosya Instance
        {
            get
            {
                if (instance == null)
                    instance = new Dosya();

                return instance;
            }
        }
        
        public void Yukle()     //BiKasa klasöründeki resimleri resim bilgilerini ve klasör bilgilerini rame kaydeder
        {
            try
            {
                if (Directory.Exists(@"C:\BiKasa"))
                {
                    List<string> dosyalar = Directory.GetFiles(@"C:\BiKasa").ToList();

                    if (dosyalar != null && dosyalar.Count > 0)     //dosyaları kontrol eder
                    {

                        for (int i = 0; i < dosyalar.Count; i++)    //.txt uzantılı dosyalar çıkartır
                            if (dosyalar[i].ToLower().EndsWith(".txt"))
                            {
                                dosyalar.RemoveAt(i);
                                i--;
                            }
                        Klasor klasor = new Klasor();
                        int j = 0;
                        for (int i = 0; i < dosyalar.Count; i++)
                            if (dosyalar[i].EndsWith(".jpg") ||     //Uzantıları kontrol eder
                                dosyalar[i].EndsWith(".jpeg") ||
                                dosyalar[i].EndsWith(".png") ||
                                dosyalar[i].EndsWith(".gif") ||
                                dosyalar[i].EndsWith(".tif") ||
                                dosyalar[i].EndsWith(".tiff") ||
                                dosyalar[i].EndsWith(".bmp"))
                            {
                                klasor.Ad = Path.GetFileName(dosyalar[i]).Substring(0, Path.GetFileName(dosyalar[i]).IndexOf("]")).Substring(1);

                                Resim tempResim = new Resim();
                                tempResim.BulunduguYol = dosyalar[i];
                                tempResim.Ad = dosyalar[i].Substring(dosyalar[i].LastIndexOf("["));
                                tempResim.Ad = tempResim.Ad.Substring(0, tempResim.Ad.LastIndexOf("."));
                                tempResim.BulunduguKlasorAdi = klasor.Ad;
                                tempResim.Uzantisi = dosyalar[i].Substring(dosyalar[i].IndexOf('.'));

                                FileStream fs = new FileStream(tempResim.BulunduguYol.Substring(0, tempResim.BulunduguYol.LastIndexOf(".")) + ".txt", FileMode.Open, FileAccess.Read, FileShare.Write);
                                StreamReader sr = new StreamReader(fs);
                                string tempString = sr.ReadLine();
                                sr.Close();
                                fs.Close();

                                tempResim.Konum = tempString.Substring(0, tempString.IndexOf("\\"));
                                tempString = tempString.Substring(tempString.IndexOf("\\") + 1);
                                tempResim.Tarih = tempString.Substring(0, tempString.IndexOf("\\"));
                                tempResim.Etiket = tempString.Substring(tempString.IndexOf("\\") + 1);
                                tempResim.TxtBulunduguYol = tempResim.BulunduguYol.Substring(0, tempResim.BulunduguYol.LastIndexOf(".") + 1) + "txt";
                                
                                klasor.Fotograflar.Add(tempResim);

                                j++;
                                //Bir sonraki fotoğrafın olmadığını veya olan fotoğrafın başka bir klasöre ait olup olmadığını kontrol eder ve klasörü kapatır
                                if (dosyalar.Count == i + 1 || Path.GetFileName(dosyalar[i]).Substring(0, Path.GetFileName(dosyalar[i]).IndexOf("]") + 1) != Path.GetFileName(dosyalar[i + 1]).Substring(0, Path.GetFileName(dosyalar[i + 1]).IndexOf("]") + 1))
                                {
                                    klasor.TxtBulunduguYol = tempResim.BulunduguYol.Substring(0, tempResim.BulunduguYol.LastIndexOf("\\") + 1) + klasor.Ad + ".txt";
                                    
                                    FileStream ffs = new FileStream(klasor.TxtBulunduguYol, FileMode.Open, FileAccess.Read, FileShare.Write);
                                    StreamReader fsr = new StreamReader(ffs);
                                    tempString = fsr.ReadLine();
                                    ffs.Close();
                                    fsr.Close();

                                    klasor.Konum = tempString.Substring(0, tempString.IndexOf("\\"));
                                    tempString = tempString.Substring(tempString.IndexOf("\\") + 1);
                                    klasor.Tarih = tempString.Substring(0, tempString.IndexOf("\\"));
                                    klasor.Etiket = tempString.Substring(tempString.IndexOf("\\") + 1);

                                    AnaProgram.Klasorler.Add(klasor);
                                    j = 0;
                                    klasor = new Klasor();
                                }
                            }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Fotoğraflar Programa Alınırken Bir Sorun Oluştu", "Uyarı");
            }
        }
    }
    
}