using _12036ByTicket.Common;
using _12036ByTicket.LogicModel;
using _12036ByTicket.Model;
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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private List<string> _lsTrainCode = new List<string>();
        private bool isAutoBuy = false;
        private List<QueryTicket> tickets = new List<QueryTicket>();
        // private Dictionary<string, string> leftSeat;
        private const string defaultTicket = "----";
        private List<Normal_passengersItem> _lsPassenger;//乘客
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer buyTimer;
         static object lockObj = new object();
        private int j = 0;
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
            //登录后获取用户信息
            this.userinfo_tb.Text = $"当前账户:【{ _12306Service.UserName}】";
            //乘客列表
            var passengerlist = _12306Service.GetPassenger();
            _lsPassenger = passengerlist;
            foreach (var item in passengerlist)
            {
                pass_ck_b.Items.Add(item.passenger_name);
            }
            //席位选项
            foreach (var item in _12306Service.GetSeatType())
            {
                seat_ck_b.Items.Add(item.SeatName);
            }
            //初始化日期
            dtpicker.Value = DateTime.Now.Date;
            //已选车次选项
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
            var list = _12306Service.getQuery(train_date, from_station, to_station);
            if (list != null && list.Count > 0)
            {
                dgv_tickets.AutoGenerateColumns = false;
                dgv_tickets.DataSource = list;
                dgv_tickets.DoubleBuffered(true);
                dgv_tickets.Rows[0].Selected = false;
                tickets = list;
                FormatLogInfo("余票查询成功！");
            }
            else {
                FormatLogInfo("很抱歉，按您的查询条件，当前未找到列车！");
            }
        }
        private void FormatLogInfo(string arginfo)
        {
            if (!string.IsNullOrEmpty(arginfo))
            {
                string time = DateTime.Now.ToString("hh:mm:ss");
              
                Log_txb.AppendText(string.Format("{0}  {1}\r\n", time, arginfo));
            }
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
                if (stations.Any())
                {
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
            if (!CheckValue()) return;
            if (_lsTrainCode.Count == 0)
            {
                MessageBox.Show("请先选择车次！");
                return;
            }
            if (pass_ck_b.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择乘客！");
                return;
            }
            if (seat_ck_b.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择座位！");
                return;
            }
            if (isAutoBuy)
            {
                isAutoBuy = false;
                buyTimer.Stop();
                Ticket_Buy_btn.Text = "抢票";
                FormatLogInfo("暂停抢票");
            }
            else
            {
                //buyTimer = new System.Windows.Forms.Timer();
                //buyTimer.Interval = 8000;
                //buyTimer.Tick += buyTimer_Tick;
                //isAutoBuy = true;
                //Ticket_Buy_btn.Text = "暂停";
                //buyTimer.Start();
                //j = 0;
                //FormatLogInfo("开始抢票");
                 buyTimer_Tick(null, null);
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



        #region Common
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
            return true;
        }
        /// <summary>
        /// 是否有票
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        private bool CheckTicket(QueryTicket ticket)
        {
            var dic = PramHelper.GetProperties(ticket);
            var parmStr = PramHelper.GetParamSrc(dic);
            return parmStr.Contains("有");
        }
        /// <summary>
        /// 获取选择的乘车人
        /// </summary>
        /// <returns></returns>
        private List<Normal_passengersItem> GetPassaPassengers()
        {
            List<Normal_passengersItem> passengers = new List<Normal_passengersItem>();
            List<string> passengerNameList = new List<string>();
            for (int i = 0; i < pass_ck_b.CheckedItems.Count; i++)
            {
                if (pass_ck_b.GetItemChecked(i) && _lsPassenger.Where(q => q.passenger_name == pass_ck_b.Items[i] as string).Any())
                {
                    passengers.Add(_lsPassenger.Where(q => q.passenger_name == pass_ck_b.Items[i] as string).First());
                }
            }
            return passengers;
        }
        /// <summary>
        /// 获取选中的座位
        /// </summary>
        /// <returns></returns>
        private List<SeatTypeDto> GetCheckSeatTypes()
        {
            var list = new List<SeatTypeDto>();
            for (int i = 0; i < seat_ck_b.CheckedItems.Count; i++)
            {
               var seatType =_12306Service.GetSeatType(seat_ck_b.CheckedItems[i] as string as string).FirstOrDefault();
                if (seatType != null) list.Add(seatType);
            }
            return list; 
        }
        private void QueryTickets()
        {
            var train_date = dtpicker.Text;
            //var train_date = "2019-09-18";
            var from_station = tb_stationFrom.Text;
            var to_station = tb_stationTo.Text;
            var list = _12306Service.getQuery(train_date, from_station, to_station);
            if (list != null && list.Count > 0)
            {
                dgv_tickets.AutoGenerateColumns = false;
                dgv_tickets.DataSource = list;
                dgv_tickets.DoubleBuffered(true);
                dgv_tickets.Rows[0].Selected = false;
                tickets = list;
                FormatLogInfo("余票查询成功！");
            }
            else
            {
                FormatLogInfo("很抱歉，按您的查询条件，当前未找到列车！");
            }
        }
        private void buyTimer_Tick(object sender, EventArgs e)
        {
            foreach (string trian in _lsTrainCode)
            {
                j++;
                QueryTicket selectedTrain = tickets.FirstOrDefault(x => x.Station_Train_Code.Equals(trian));
                if (selectedTrain == null || string.IsNullOrEmpty(selectedTrain.SecretStr))
                {
                     QueryTickets();
                    Thread.Sleep(1000);
                    continue;
                }
                string secretStr = selectedTrain.SecretStr;
                string logMsg = "第" + j + "次购票：" + selectedTrain.Station_Train_Code;
                FormatLogInfo(logMsg);
                bool ticket = CheckTicket(selectedTrain);
                var buySeat = string.Empty;//座位号
               var seats= GetCheckSeatTypes();
                buySeat = seats.FirstOrDefault()?.SeatCode;
                if (!ticket || string.IsNullOrEmpty(buySeat))
                {
                    logMsg = selectedTrain.Station_Train_Code + "无票";
                    FormatLogInfo(logMsg);
                      QueryTickets();
                    Thread.Sleep(1000);
                    continue;
                }
                var selectedPassengers = GetPassaPassengers();
                string msg = string.Empty;
                //出发地
                var stationFrom = tb_stationFrom.Text;
                //结束地
                var stationTo = tb_stationTo.Text;
                //行程日期
                var train_date = dtpicker.Text;
                lock (lockObj)
                {
                    if (BuyTicket(secretStr, selectedPassengers, stationFrom, stationTo, train_date, buySeat, out msg, selectedTrain.IsWait))
                    {
                        buyTimer.Stop();
                    }
                    FormatLogInfo(msg);
                }
            }
        }
        /// <summary>
        /// 购票逻辑
        /// </summary>
        /// <param name="secretStr">车次代码</param>
        /// <param name="selectedPassengers">乘客</param>
        /// <param name="stationFrom">起始站</param>
        /// <param name="stationTo">终点站</param>
        /// <param name="train_date">行程日期</param>
        /// <returns></returns>
        private bool BuyTicket(string secretStr, List<Normal_passengersItem> selectedPassengers,
                                   string stationFrom, string stationTo, string train_date, string buySeat, out string msg, string IsWait)
        {
            msg = "购票失败";
            ////测试候补
            //if (IsWait == "1")
            //{
            //    StandbyTicket(secretStr, selectedPassengers, stationFrom, stationTo, train_date, buySeat, out msg);
            //}

            if (_12306Service.Check_User())
            {
                var isSubmintOk = _12306Service.SubmitOrder(secretStr, stationFrom, stationTo, train_date, out msg);
                if (isSubmintOk)
                {
                    var from = _12306Service.GetinitDc(out msg);
                    if (!string.IsNullOrEmpty(from.token))
                    {

                        string passengerTicketStr, oldPassengerStr;
                        var orderInfo = _12306Service.checkOrderInfo(selectedPassengers, buySeat, from.token, out passengerTicketStr, out oldPassengerStr, out msg);
                        if (orderInfo.submitStatus)
                        {
                            var queueInfo = _12306Service.GetQueueCount(train_date, buySeat, from);
                            var ticket = queueInfo.ticket.ToString().Split(',');// ticket[0] 代表 有对应的座位票 ticket[1] 代表 有站票
                            if (Convert.ToInt32(ticket[0]) == 0)//这个地方判断还需要明确
                            {
                                //to do 如果余票为0 放弃排队
                                msg = "暂无余票";
                            }
                            else
                            {
                                var passengerStr = passengerTicketStr.Split('_');
                                var oldpassengerStr = oldPassengerStr.Split('_');
                                var isOk = _12306Service.confirmSingleForQueue(passengerStr[0], oldpassengerStr[0], from,out msg);
                                if (isOk)//这时候 12306 就会有订单了 让你去支付
                                {
                                    //返回车票的信息 
                                    //留着 出票的接口
                                    var order = _12306Service.queryOrderWaitTime(out msg);
                                    if (!string.IsNullOrEmpty(order.orderId))
                                    {
                                        var s = order;//这个是订单的信息
                                        msg = "购票成功,请及时前往12306处理订单";
                                        return true;
                                    }
                                    else
                                    {
                                        var orderWait = _12306Service.taskqueryOrderWaitTime(out msg);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            else
            {
                msg = "状态异常，需重新登录";
            }
            return false;
        }

        /// <summary>
        /// 候补逻辑
        /// </summary>
        /// <param name="secretStr"></param>
        /// <param name="selectedPassengers"></param>
        /// <param name="stationFrom"></param>
        /// <param name="stationTo"></param>
        /// <param name="train_date"></param>
        /// <param name="buySeat"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool StandbyTicket(string secretStr, List<Normal_passengersItem> selectedPassengers,
            string stationFrom, string stationTo, string train_date, string buySeat, out string msg)
        {
            msg = "候补失败";
            if (_12306Service.Check_User())
            {
                //人脸验证
                var ischechFaceOk = _12306Service.chechFace(secretStr, buySeat);
                if (ischechFaceOk)
                {
                    //加入候补列表
                    var from = _12306Service.getSuccessRate(secretStr, buySeat);

                    if (from.Count != 0)
                    {
                        //提交候补订单
                        var isSubmitOk = _12306Service.submitOrderRequest(secretStr, buySeat);
                        if (isSubmitOk)
                        {
                            //获取候补信息
                            var orderInfo = _12306Service.passengerInitApi();
                            if (orderInfo != null)
                            {
                                //提交候补信息
                                var jzdhDate = string.Format("{0}#{1}", orderInfo.jzdhDateE,
                                    orderInfo.jzdhHourE.Replace(":", "#"));
                                var passengerTicketStr = string.Empty;
                                var oldPassengerStr = string.Empty;
                                foreach (var passenger in selectedPassengers)
                                {
                                    string passengerticket = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", buySeat, "0", "1",
                                        passenger.passenger_name, passenger.passenger_id_type_code, passenger.passenger_id_no,
                                        passenger.mobile_no, "N", passenger.allEncStr, "_");
                                    passengerTicketStr = passengerTicketStr + passengerticket + "_";

                                    string oldPassenger = string.Format("{0},{1},{2},{3}", passenger.passenger_name,
                                        passenger.passenger_id_type_code, passenger.passenger_id_no, passenger.passenger_type);
                                    oldPassengerStr = oldPassengerStr + oldPassenger + "_";
                                }

                                var hbTrain = string.Format("{0},{1}#", secretStr, buySeat);
                                var hbInfo = _12306Service.confirmHB(passengerTicketStr, jzdhDate, hbTrain);
                                if (hbInfo.flag)
                                {
                                    var standby = _12306Service.queryQueue();
                                    if (standby.flag)
                                    {
                                        return true;
                                    }
                                }

                            }

                        }




                    }
                }
            }
            else
            {
                msg = "状态异常，需重新登录";
            }
            return false;
        }
        #endregion
    }
}
