using _12036ByTicket.Common;
using _12036ByTicket.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _12036ByTicket
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            #region Login_init
            LoadCaptchaImg();
            #endregion
        }

        private void Login_Click(object sender, EventArgs e)
        {
            //日志测试
            // Logger.Info($"test");
            
            //登录
             _12306Service.Login("xiangmingqo","","123");
            //登录逻辑
            MainForm logForm = new MainForm();
            logForm.Show();

        }
        private void LoadCaptchaImg()
        {
            var img = _12306Service.GetCaptcha();
            if (img == null)
            {
                MessageBox.Show("加载验证码失败！");
                return;
            }
            Captcha_Img.AutoSize = true;
            Captcha_Img.BackgroundImage = img;
        }

        private void bnt_Captcha_Refresh_Click(object sender, EventArgs e)
        {
            LoadCaptchaImg();
        }
    }
}
