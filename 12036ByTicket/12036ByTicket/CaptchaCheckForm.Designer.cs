namespace _12036ByTicket
{
    partial class CaptchaCheckForm
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Captcha_Img = new System.Windows.Forms.PictureBox();
            this.bnt_Captcha_Refresh = new System.Windows.Forms.Button();
            this.Login = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Captcha_Img)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Captcha_Img);
            this.groupBox3.Controls.Add(this.bnt_Captcha_Refresh);
            this.groupBox3.Controls.Add(this.Login);
            this.groupBox3.Location = new System.Drawing.Point(26, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(371, 278);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            // 
            // Captcha_Img
            // 
            this.Captcha_Img.Location = new System.Drawing.Point(15, 18);
            this.Captcha_Img.Name = "Captcha_Img";
            this.Captcha_Img.Size = new System.Drawing.Size(293, 190);
            this.Captcha_Img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Captcha_Img.TabIndex = 2;
            this.Captcha_Img.TabStop = false;
            this.Captcha_Img.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Captcha_Img_MouseDown);
            // 
            // bnt_Captcha_Refresh
            // 
            this.bnt_Captcha_Refresh.Location = new System.Drawing.Point(314, 13);
            this.bnt_Captcha_Refresh.Name = "bnt_Captcha_Refresh";
            this.bnt_Captcha_Refresh.Size = new System.Drawing.Size(52, 33);
            this.bnt_Captcha_Refresh.TabIndex = 1;
            this.bnt_Captcha_Refresh.Text = "刷新";
            this.bnt_Captcha_Refresh.UseVisualStyleBackColor = true;
            this.bnt_Captcha_Refresh.Click += new System.EventHandler(this.bnt_Captcha_Refresh_Click_1);
            // 
            // Login
            // 
            this.Login.Location = new System.Drawing.Point(18, 218);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(290, 31);
            this.Login.TabIndex = 0;
            this.Login.Text = "确定";
            this.Login.UseVisualStyleBackColor = true;
            this.Login.Click += new System.EventHandler(this.Login_Click);
            // 
            // CaptchaCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 297);
            this.Controls.Add(this.groupBox3);
            this.Name = "CaptchaCheckForm";
            this.Text = "请输入验证码";
            this.Load += new System.EventHandler(this.CaptchaCheckForm_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Captcha_Img)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox Captcha_Img;
        private System.Windows.Forms.Button bnt_Captcha_Refresh;
        private System.Windows.Forms.Button Login;
    }
}