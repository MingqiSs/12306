using _12036ByTicket.Common;
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
        // private List<string> _lsTrainCode = new List<string>();
        private bool isAutoBuy = false;
        private List<NewQueryTicket> tickets = new List<NewQueryTicket>();
        // private Dictionary<string, string> leftSeat;
        private const string defaultTicket = "----";
        private List<Normal_passengersItem> _lsPassenger;//乘客
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer buyTimer;
        static object lockObj = new object();
        private int j = 0;
        private void Form1_Load(object sender, EventArgs e)
        {         
            ////乘客列表
            //var passengerlist = _12306Service.GetPassenger();
            //_lsPassenger = passengerlist;
            //foreach (var item in passengerlist)
            //{
            //    pass_ck_b.Items.Add(item.passenger_name);
            //}
            ////席位选项
            foreach (var item in _12306Service.GetSeatType())
            {
                seat_ck_b.Items.Add(item.SeatName);
            }
            #region 初始化点击事件
            ToolStripMenuItem tsMenumItem = new ToolStripMenuItem("删除选中");
            tsMenumItem.Click += ToolStripMenuItem_Click;
            this.cms_train.Items.Add(tsMenumItem);
            tsMenumItem = new ToolStripMenuItem("清空列表");
            tsMenumItem.Click += ToolStripMenuItem_Click;
            this.cms_train.Items.Add(tsMenumItem);
            #endregion
            ////初始化日期
            dtpicker.text = DateTime.Now.Date.ToString("yyyy-MM-dd");
            ////初始化站点的代码
           var stations=  _12306Service.getFavoriteName();
            tb_stationFrom.Items.AddRange(stations.Select(q=>q.name).ToArray());
            ////初始化站点的代码
            tb_stationTo.Items.AddRange(stations.Select(q => q.name).ToArray());
            ////已选车次选项
            ////日志输出
            FormatLogInfo($"登录成功 {_12306Service.UserName}");

            _12306Service.Ticket_Init("");
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
            var list = _12306Service.GetQuery(train_date, from_station, to_station);
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
                //dgv_tickets.AutoResizeColumns();
                //dgv_tickets.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                //设置自动换行  
                dgv_tickets.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //设置自动调整高度  
                dgv_tickets.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgv_tickets.ReadOnly = true;
                tickets = list;
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
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cb_all_Click(object sender, EventArgs e)
        {
            if (ckb_all.CheckState == CheckState.Checked)
            {
                ckb_Gc.CheckState = CheckState.Checked;
                chb_D.CheckState = CheckState.Checked;
                ckb_K.CheckState = CheckState.Checked;
                ckb_T.CheckState = CheckState.Checked;
                ckb_Z.CheckState = CheckState.Checked;
                ckb_P.CheckState = CheckState.Checked;
                ckb_L.CheckState = CheckState.Checked;
            }
            else {

                ckb_Gc.CheckState = CheckState.Unchecked;
                chb_D.CheckState = CheckState.Unchecked;
                ckb_K.CheckState = CheckState.Unchecked;
                ckb_T.CheckState = CheckState.Unchecked;
                ckb_Z.CheckState = CheckState.Unchecked;
                ckb_P.CheckState = CheckState.Unchecked;
                ckb_L.CheckState = CheckState.Unchecked;
            }
        }
        /// <summary>
        /// 列表点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_tickets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex!=-1&& tickets.Count() >= e.RowIndex + 1)
            {
                string trainNo = tickets[e.RowIndex].Station_Train_Code;
                if (select_train_lb.Items.Contains(trainNo))
                    select_train_lb.Items.Remove(trainNo);
                else
                    select_train_lb.Items.Add(trainNo);
            }
        }
        /// <summary>
        /// 设定右键菜单勾选项,设置ListView列表显示样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsMenumItem = sender as ToolStripMenuItem;
            if (tsMenumItem.Text == "清空列表")
            {
                 select_train_lb.Items.Clear();
            }
            if (tsMenumItem.Text == "删除选中")
            {
                for (int i = 0; i < select_train_lb.Items.Count; i++)
                {
                    if (select_train_lb.GetSelected(i))
                    {
                        select_train_lb.Items.RemoveAt(i);
                    }
                }

            }
        }

        private void select_train_lb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.cms_train.Show(Control.MousePosition.X, Control.MousePosition.Y);

            }
        }
    }
}
