using _12036ByTicket.Services;
using CCWin;
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
    public partial class Form1 : Skin_DevExpress
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ////登录后获取用户信息
            //this.userinfo_tb.Text = $"当前账户:【{ _12306Service.UserName}】";
            ////乘客列表
            //var passengerlist = _12306Service.GetPassenger();
            //_lsPassenger = passengerlist;
            //foreach (var item in passengerlist)
            //{
            //    pass_ck_b.Items.Add(item.passenger_name);
            //}
            ////席位选项
            //foreach (var item in _12306Service.GetSeatType())
            //{
            //    seat_ck_b.Items.Add(item.SeatName);
            //}
            //#region 初始化车次点击事件
            //ToolStripMenuItem tsMenumItem = new ToolStripMenuItem("删除选中");
            //tsMenumItem.Click += ToolStripMenuItem_Click;
            //this.cms_train.Items.Add(tsMenumItem);
            //tsMenumItem = new ToolStripMenuItem("清空列表");
            //tsMenumItem.Click += ToolStripMenuItem_Click;
            //this.cms_train.Items.Add(tsMenumItem);
            //#endregion
            ////初始化日期
            dtpicker.text = DateTime.Now.Date.ToString("yyyy-MM-dd");
            ////初始化站点的代码
            //_12306Service.getFavoriteName();
            ////已选车次选项
            ////日志输出
            FormatLogInfo("登录成功");
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {

        }

        private void skinButton2_Click(object sender, EventArgs e)
        {

        }

        private void skinGroupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void FormatLogInfo(string arginfo)
        {
            if (!string.IsNullOrEmpty(arginfo))
            {
                string time = DateTime.Now.ToString("hh:mm:ss");

                Log_txb.Text+= string.Format("{0}  {1}\r\n", time, arginfo);
            }
        }
    }
}
