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
            this.label1 = new System.Windows.Forms.Label();
            this.tb_userName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_passWord = new System.Windows.Forms.TextBox();
            this.btn_Login = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.err_lb = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(57, 106);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "账号:";
            // 
            // tb_userName
            // 
            this.tb_userName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_userName.Font = new System.Drawing.Font("微软雅黑", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_userName.Location = new System.Drawing.Point(96, 106);
            this.tb_userName.Margin = new System.Windows.Forms.Padding(2);
            this.tb_userName.Name = "tb_userName";
            this.tb_userName.Size = new System.Drawing.Size(190, 23);
            this.tb_userName.TabIndex = 6;
            this.tb_userName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.remove_err_lb_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(57, 178);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 19);
            this.label2.TabIndex = 7;
            this.label2.Text = "密码:";
            // 
            // tb_passWord
            // 
            this.tb_passWord.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_passWord.Font = new System.Drawing.Font("微软雅黑", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_passWord.Location = new System.Drawing.Point(96, 178);
            this.tb_passWord.Margin = new System.Windows.Forms.Padding(2);
            this.tb_passWord.Name = "tb_passWord";
            this.tb_passWord.PasswordChar = '*';
            this.tb_passWord.Size = new System.Drawing.Size(190, 23);
            this.tb_passWord.TabIndex = 8;
            this.tb_userName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.remove_err_lb_MouseDown);
            // 
            // btn_Login
            // 
            this.btn_Login.Location = new System.Drawing.Point(95, 249);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(191, 31);
            this.btn_Login.TabIndex = 9;
            this.btn_Login.Text = "登录";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.err_lb);
            this.groupBox1.Controls.Add(this.webBrowser1);
            this.groupBox1.Controls.Add(this.btn_Login);
            this.groupBox1.Controls.Add(this.tb_passWord);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_userName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(29, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(425, 341);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "12306";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(50, 21);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(343, 80);
            this.webBrowser1.TabIndex = 9;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // err_lb
            // 
            this.err_lb.AutoSize = true;
            this.err_lb.Location = new System.Drawing.Point(96, 207);
            this.err_lb.Name = "err_lb";
            this.err_lb.Size = new System.Drawing.Size(0, 19);
            this.err_lb.TabIndex = 10;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 385);
            this.Controls.Add(this.groupBox1);
            this.Name = "LoginForm";
            this.Text = "登录";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_userName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_passWord;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Label err_lb;
    }
}

