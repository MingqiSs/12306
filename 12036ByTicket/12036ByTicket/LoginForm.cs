using _12036ByTicket.Common;
using _12036ByTicket.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            _12306Service.Ticket_Init();
      
            #endregion
        }
        private void btn_Login_Click(object sender, EventArgs e)
        {
            var randCode = string.Empty;
            if (string.IsNullOrWhiteSpace(tb_userName.Text))
            {
                MessageBox.Show("请输入用户名！");
                return;
            }
            if (string.IsNullOrWhiteSpace(tb_passWord.Text))
            {
                MessageBox.Show("请输入密码！");
                return;
            }
            btn_Login.Name = "登陆中";
            var captchaImgStr = _12306Service.GetCaptcha();
            var captchaCode = _12306Service.CerifyCaptchaCode(captchaImgStr);
            if (captchaCode.Data.Any())
            {
                //处理云打码逻辑
                foreach (var item in captchaCode.Data)
                {
                   var coords =_12306Service.GetCoords(item);
                    randCode = randCode+ coords + ",";
                }
                var isCheck = _12306Service.CheckCaptcha(randCode);
                if (isCheck)
                {
                    //登录
                    if (_12306Service.Login(tb_userName.Text, tb_passWord.Text, randCode.TrimEnd(',')))
                    {
                        //跳转主页
                        MainForm logForm = new MainForm();
                        logForm.Show();
                        return ;
                    }
                }
            }
            //手动输入验证码逻辑
            CaptchaCheckForm captchaCheckForm = new CaptchaCheckForm();
            DialogResult ddr = captchaCheckForm.ShowDialog();
            if (ddr == DialogResult.OK)
            {
                //登录
                if (_12306Service.Login(tb_userName.Text, tb_passWord.Text, captchaCheckForm.RandCode))
                {
                    //跳转主页
                    MainForm logForm = new MainForm();
                    logForm.Show();
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

    }
}
