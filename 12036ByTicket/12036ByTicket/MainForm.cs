using _12036ByTicket.Common;
using _12036ByTicket.LogicModel;
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
        private List<string> _lsTrainCode = new List<string>();
        private bool isAutoBuy = false;
        private List<QueryTicket> tickets=new List<QueryTicket>();
        private Dictionary<string, string> leftSeat;
        private const string defaultTicket = "----";
        private List<Normal_passengersItem> _lsPassenger;//乘客
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
            var userName = _12306Service.UserName;
            //登录后获取用户信息
            //this.userinfo_tb.Text = $"当前账户:【{userName}】";
            //乘客列表
          var passengerlist= _12306Service.GetPassenger();
            _lsPassenger = passengerlist;
            foreach (var item in passengerlist)
            {
                pass_ck_b.Items.Add(item.passenger_name);
            }
            //席位选项
            String[] seatlist = new String[] { "硬卧", "硬座", "二等座", "一等座", "无座", "软卧", "动卧","软座","商务座","特等座" };
            for (int i = 0; i < seatlist.Count(); i++)
            {
                seat_ck_b.Items.Add(seatlist[i]);
            }
            //初始化日期
            dtpicker.Value=DateTime.Now.Date;
            //已选车次选项
          //  select_train_lb.Items.Add("k9052");
          //  select_train_lb.Items.Add("k9053");
            //日志输出
            FormatLogInfo("登录成功");

            
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (!CheckValue()) return;
            var train_date = dtpicker.Text;
            //var train_date = "2019-09-18";
            var from_station = tb_stationFrom.Text;
            var to_station = tb_stationTo.Text;
           var list= _12306Service.getQuery(train_date, from_station, to_station);
            if (list != null && list.Count > 0)
            {
                dgv_tickets.AutoGenerateColumns = false;
                dgv_tickets.DataSource = list;
                dgv_tickets.DoubleBuffered(true);
                dgv_tickets.Rows[0].Selected = false;
                tickets = list;
                FormatLogInfo("余票查询成功！");
            }
        }
        private bool CheckValue()
        {
            DateTime selected = dtpicker.Value.Date;
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
            //_date = dtpicker.Value.ToString("yyyy-MM-dd");
            
            //if (rb_student.Checked == true)
            //{
            //    _type = "0X00";
            //}
            return true;
        }
        private void FormatLogInfo(string arginfo)
        {
            string time = DateTime.Now.ToString("hh:mm:ss");
            Log_txb.AppendText(string.Format("{0}  {1}\n", time, arginfo));
        }

        private void tb_station_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            string text = tb.Text.Trim();
            if (!string.IsNullOrWhiteSpace(text))
            {
                var stations = _12306Service.getFavoriteName().Where(x =>
                        x.name.Contains(text) || x.pinYin.ToUpper().Contains(text.ToUpper()) ||
                        x.pinYinInitials.ToUpper().Contains(text.ToUpper()));
                BindingSource bs = new BindingSource();
                bs.DataSource = stations;
                if (tb.Name.Equals("tb_stationFrom"))
                {
                    lb_from.DataSource = bs;
                    lb_from.DisplayMember = "Name";
                    lb_from.ValueMember = "Code";
                    lb_from.Visible = true;
                }
                else
                {
                    lb_to.DataSource = bs;
                    lb_to.DisplayMember = "Name";
                    lb_to.ValueMember = "Code";
                    lb_to.Visible = true;
                }

            }
        }

        private void lb_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListBox lb = sender as ListBox;

            var station = lb.SelectedItem as StationNames;
            if (lb.Name.Equals("lb_from"))
            {
                tb_stationFrom.Text = station.name;
                lb_from.Visible = false;
            }
            else
            {
                tb_stationTo.Text = station.name;
                lb_to.Visible = false;
            }
        }

        private void Ticket_Buy_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckValue()) return;
                var a= _lsTrainCode ;
                if (_lsTrainCode.Count ==0)
                {
                    MessageBox.Show("请先选择车次！");
                    return;
                }
                //
                var secretStr = tickets.Where(q => q.Station_Train_Code == _lsTrainCode.FirstOrDefault()).Select(q => q.SecretStr).FirstOrDefault();
                if (string.IsNullOrEmpty(secretStr))
                {
                    MessageBox.Show("请先选择车次！");
                    return;
                }
                //出发地
                var stationFrom= tb_stationFrom.Text;
                //结束地方
                var stationTo = tb_stationTo.Text;
                ///行程日期
                var train_date = dtpicker.Text;
                if (_12306Service.Check_User())
                {
                    _12306Service.SubmitOrder(secretStr, stationFrom, stationTo, train_date);
                }
                //if (isAutoBuy)
                //{
                //    isAutoBuy = false;
                //    //buyTimer.Stop();
                //    //btn_autoBuy.Text = "抢票";
                //    //FormatLogInfo("暂停抢票");
                //}
                //else
                //{
                //    //buyTimer = new System.Windows.Forms.Timer();
                //    //buyTimer.Interval = 3000;
                //    //buyTimer.Tick += buyTimer_Tick;
                //    //isAutoBuy = true;
                //    //btn_autoBuy.Text = "暂停";
                //    //buyTimer.Start();
                //    //j = 0;
                //    FormatLogInfo("开始抢票");
                //}
            }
            catch (Exception ex)
            {
                Logger.Error($"购票失败！{ex}");
                FormatLogInfo($"购票失败！{ex}");
            }
            
        }

        private void dgv_tickets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            _lsTrainCode = new List<string>();
            select_train_lb.Items.Clear();
            var rows = dgv_tickets.SelectedRows;
            foreach (DataGridViewRow row in rows)
            {
                string trainNo = row.Cells["TrianCode"].Value.ToString();
                _lsTrainCode.Add(trainNo);
                select_train_lb.Items.Add(trainNo);
            }
           
          
        }

        private void dgv_tickets_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    if (dgv_tickets.CurrentRow == null) return;
            //    if (dgv_tickets.SelectedRows.Count > 1 || isAutoBuy)
            //    {
            //        return;
            //    }
            //    string secretStr = dgv_tickets.CurrentRow.Cells["SecretStr"].Value.ToString();
            //    Logger.Info("车次secretStr：" + secretStr);

            //    QueryTicket selectedTrain = tickets.FirstOrDefault(x => x.SecretStr.Equals(secretStr));
            //    if (CheckIsNoTicket(selectedTrain))
            //    {
            //        MessageBox.Show("此车次无票！");
            //        return;
            //    }
            //    //已选车次选项
            //    if (!select_train_lb.Items.Contains(selectedTrain.Station_Train_Code))
            //    {
            //        select_train_lb.Items.Add(selectedTrain.Station_Train_Code);
            //    }
               
            //}
            //catch (Exception exception)
            //{
            //    MessageBox.Show("系统异常,请重试！");
            //    Logger.Error($"error:{exception}");
            //}

        }
        /// <summary>
        /// 是否有票
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        private bool CheckIsNoTicket(QueryTicket ticket)
        {
            leftSeat = new Dictionary<string, string>();
            bool result = true;
            var t = ticket.GetType();
            //由于我们只在Property设置了Attibute,所以先获取Property
            var properties = t.GetProperties();
            foreach (var property in properties)
            {
                if (!property.IsDefined(typeof(SeatAttribute), false)) continue;
                var propertyValue = property.GetValue(ticket) as string;
                if (propertyValue != null && (!propertyValue.Equals(defaultTicket) && !propertyValue.Equals("无")))
                {
                    result = false;
                    SeatAttribute attribute = (SeatAttribute)property.GetCustomAttributes(typeof(SeatAttribute), true).FirstOrDefault();
                    leftSeat.Add(attribute.Code, attribute.Name);
                }
            }
            return result;
        }
        /// <summary>
        /// 获取选择的乘车人
        /// </summary>
        /// <returns></returns>
        private List<Normal_passengersItem> GetPassaPassengers()
        {
            List<Normal_passengersItem> passengers = new List<Normal_passengersItem>();
            return passengers;
        }
    }
}
