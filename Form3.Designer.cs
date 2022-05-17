
namespace BiKasa
{
    partial class KonumEtiketDuzenle
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.iptal = new System.Windows.Forms.Button();
            this.kaydet = new System.Windows.Forms.Button();
            this.konum = new System.Windows.Forms.TextBox();
            this.etiket1 = new System.Windows.Forms.TextBox();
            this.etiket2 = new System.Windows.Forms.TextBox();
            this.etiket3 = new System.Windows.Forms.TextBox();
            this.etiket4 = new System.Windows.Forms.TextBox();
            this.etiket5 = new System.Windows.Forms.TextBox();
            this.konumBasligi = new System.Windows.Forms.Label();
            this.etiketBasligi = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // iptal
            // 
            this.iptal.Location = new System.Drawing.Point(196, 233);
            this.iptal.Name = "iptal";
            this.iptal.Size = new System.Drawing.Size(75, 23);
            this.iptal.TabIndex = 6;
            this.iptal.Text = "İptal";
            this.iptal.UseVisualStyleBackColor = true;
            this.iptal.Click += new System.EventHandler(this.Iptal_Click);
            // 
            // kaydet
            // 
            this.kaydet.Location = new System.Drawing.Point(64, 233);
            this.kaydet.Name = "kaydet";
            this.kaydet.Size = new System.Drawing.Size(126, 23);
            this.kaydet.TabIndex = 5;
            this.kaydet.Text = "Değişiklikleri Kaydet";
            this.kaydet.UseVisualStyleBackColor = true;
            this.kaydet.Click += new System.EventHandler(this.Kaydet_Click);
            // 
            // konum
            // 
            this.konum.Location = new System.Drawing.Point(12, 28);
            this.konum.Name = "konum";
            this.konum.Size = new System.Drawing.Size(152, 23);
            this.konum.TabIndex = 7;
            // 
            // etiket1
            // 
            this.etiket1.Location = new System.Drawing.Point(12, 78);
            this.etiket1.Name = "etiket1";
            this.etiket1.Size = new System.Drawing.Size(152, 23);
            this.etiket1.TabIndex = 8;
            // 
            // etiket2
            // 
            this.etiket2.Location = new System.Drawing.Point(12, 107);
            this.etiket2.Name = "etiket2";
            this.etiket2.Size = new System.Drawing.Size(152, 23);
            this.etiket2.TabIndex = 9;
            // 
            // etiket3
            // 
            this.etiket3.Location = new System.Drawing.Point(12, 136);
            this.etiket3.Name = "etiket3";
            this.etiket3.Size = new System.Drawing.Size(152, 23);
            this.etiket3.TabIndex = 10;
            // 
            // etiket4
            // 
            this.etiket4.Location = new System.Drawing.Point(12, 165);
            this.etiket4.Name = "etiket4";
            this.etiket4.Size = new System.Drawing.Size(152, 23);
            this.etiket4.TabIndex = 11;
            // 
            // etiket5
            // 
            this.etiket5.Location = new System.Drawing.Point(12, 194);
            this.etiket5.Name = "etiket5";
            this.etiket5.Size = new System.Drawing.Size(152, 23);
            this.etiket5.TabIndex = 12;
            // 
            // konumBasligi
            // 
            this.konumBasligi.AutoSize = true;
            this.konumBasligi.Location = new System.Drawing.Point(12, 10);
            this.konumBasligi.Name = "konumBasligi";
            this.konumBasligi.Size = new System.Drawing.Size(46, 15);
            this.konumBasligi.TabIndex = 13;
            this.konumBasligi.Text = "Konum";
            // 
            // etiketBasligi
            // 
            this.etiketBasligi.AutoSize = true;
            this.etiketBasligi.Location = new System.Drawing.Point(12, 60);
            this.etiketBasligi.Name = "etiketBasligi";
            this.etiketBasligi.Size = new System.Drawing.Size(49, 15);
            this.etiketBasligi.TabIndex = 14;
            this.etiketBasligi.Text = "Etiketler";
            // 
            // KonumEtiketDuzenle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 268);
            this.Controls.Add(this.etiketBasligi);
            this.Controls.Add(this.konumBasligi);
            this.Controls.Add(this.etiket5);
            this.Controls.Add(this.etiket4);
            this.Controls.Add(this.etiket3);
            this.Controls.Add(this.etiket2);
            this.Controls.Add(this.etiket1);
            this.Controls.Add(this.konum);
            this.Controls.Add(this.iptal);
            this.Controls.Add(this.kaydet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "KonumEtiketDuzenle";
            this.Text = "Konumu Veya Etiketi Düzenle";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button iptal;
        private System.Windows.Forms.Button kaydet;
        private System.Windows.Forms.Label konumBasligi;
        private System.Windows.Forms.Label etiketBasligi;
        private System.Windows.Forms.TextBox konum;
        private System.Windows.Forms.TextBox etiket1;
        private System.Windows.Forms.TextBox etiket2;
        private System.Windows.Forms.TextBox etiket3;
        private System.Windows.Forms.TextBox etiket4;
        private System.Windows.Forms.TextBox etiket5;
    }
}