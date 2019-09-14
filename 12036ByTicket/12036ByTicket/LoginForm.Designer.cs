namespace _12036ByTicket
{
    partial class LoginForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Captcha_Img = new System.Windows.Forms.PictureBox();
            this.bnt_Captcha_Refresh = new System.Windows.Forms.Button();
            this.Login = new System.Windows.Forms.Button();
            this.tb_passWord = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_userName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rb_check2 = new System.Windows.Forms.RadioButton();
            this.rb_check1 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_remember = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Captcha_Img)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.tb_passWord);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_userName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cb_remember);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(29, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(390, 366);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "12306";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Captcha_Img);
            this.groupBox3.Controls.Add(this.bnt_Captcha_Refresh);
            this.groupBox3.Controls.Add(this.Login);
            this.groupBox3.Location = new System.Drawing.Point(64, 88);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(297, 256);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            // 
            // Captcha_Img
            // 
            this.Captcha_Img.Location = new System.Drawing.Point(15, 22);
            this.Captcha_Img.Name = "Captcha_Img";
            this.Captcha_Img.Size = new System.Drawing.Size(218, 175);
            this.Captcha_Img.TabIndex = 2;
            this.Captcha_Img.TabStop = false;
            this.Captcha_Img.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Captcha_Img_MouseDown);
            // 
            // bnt_Captcha_Refresh
            // 
            this.bnt_Captcha_Refresh.Location = new System.Drawing.Point(239, 13);
            this.bnt_Captcha_Refresh.Name = "bnt_Captcha_Refresh";
            this.bnt_Captcha_Refresh.Size = new System.Drawing.Size(52, 33);
            this.bnt_Captcha_Refresh.TabIndex = 1;
            this.bnt_Captcha_Refresh.Text = "刷新";
            this.bnt_Captcha_Refresh.UseVisualStyleBackColor = true;
            this.bnt_Captcha_Refresh.Click += new System.EventHandler(this.bnt_Captcha_Refresh_Click);
            // 
            // Login
            // 
            this.Login.Location = new System.Drawing.Point(22, 203);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(248, 31);
            this.Login.TabIndex = 0;
            this.Login.Text = "登录";
            this.Login.UseVisualStyleBackColor = true;
            this.Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // tb_passWord
            // 
            this.tb_passWord.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_passWord.Font = new System.Drawing.Font("微软雅黑", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_passWord.Location = new System.Drawing.Point(49, 60);
            this.tb_passWord.Margin = new System.Windows.Forms.Padding(2);
            this.tb_passWord.Name = "tb_passWord";
            this.tb_passWord.PasswordChar = '*';
            this.tb_passWord.Size = new System.Drawing.Size(146, 23);
            this.tb_passWord.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(10, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 19);
            this.label2.TabIndex = 7;
            this.label2.Text = "密码";
            // 
            // tb_userName
            // 
            this.tb_userName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_userName.Font = new System.Drawing.Font("微软雅黑", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_userName.Location = new System.Drawing.Point(49, 24);
            this.tb_userName.Margin = new System.Windows.Forms.Padding(2);
            this.tb_userName.Name = "tb_userName";
            this.tb_userName.Size = new System.Drawing.Size(146, 23);
            this.tb_userName.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(10, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "账号";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rb_check2);
            this.groupBox2.Controls.Add(this.rb_check1);
            this.groupBox2.Location = new System.Drawing.Point(214, 10);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(136, 35);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // rb_check2
            // 
            this.rb_check2.AutoSize = true;
            this.rb_check2.Location = new System.Drawing.Point(59, 10);
            this.rb_check2.Margin = new System.Windows.Forms.Padding(2);
            this.rb_check2.Name = "rb_check2";
            this.rb_check2.Size = new System.Drawing.Size(61, 23);
            this.rb_check2.TabIndex = 1;
            this.rb_check2.Text = "线路2";
            this.rb_check2.UseVisualStyleBackColor = true;
            // 
            // rb_check1
            // 
            this.rb_check1.AutoSize = true;
            this.rb_check1.Checked = true;
            this.rb_check1.Location = new System.Drawing.Point(3, 10);
            this.rb_check1.Margin = new System.Windows.Forms.Padding(2);
            this.rb_check1.Name = "rb_check1";
            this.rb_check1.Size = new System.Drawing.Size(61, 23);
            this.rb_check1.TabIndex = 0;
            this.rb_check1.TabStop = true;
            this.rb_check1.Text = "线路1";
            this.rb_check1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "验证码";
            // 
            // cb_remember
            // 
            this.cb_remember.AutoSize = true;
            this.cb_remember.Location = new System.Drawing.Point(214, 54);
            this.cb_remember.Margin = new System.Windows.Forms.Padding(2);
            this.cb_remember.Name = "cb_remember";
            this.cb_remember.Size = new System.Drawing.Size(67, 23);
            this.cb_remember.TabIndex = 1;
            this.cb_remember.Text = "记住我";
            this.cb_remember.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 390);
            this.Controls.Add(this.groupBox1);
            this.Name = "LoginForm";
            this.Text = "登录";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Captcha_Img)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button bnt_Captcha_Refresh;
        private System.Windows.Forms.Button Login;
        private System.Windows.Forms.TextBox tb_passWord;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_userName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rb_check2;
        private System.Windows.Forms.RadioButton rb_check1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cb_remember;
        private System.Windows.Forms.PictureBox Captcha_Img;
    }
}

