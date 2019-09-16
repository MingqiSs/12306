﻿using _12036ByTicket.Common;
using _12036ByTicket.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            LoadCaptchaImg();
      
            #endregion
        }
        private List<Point> _clickPoints = null;
        private const int ClickImgSize = 32;//287, 175
        private void btn_Login_Click(object sender, EventArgs e)
        {
            string answer = "";
            if (_clickPoints.Count > 0)
            {
                answer = _clickPoints.Aggregate(answer,
                     (current, p) => current + (p.X + 10) + ',' + (p.Y - 20) + ',');
            } 
           var  randCode = answer.TrimEnd(',');  //todo:124,114,163,64  
        //    MessageBox.Show(randCode);
            var isCheck = _12306Service.CheckCaptcha(randCode);
            if (isCheck)
            {
                //登录
                if (_12306Service.Login(tb_userName.Text, tb_passWord.Text, randCode))
                {
                    //拿用户信息
                  //  var username = _12306Service.GetUserInfo();
                    //跳转主页
                    MainForm logForm = new MainForm();
                    logForm.Show();
                }
            }
            else {
                LoadCaptchaImg();
            }

        }
        private void LoadCaptchaImg()
        {
            
            var img = _12306Service.GetCaptcha();
            if (img == null)
            {
                MessageBox.Show("加载验证码失败！");
                return;
            }
            _clickPoints = new List<Point>();
            Captcha_Img.AutoSize = true;
            Captcha_Img.BackgroundImage = img;
        }
        /// <summary>
        /// 刷新验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bnt_Captcha_Refresh_Click(object sender, EventArgs e)
        {
            LoadCaptchaImg();
        }
        /// <summary>
        /// 验证码点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Captcha_Img_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.Location.X;
            int y = e.Location.Y;
            if (e.Button != MouseButtons.Left) return;
            if (y <= 30) return;
            Point point =
                _clickPoints.FirstOrDefault(
                    p => p.X <= e.X && e.X <= p.X + ClickImgSize && p.Y <= e.Y && e.Y <= p.Y + ClickImgSize);
            Graphics g = Captcha_Img.CreateGraphics();
            if (!point.IsEmpty)
            {
                //再次点击时取消点击标志（用背景将点击验证码图片覆盖）
                g.DrawImage(Captcha_Img.BackgroundImage,
                    new Rectangle(point.X, point.Y, ClickImgSize, ClickImgSize),
                    new Rectangle(point.X, point.Y, ClickImgSize, ClickImgSize), GraphicsUnit.Pixel);
                _clickPoints.Remove(point);
            }
            else
            {
                Image clickImg =
                    new Bitmap("../../Static/click.png");

                g.DrawImage(clickImg, new Point(x - 10, y - 10));
                _clickPoints.Add(new Point(x - 10, y - 10));
            }

        }

        private void Captcha_Img_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
