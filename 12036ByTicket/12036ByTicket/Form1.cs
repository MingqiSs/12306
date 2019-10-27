using _12036ByTicket.Services;
using CCWin;
using CCWin.SkinControl;
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
        public event Action LogoutMethod;
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
           var stations=  _12306Service.getFavoriteName();
            tb_stationTo.Items.AddRange(stations.Select(q=>q.name).ToArray());
            ////初始化站点的代码
            tb_stationTo.Items.AddRange(stations.Select(q => q.name).ToArray());
            ////已选车次选项
            ////日志输出
            FormatLogInfo("登录成功");
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {

        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            if (!CheckValue()) return;
            var train_date = dtpicker.Text;

            var from_station = tb_stationFrom.Text;
            var to_station = tb_stationTo.Text;
            var list = _12306Service.getQuery(train_date, from_station, to_station);
            if (!ckb_Gc.Checked) list = list.Where(q => !q.Train_No.Contains("G")).ToList();
            if (!chb_D.Checked) list = list.Where(q => !q.Train_No.Contains("D")).ToList();
            if (!ckb_K.Checked) list = list.Where(q => !q.Train_No.Contains("K")).ToList();
            if (!ckb_T.Checked) list = list.Where(q => !q.Train_No.Contains("T")).ToList();
            if (!ckb_Z.Checked) list = list.Where(q => !q.Train_No.Contains("Z")).ToList();
            if (list != null && list.Count > 0)
            {
                dgv_tickets.AutoGenerateColumns = false;
                dgv_tickets.DataSource = list;
                //dgv_tickets.DoubleBuffered(true);
                dgv_tickets.Rows[0].Selected = false;
                //tickets = list;
                FormatLogInfo("余票查询成功！");
            }
            else
            {
                FormatLogInfo("很抱歉，按您的查询条件，当前未找到列车！");
            }
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

        private void cb_stationFrom_TextChanged(object sender, EventArgs e)
        {
            //SkinComboBox tb = sender as SkinComboBox;
            //string text = tb.Text.Trim();
            //if (!string.IsNullOrWhiteSpace(text))
            //{
            //    var stations = _12306Service.getFavoriteName().Where(x =>
            //            x.name.Contains(text) || x.pinYin.ToUpper().Contains(text.ToUpper()) ||
            //            x.pinYinInitials.ToUpper().Contains(text.ToUpper()));
            //    if (stations.Any())
            //    {
            //        if (tb.Name.Equals("cb_stationFrom"))
            //        {
            //            cb_stationFrom.Items.Clear();
            //            cb_stationFrom.Items.AddRange(stations.Select(q => q.name).ToArray());
            //        }
            //        else
            //        {
                       
            //        }
            //    }

            //}
        }

        private bool CheckValue()
        {
            DateTime selected = DateTime.Parse(dtpicker.text);
            if (selected < DateTime.Now.Date)
            {
                MessageBox.Show("出发日期不能小于当前日期！");
                return false;
            }
            if (selected > DateTime.Now.Date.AddDays(29))
            {
                MessageBox.Show("预售期为30天，今日可购" + DateTime.Now.Date.AddDays(29).ToShortDateString());
                return false;
            }
            if (string.IsNullOrWhiteSpace(tb_stationFrom.Text))
            {
                MessageBox.Show("出发站不能为空！");
                return false;
            }
            if (string.IsNullOrWhiteSpace(tb_stationTo.Text))
            {
                MessageBox.Show("到达站不能为空！");
                return false;
            }
            return true;
        }
    }
}
