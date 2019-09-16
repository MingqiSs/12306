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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void gb_main_Enter(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 窗体初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            var userName = _12306Service.GetUserInfo();
            //登录后获取用户信息
            this.userinfo_tb.Text = $"当前账户:【{userName}】";
            //乘客列表
            _12306Service.GetPassenger();
            String[] arr = new String[] { "乘客1", "乘客2", "乘客2", "乘客2", "乘客2", "乘客2", "乘客2" };
            for (int i = 0; i < arr.Count(); i++)
            {
                pass_ck_b.Items.Add(arr[i]);
            }
            //席位选项
            String[] seatlist = new String[] { "硬卧", "硬座", "二等座", "一等座", "无座", "软卧", "动卧","软座","商务座","特等座" };
            for (int i = 0; i < arr.Count(); i++)
            {
                seat_ck_b.Items.Add(seatlist[i]);
            }
            //已选车次选项
            select_train_lb.Items.Add("k9052");
            select_train_lb.Items.Add("k9053");
            //日志输出
            Log_txb.AppendText(DateTime.Now.ToShortTimeString() + "  查询完毕,本次查询共用时:579毫秒"+ Environment.NewLine);
            Log_txb.AppendText(DateTime.Now.ToShortTimeString() + "  查询完毕,本次查询共用时:579毫秒" + Environment.NewLine);
            Log_txb.AppendText(DateTime.Now.ToShortTimeString() + "  查询完毕,本次查询共用时:579毫秒" + Environment.NewLine);
        }
    }
}
