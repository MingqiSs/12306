﻿using _12036ByTicket.Common;
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
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private void btn_Login_Click(object sender, EventArgs e)
        {
            Login_init();

            var randCode = string.Empty;
            if (string.IsNullOrWhiteSpace(tb_userName.Text))
            {
               
                err_lb.Text = "请输入用户名！";
                return;
            }
            if (string.IsNullOrWhiteSpace(tb_passWord.Text))
            {
                err_lb.Text = "请输入密码！";
                return;
            }
            if (tb_passWord.Text.Length < 6)
            {
                err_lb.Text = "密码长度不能少于6位！";
                return;
            }
            var msg = string.Empty;
            btn_Login.Text = "登陆中..";

            btn_Login.Enabled = false;
            Login();
            //var   thread = new Thread(new ThreadStart(Login));
            //thread.IsBackground = true;
            //thread.Start();
            
        }
      

        private void LoginForm_Load(object sender, EventArgs e)
        {
            //初始化窗体
            // string authHeader = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
            webBrowser1.Hide();
            webBrowser1.Navigate("https://www.12306.cn/index/index.html", "", null, "");//打开网页
            Thread.Sleep(2000);

            //默认错误文案颜色
            err_lb.ForeColor = Color.Red;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           
        }
       
        private void Login_init()
        {

            try
            {
                string cookieStr = string.Empty;

                while (string.IsNullOrEmpty(cookieStr))
                {
                    if (webBrowser1.Document == null)
                    {
                        Logger.Info("当前未获取到:webBrowser数据");
                        webBrowser1.Refresh();
                        // webBrowser1.Navigate("https://kyfw.12306.cn/otn/resources/login.html", null, null, @"Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8" + System.Environment.NewLine + "Referer: https://www.12306.cn/index/");//打开网页
                        Thread.Sleep(2000);
                    }
                    if (webBrowser1.Document != null)
                    {
                        if (webBrowser1.Document.Cookie == null)
                        {
                            //webBrowser1.Refresh();
                        }
                        else
                        {
                            cookieStr = webBrowser1.Document.Cookie;
                            Logger.Info($"获取当前cookie:{cookieStr}");
                        }

                    }
                }
                _12306Service.Ticket_Init(cookieStr);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (webBrowser1 != null) webBrowser1.Dispose();
            }
        }
        private void Login()
        {
            var randCode = string.Empty;
            var msg = string.Empty;
            var captchaImgStr = _12306Service.GetCaptcha();
            var captchaCode = _12306Service.CerifyCaptchaCode(captchaImgStr);
            if (captchaCode.Data != null && captchaCode.Data.Any())
            {
                //处理云打码逻辑
                foreach (var item in captchaCode.Data)
                {
                    var coords = _12306Service.GetCoords(item);
                    randCode = randCode + coords + ",";
                }
                randCode = randCode.TrimEnd(',');
                var isCheck = _12306Service.CheckCaptcha(randCode);
                if (isCheck)
                {
                    //登录
                    if (_12306Service.Login(tb_userName.Text, tb_passWord.Text, randCode, out msg))
                    {
                        //跳转主页
                       MainForm logForm = new MainForm();
                        logForm.LogoutMethod += ShowLoginForm;
                        this.Visible = false;
                        logForm.Show();
                        return;
                    }
                }
                else
                {
                    //手动输入验证码逻辑
                    CaptchaCheckForm captchaCheckForm = new CaptchaCheckForm();
                    DialogResult ddr = captchaCheckForm.ShowDialog();
                    if (ddr == DialogResult.OK)
                    {
                        //登录
                        if (_12306Service.Login(tb_userName.Text, tb_passWord.Text, captchaCheckForm.RandCode, out msg))
                        {
                            //跳转主页
                            MainForm logForm = new MainForm();
                            logForm.LogoutMethod += ShowLoginForm;
                            this.Visible = false;
                            logForm.Show();
                            return;
                        }
                    }
                }
            }
            else
            {
                //手动输入验证码逻辑
                CaptchaCheckForm captchaCheckForm = new CaptchaCheckForm();
                DialogResult ddr = captchaCheckForm.ShowDialog();
                if (ddr == DialogResult.OK)
                {
                    //登录
                    if (_12306Service.Login(tb_userName.Text, tb_passWord.Text, captchaCheckForm.RandCode, out msg))
                    {

                        //跳转主页
                        MainForm logForm = new MainForm();
                        logForm.LogoutMethod += ShowLoginForm;
                        this.Visible = false;
                        logForm.Show();
                        return;
                    }
                }
            }
            //登录失败
            err_lb.Text = msg;
            btn_Login.Text = "登录";
            btn_Login.Enabled = true;
        }

        private void remove_err_lb_MouseDown(object sender, MouseEventArgs e)
        {
            err_lb.Text = string.Empty;
        }
        private void ShowLoginForm()
        {
            this.Visible = true;
            btn_Login.Text = "登录";
            btn_Login.Enabled = true;
        }
    }
}
