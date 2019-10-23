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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.err_lb = new CCWin.SkinControl.SkinLabel();
            this.tb_userName = new CCWin.SkinControl.SkinTextBox();
            this.tb_passWord = new CCWin.SkinControl.SkinTextBox();
            this.btn_Login = new CCWin.SkinControl.SkinButton();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(300, 35);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(123, 72);
            this.webBrowser1.TabIndex = 9;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // err_lb
            // 
            this.err_lb.AutoSize = true;
            this.err_lb.BackColor = System.Drawing.Color.Transparent;
            this.err_lb.BorderColor = System.Drawing.Color.White;
            this.err_lb.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.err_lb.Location = new System.Drawing.Point(109, 219);
            this.err_lb.Name = "err_lb";
            this.err_lb.Size = new System.Drawing.Size(69, 17);
            this.err_lb.TabIndex = 10;
            this.err_lb.Text = "skinLabel1";
            // 
            // tb_userName
            // 
            this.tb_userName.BackColor = System.Drawing.Color.Transparent;
            this.tb_userName.DownBack = null;
            this.tb_userName.Icon = null;
            this.tb_userName.IconIsButton = false;
            this.tb_userName.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.tb_userName.IsPasswordChat = '\0';
            this.tb_userName.IsSystemPasswordChar = false;
            this.tb_userName.Lines = new string[0];
            this.tb_userName.Location = new System.Drawing.Point(112, 131);
            this.tb_userName.Margin = new System.Windows.Forms.Padding(0);
            this.tb_userName.MaxLength = 32767;
            this.tb_userName.MinimumSize = new System.Drawing.Size(28, 28);
            this.tb_userName.MouseBack = null;
            this.tb_userName.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.tb_userName.Multiline = false;
            this.tb_userName.Name = "tb_userName";
            this.tb_userName.NormlBack = null;
            this.tb_userName.Padding = new System.Windows.Forms.Padding(5);
            this.tb_userName.ReadOnly = false;
            this.tb_userName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tb_userName.Size = new System.Drawing.Size(204, 28);
            // 
            // 
            // 
            this.tb_userName.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_userName.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_userName.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.tb_userName.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.tb_userName.SkinTxt.Name = "BaseText";
            this.tb_userName.SkinTxt.Size = new System.Drawing.Size(194, 18);
            this.tb_userName.SkinTxt.TabIndex = 0;
            this.tb_userName.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tb_userName.SkinTxt.WaterText = "12306账号/手机号码/邮箱";
            this.tb_userName.TabIndex = 12;
            this.tb_userName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tb_userName.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tb_userName.WaterText = "12306账号/手机号码/邮箱";
            this.tb_userName.WordWrap = true;
            // 
            // tb_passWord
            // 
            this.tb_passWord.BackColor = System.Drawing.Color.Transparent;
            this.tb_passWord.DownBack = null;
            this.tb_passWord.Icon = null;
            this.tb_passWord.IconIsButton = false;
            this.tb_passWord.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.tb_passWord.IsPasswordChat = '●';
            this.tb_passWord.IsSystemPasswordChar = true;
            this.tb_passWord.Lines = new string[0];
            this.tb_passWord.Location = new System.Drawing.Point(112, 179);
            this.tb_passWord.Margin = new System.Windows.Forms.Padding(0);
            this.tb_passWord.MaxLength = 32767;
            this.tb_passWord.MinimumSize = new System.Drawing.Size(28, 28);
            this.tb_passWord.MouseBack = null;
            this.tb_passWord.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.tb_passWord.Multiline = false;
            this.tb_passWord.Name = "tb_passWord";
            this.tb_passWord.NormlBack = null;
            this.tb_passWord.Padding = new System.Windows.Forms.Padding(5);
            this.tb_passWord.ReadOnly = false;
            this.tb_passWord.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tb_passWord.Size = new System.Drawing.Size(204, 28);
            // 
            // 
            // 
            this.tb_passWord.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_passWord.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_passWord.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.tb_passWord.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.tb_passWord.SkinTxt.Name = "BaseText";
            this.tb_passWord.SkinTxt.PasswordChar = '●';
            this.tb_passWord.SkinTxt.Size = new System.Drawing.Size(194, 18);
            this.tb_passWord.SkinTxt.TabIndex = 0;
            this.tb_passWord.SkinTxt.UseSystemPasswordChar = true;
            this.tb_passWord.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tb_passWord.SkinTxt.WaterText = "密码";
            this.tb_passWord.TabIndex = 13;
            this.tb_passWord.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tb_passWord.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tb_passWord.WaterText = "密码";
            this.tb_passWord.WordWrap = true;
            // 
            // btn_Login
            // 
            this.btn_Login.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_Login.BackColor = System.Drawing.Color.Transparent;
            this.btn_Login.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_Login.BackRectangle = new System.Drawing.Rectangle(50, 23, 50, 23);
            this.btn_Login.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(21)))), ((int)(((byte)(26)))));
            this.btn_Login.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btn_Login.DownBack = ((System.Drawing.Image)(resources.GetObject("btn_Login.DownBack")));
            this.btn_Login.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.btn_Login.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btn_Login.ForeColor = System.Drawing.Color.White;
            this.btn_Login.Location = new System.Drawing.Point(112, 236);
            this.btn_Login.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Login.MouseBack = ((System.Drawing.Image)(resources.GetObject("btn_Login.MouseBack")));
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.NormlBack = ((System.Drawing.Image)(resources.GetObject("btn_Login.NormlBack")));
            this.btn_Login.Size = new System.Drawing.Size(204, 30);
            this.btn_Login.TabIndex = 14;
            this.btn_Login.Text = "登  录";
            this.btn_Login.UseVisualStyleBackColor = false;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 330);
            this.Controls.Add(this.btn_Login);
            this.Controls.Add(this.tb_passWord);
            this.Controls.Add(this.tb_userName);
            this.Controls.Add(this.err_lb);
            this.Controls.Add(this.webBrowser1);
            this.Name = "LoginForm";
            this.Text = "登录";
            this.TitleOffset = new System.Drawing.Point(0, 0);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.WebBrowser webBrowser1;
        private CCWin.SkinControl.SkinLabel err_lb;
        private CCWin.SkinControl.SkinTextBox tb_userName;
        private CCWin.SkinControl.SkinTextBox tb_passWord;
        private CCWin.SkinControl.SkinButton btn_Login;
    }
}

