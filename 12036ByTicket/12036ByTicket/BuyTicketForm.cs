using _12036ByTicket.Common;
using _12036ByTicket.LogicModel;
using _12036ByTicket.Model;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _12036ByTicket
{
    public partial class BuyTicketForm : Skin_DevExpress
    {
        public BuyTicketForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public event Action LogoutMethod;
        // private List<string> _lsTrainCode = new List<string>();
        private bool isAutoBuy = false;
        private List<NewQueryTicket> tickets = new List<NewQueryTicket>();
        private const string defaultTicket = "----";
        private List<Normal_passengersItem> _lsPassenger;//乘客
        private System.Windows.Forms.Timer buyTimer;
       // List<string> _stationlist = new List<string>();
        static object lockObj = new object();
        private int j = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
           
            #region 初始化数据
            ////乘客列表
            var passengerlist = _12306Service.GetPassenger();
            _lsPassenger = passengerlist;
            foreach (var item in passengerlist)
            {
                pass_ck_b.Items.Add(item.passenger_name);
            }
            ////席位选项
            foreach (var item in _12306Service.GetSeatType())
            {
                seat_ck_b.Items.Add(item.SeatName);
            }
            ////初始化日期
            dtpicker.text = DateTime.Now.Date.ToString("yyyy-MM-dd");
           // dtpicker.MinimumSize
            #endregion

            #region 初始化点击事件
            ToolStripMenuItem tsMenumItem = new ToolStripMenuItem("删除选中");
            tsMenumItem.Click += ToolStripMenuItem_Click;
            this.cms_train.Items.Add(tsMenumItem);
            tsMenumItem = new ToolStripMenuItem("清空列表");
            tsMenumItem.Click += ToolStripMenuItem_Click;
            this.cms_train.Items.Add(tsMenumItem);
            #endregion

            #region 初始化站点的代码
            ////初始化站点的代码
            var stationNames = _12306Service.getFavoriteName().Select(q => q.name).ToArray();
            tb_stationFrom.Items.AddRange(stationNames);
            tb_stationFrom.AutoCompleteCustomSource.AddRange(stationNames);
            tb_stationFrom.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tb_stationFrom.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ////初始化站点的代码
            tb_stationTo.Items.AddRange(stationNames);
            tb_stationTo.AutoCompleteCustomSource.AddRange(stationNames);
            tb_stationTo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tb_stationTo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            #endregion

            #region 初始化dgv设置
            dgv_tickets.AutoGenerateColumns = false;
            //设置自动换行  
            dgv_tickets.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //设置自动调整高度  
            dgv_tickets.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv_tickets.ReadOnly = true;
            #endregion
            ////日志输出
            FormatLogInfo($"登录成功 {_12306Service.UserName}");

          //  _12306Service.Ticket_Init("");
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {

        }

        private void skinButton2_Click(object sender, EventArgs e)
        {

            if (!CheckValue()) return;
              QueryTickets();
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

        private void Ticket_Buy_btn_Click(object sender, EventArgs e)
        {
            if (!CheckValue()) return;
            if (select_train_lb.Items.Count == 0)
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
                var newline = System.Environment.NewLine;
                var passengers = string.Join(",", GetPassaPassengers().Select(q => q.passenger_name));
                var seats = string.Join(",", GetCheckSeatTypes().Select(q => q.SeatName));
                var trains = string.Join(",", select_train_lb.Items.Cast<string>());
                var dr = MessageBox.Show($@"出发:{tb_stationFrom.Text}{newline}目的:{tb_stationTo.Text}{newline}日期:{dtpicker.text}{newline}乘客:{passengers}{newline}座位:{seats}{newline}车次:{trains}{newline}
                                       ", "请确认订单信息!", MessageBoxButtons.OKCancel,
                  MessageBoxIcon.Question);
                if (dr != DialogResult.OK) return;
                j = 0;
                FormatLogInfo("开始抢票");
               
                //buyTimer_Tick(null, null);
                buyTimer = new System.Windows.Forms.Timer();
                buyTimer.Interval = 1000;
                buyTimer.Tick += buyTimer_Tick;
                isAutoBuy = true;
                Ticket_Buy_btn.Text = "暂停";
                buyTimer.Start();
            }

        }
        private void QueryTickets()
        {
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
                dgv_tickets.DataSource = list;
                //dgv_tickets.DoubleBuffered(true);
                //dgv_tickets.AutoResizeColumns();
                //dgv_tickets.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;              
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
            //第一次执行后修改为8秒
            if (buyTimer.Interval == 1000) buyTimer.Interval = 8000;
            foreach (string trian in select_train_lb.Items)
            {
                j++;
                var selectedTrain = tickets.FirstOrDefault(x => x.Station_Train_Code.Equals(trian));
                if (selectedTrain == null || string.IsNullOrEmpty(selectedTrain.SecretStr))
                {
                    QueryTickets();
                    Thread.Sleep(1000);
                    continue;
                }
                string secretStr = selectedTrain.SecretStr;
                string logMsg = "第" + j + "次购票：" + selectedTrain.Station_Train_Code;
                FormatLogInfo(logMsg);
                var buySeat = string.Empty;//座位号
                var seats = GetCheckSeatTypes();
                buySeat = seats.FirstOrDefault()?.SeatCode;
                if (string.IsNullOrEmpty(buySeat))
                {
                    logMsg = selectedTrain.Station_Train_Code + "无票";
                    FormatLogInfo(logMsg);
                    QueryTickets();
                    Thread.Sleep(1000);
                    continue;
                }
                var selectedPassengers = GetPassaPassengers();
                //出发地
                var stationFrom = tb_stationFrom.Text;
                //结束地
                var stationTo = tb_stationTo.Text;
                //行程日期
                string msg = string.Empty;
                var train_date = dtpicker.Text;
                if (BuyTicket(secretStr, selectedPassengers, stationFrom, stationTo, train_date, buySeat, out msg, selectedTrain.IsWait))
                {
                    FormatLogInfo(msg);
                    if (buyTimer != null)
                    {
                        buyTimer.Stop();
                        Ticket_Buy_btn.Text = "抢票";
                    }
                    BuyTicketSuccess();
                }
                else
                FormatLogInfo(msg);
                //Task task = new Task(StratRunButTicket);
                //task.Start();
            }
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
        /// 是否有票
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        private bool CheckTicket(QueryTicket ticket)
        {
            return true;
            //var dic = PramHelper.GetProperties(ticket);
            //var parmStr = PramHelper.GetParamSrc(dic);
            //return parmStr.Contains("有");
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
                var seatType = _12306Service.GetSeatType(seat_ck_b.CheckedItems[i] as string as string).FirstOrDefault();
                if (seatType != null) list.Add(seatType);
            }
            return list;
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
                            var queueInfo = _12306Service.GetQueueCount(train_date, buySeat, from, out msg);
                            var ticket = queueInfo.ticket.ToString().Split(',');// ticket[0] 代表 有对应的座位票 ticket[1] 代表 有站票
                            if (Convert.ToInt32(ticket[0]) == 0)//这个地方判断还需要明确
                            {
                                //to do 如果余票为0 放弃排队
                                msg = "暂无余票";
                            }
                            else
                            {
                                var passengerStr = passengerTicketStr.Split('_');
                                //var oldpassengerStr = oldPassengerStr.Split('_');
                                var isOk = _12306Service.confirmSingleForQueue(passengerStr[0], oldPassengerStr, from, out msg);
                                if (isOk)//这时候 12306 就会有订单了 让你去支付
                                {
                                    //返回车票的信息 
                                    //留着 出票的接口
                                    var order = _12306Service.queryOrderWaitTime(out msg);
                                    if (!string.IsNullOrEmpty(order.orderId))
                                    {
                                        var s = order;//这个是订单的信息
                                        msg = "购票成功,请及时前往12306处理订单,订单号" + order.orderId;
                                        return true;
                                    }
                                    else
                                    {
                                        Thread.Sleep(1000);
                                        var orderWait = _12306Service.taskqueryOrderWaitTime(out msg);
                                        if (!string.IsNullOrEmpty(orderWait.orderId))
                                        {
                                            var s = order;//这个是订单的信息
                                            msg = "购票成功,请及时前往12306处理订单,订单号" + order.orderId;
                                            return true;
                                        }
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
        /// 数据交换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void switch_bnt_Click(object sender, EventArgs e)
        {
            var stationTo = tb_stationTo.Text;
            var stationFrom = tb_stationFrom.Text;
            tb_stationFrom.Text = stationTo;
            tb_stationTo.Text = stationFrom;
        }

        private void BuyTicketSuccess()
        {
            var order = _12306Service.queryMyOrderNoComplete();
            if (order != null && order.orderDBList != null && order.orderDBList.Count() > 0)
            {
                //{"sequence_no":"EH07529330","order_date":"2019-10-30 22:35:11","ticket_totalnum":"1","array_passser_name_page":["向明其"],"from_station_name_page":["深圳北"],"to_station_name_page":["长沙南"],"start_train_date_page":"2019-11-21 07:15","start_time_page":"07:15","arrive_time_page":"10:35","train_code_page":"G6012","ticket_total_price_page":"603.5","come_go_traveller_order_page":"N"}
                var orderinfo = order.orderDBList.First();
                var newline = System.Environment.NewLine;
                var a = orderinfo.from_station_name_page;
              var dr = MessageBox.Show($@"您已成功下单,{string.Join(",", orderinfo.from_station_name_page)}至{string.Join(",", orderinfo.to_station_name_page)}的车票{newline}乘车日期:{orderinfo.start_train_date_page}{newline}待付款金额:{orderinfo.ticket_total_price_page}￥{newline}是否前往官网付款?"
                                           , "下单成功!", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    BrowserHelper.OpenDefaultBrowserUrl("https://www.12306.cn/index/");
                }
            }
        }
        //private void station_TextUpdate(object sender, EventArgs e)
        //{
           
        //    SkinComboBox comboBox = sender as SkinComboBox;
        //    var text = comboBox.Text;
        //    if (comboBox.Name.Equals("tb_stationFrom"))
        //    {
        //        //清空combobox
        //        this.tb_stationFrom.Items.Clear();
        //        //清空listNew
        //        _stationlist.Clear();
        //        //遍历全部备查数据
        //        var stations = _12306Service.getFavoriteName().Where(x =>
        //                     x.name.Contains(text) || x.pinYin.ToUpper().Contains(text.ToUpper()) ||
        //                     x.pinYinInitials.ToUpper().Contains(text.ToUpper()));
        //        if (stations.Count() > 0)
        //        {
        //            //符合，插入ListNew
        //            _stationlist.AddRange(stations.Select(q => q.name));
        //        }
        //        tb_stationFrom.Items.AddRange(_stationlist.ToArray());
        //        tb_stationFrom.SelectionStart = this.tb_stationFrom.Text.Length;
        //        Cursor = Cursors.Default;
        //        this.tb_stationFrom.DroppedDown = true;
        //    }
        //    else
        //    {
        //        //清空combobox
        //        this.tb_stationTo.Items.Clear();
        //        //清空listNew
        //        _stationlist.Clear();
        //        //遍历全部备查数据
        //        var stations = _12306Service.getFavoriteName().Where(x =>
        //                     x.name.Contains(text) || x.pinYin.ToUpper().Contains(text.ToUpper()) ||
        //                     x.pinYinInitials.ToUpper().Contains(text.ToUpper()));
        //        if (stations.Count() > 0)
        //        {
        //            //符合，插入ListNew
        //            _stationlist.AddRange(stations.Select(q => q.name));
        //        }
        //        tb_stationTo.Items.AddRange(_stationlist.ToArray());
        //        tb_stationTo.SelectionStart = this.tb_stationFrom.Text.Length;
        //        Cursor = Cursors.Default;
        //        this.tb_stationTo.DroppedDown = true;
        //    }
        //}

    }
}
