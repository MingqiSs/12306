using _12036ByTicket.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12036ByTicket.LogicModel
{
    /// <summary>
    /// 浏览器指纹
    /// </summary>
    public class Rail
    {
        public string RAIL_EXPIRATION { get; set; }

        public string RAIL_DEVICEID { get; set; }
    }
    public class logdevice
    {
        public string dfp { get; set; }

        public string exp { get; set; }
    }
    /// <summary>
    /// 车站的信息
    /// </summary>
    public class StationNames
    {
        /// <summary>
        /// PS @bjb
        /// </summary>
        public string nameCode { get; set; }

        /// <summary>
        /// PS 北京北
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// PS VAP
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// beijingbei 拼音
        /// </summary>
        public string pinYin { get; set; }

        /// <summary>
        /// 缩写 bjb
        /// </summary>
        public string pinYinInitials { get; set; }

        /// <summary>
        /// 没啥用的 0
        /// </summary>
        public string id { get; set; }
    }

    /// <summary>
    /// 查询出来的信息
    /// </summary>
    public  class stationData
    {
        public Data  data { get; set; }

        public string httpstatus { get; set; }

        public string messages { get; set; }

        public bool status { get; set; }


    }

    public class Data
    {
        public string flag { get; set; }

        public Dictionary<string,string> map { get; set; }

        public string[] result { get; set; }
    }

    public class DataInfo
    {
        /// <summary>
        /// 加密信息 下单时必须要使用到的加密字符串，不论坐席，只分车次（需要取出）
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 状态 // 判断车次是否可以预定，候补和下单都可以根据这个数据判断
        /// </summary>
        public string state { get; set; }

        /// <summary>
        ///  当前车次的代号（需要去除）
        /// </summary>
        public string trainsNumber { get; set; }

        /// <summary>
        /// 当前车次
        /// </summary>
        public string currentSchedule { get; set; }

        /// <summary>
        /// 当前始发站
        /// </summary>
        public string departureStation { get; set; }

        /// <summary>
        /// 输入的始发站
        /// </summary>
        public string originInput { get; set; }

        /// <summary>
        /// 当前到达站 （参考）
        /// </summary>
        public string currentArrivalStation { get; set; }

        /// <summary>
        /// 输入到达站
        /// </summary>
        public string currentArrivalStationInput { get; set; }

        /// <summary>
        /// 始发时间
        /// </summary>
        public string startingTime { get; set; }

        /// <summary>
        /// 到达时间
        /// </summary>
        public string arrivalTime { get; set; }

        /// <summary>
        /// 历时
        /// </summary>
        public string after { get; set; }

        /// <summary>
        /// 始发可购买，此字段值能判断预售的票务数据是否可购买，候补不已次数据为判断依据
        /// </summary>
        public string isCanBePurchased { get; set; }

        /// <summary>
        /// 快速下单需要的字符串，预售的时候需取出
        /// </summary>
        public string fastSign { get; set; }

        /// <summary>
        /// 当前查询日期
        /// </summary>
        public string currentQueryDate { get; set; }

        /// <summary>
        /// 不知道
        /// </summary>
        public string unknown { get; set; }

        public string unknown1 { get; set; }

        public string unknown2 { get; set; }

        public string unknown3 { get; set; }

        public string unknown4 { get; set; }

        public string unknown5 { get; set; }

        public string unknown6 { get; set; }

        public string unknown7 { get; set; }

        public string unknown8 { get; set; }

        /// <summary>
        /// 软座
        /// </summary>
        public string softSeat { get; set; }

        public string unknown9 { get; set; }

        /// <summary>
        /// 特等座
        /// </summary>
        public string specialSeat { get; set; }

        /// <summary>
        /// 无座
        /// </summary>
        public string noSeat { get; set; }

        public string unknown10 { get; set; }

        /// <summary>
        /// 硬卧
        /// </summary>
        public string hardSleeper { get; set; }

        /// <summary>
        /// 硬座
        /// </summary>
        public string hardSeat { get; set; }

        /// <summary>
        /// 二等座
        /// </summary>
        public string secondClass { get; set; }

        /// <summary>
        /// 一等座
        /// </summary>
        public string firstClass { get; set; }

        /// <summary>
        /// 商务座
        /// </summary>
        public string business { get; set; }

        /// <summary>
        /// 动卧
        /// </summary>
        public string stillLie { get; set; }

        public string unknown11 { get; set; }

        public string unknown12 { get; set; }

        public string unknown13 { get; set; }

        /// <summary>
        /// 判断该车次是否可候补（0：不可以，1： 可以）
        /// </summary>
        public string isCanAlternate { get; set; }

        /// <summary>
        ///这里会显示当前车次不可以候补坐席（显示的是坐席对应编码）
        /// </summary>
        public string noWaitingTables { get; set; }
    }

    /// <summary>
    /// [普通下单]提交订单的model
    /// </summary>
    public class SubmitOrderModel
    {
        [Description("提交车次需要的字符串(需要解码。url.encode)")]
        public string secretStr { get; set; }

        [Description("车次发车时间")]
        public string train_date { get; set; }

        [Description("返程时间")]
        public string back_train_date { get; set; }

        [Description("写死dc")]
        public string tour_flag { get; set; }

        [Description("ADULT==成人票")]
        public string purpose_codes { get; set; }

        [Description("出发站")]
        public string query_from_station_name { get; set; }

        [Description("到达站")]
        public string query_to_station_name { get; set; }

        [Description("")]
        public string undefined { get; set; }
    }

    /// <summary>
    /// [普通下单]返回
    /// </summary>
    public class SubmitOrderResponse
    {
        [Description("为N时候代表车次代码可以提交，反之必然有错误信息，需要把错误信息打印出来")]
        public string data { get; set; }

        [Description("表示接口是否请求成功")]
        public string status { get; set; }

        [Description("错误信息")]
        public object messages { get; set; }

        [Description("校验的错误信息")]
        public object validateMessages { get; set; }
    }

    /// <summary>
    /// 预定下单等待出票 返回
    /// </summary>
    public class queryOrderWaitTimeResponse
    {
        public string status { get; set; }

        public string httpstatus { get; set; }

        public queryOrderWaitTimeResponseData data { get; set; }
    }

    public class queryOrderWaitTimeResponseData
    {
        public string queryOrderWaitTimeStatus { get; set; }

        public string count { get; set; }

        [Description("为当前用户还需等待时长 如果等待时长超过1000S，放弃排队，去订单中心取消此订单")]
        public string waitTime { get; set; }

        public string requestId { get; set; }

        public string waitCount { get; set; }

        [Description("当前车票类型")]
        public string tourFlag { get; set; }

        [Description("当前车票订单号 - 以此为判断下单是否成功")]
        public string orderId { get; set; }
    }

    public class checkOrderInfoResponse
    {
        public string status { get; set; }

        public string httpstatus { get; set; }

        public checkOrderInfoResponseData data { get; set; }

    }

    public class checkOrderInfoResponseData
    {
        [Description("安全等待时间")]
        public string ifShowPassCodeTime { get; set; }

        [Description("是否可以提交，为false时候，需要打印messages or validateMessages里面的消息")]
        public bool submitStatus { get; set; }

        [Description("是否需要验证码，N=不需要")]
        public string ifShowPassCode { get; set; }

        public string canChooseBeds { get; set; }

        public string canChooseSeats { get; set; }

        public string choose_Seats { get; set; }

        public string isCanChooseMid { get; set; }

        //public string errMsg { get; set; }

    }

    /// <summary>
    /// 订单生成页
    /// </summary>
    public class ticketInfoForPassengerForm
    {
        [Description("字符串")]
        public string leftTicketStr { get; set; }

        [Description("列车位置")]
        public string purpose_codes { get; set; }

        [Description("乘客编号代码")]
        public string train_location { get; set; }

        public queryLeftTicketRequestDTOs queryLeftTicketRequestDTO { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string token { get; set; }

        [Description("字符串")]
        public string key_check_isChange { get; set; }
    }

    public class queryLeftTicketRequestDTOs
    {
        [Description("车次")]
        public string train_no { get; set; }

        [Description("车次代号")]
        public string station_train_code { get; set; }

        [Description("起始车站")]
        public string from_station { get; set; }

        [Description("到达车站")]
        public string to_station { get; set; }
    }

    /// <summary>
    /// 排队
    /// </summary>
    public class getQueueCountResponse
    {
        public string status { get; set; }

        public string httpstatus { get; set; }

        public getQueueCountResponseData data { get; set; }
    }

    public class getQueueCountResponseData
    {
        [Description("当前订单排队人数")]
        public string count { get; set; }

   
        public string countT { get; set; }

        [Description("代表当前余票数，如果余票数为0，可以放弃提交")]
        public object ticket { get; set; }

        public string op_2 { get; set; }

        public string op_1 { get; set; }
    }

    /// <summary>
    /// 确认订单
    /// </summary>
    public class confirmSingleForQueueResponse
    {
        public string status { get; set; }

        public string httpstatus { get; set; }

        public confirmSingleForQueueData data { get; set; }
    }

    public class confirmSingleForQueueData
    {
        public bool submitStatus { get; set; }
    }

    public class chechFaceResponse
    {
        public string status { get; set; }

        public string httpstatus { get; set; }

        public chechFaceData data { get; set; }
    }

    public class chechFaceData
    {
        public bool login_flag { get; set; }

        public string face_check_code { get; set; }

        public bool face_flag { get; set; }
    }

    public class getSuccessRateResponse
    {
        public string status { get; set; }

        public string httpstatus { get; set; }

        public getSuccessRateData data { get; set; }
    }

    public class getSuccessRateData
    {
        public List<getSuccessResponseRateData> flag { get; set; }
    }

    public class getSuccessResponseRateData
    {
        public int level { get; set; }

        [Description("坐席号")]
        public string seat_type_code { get; set; }

        [Description("")]
        public string train_no { get; set; }

        [Description("候补日期")]
        public string start_train_date { get; set; }

        [Description("当前候补位置")]
        public string info { get; set; }
    }

    public class submitOrderRequestResponse
    {
        public string status { get; set; }

        public string httpstatus { get; set; }

        public submitOrderRequestData data { get; set; }
    }

    public class submitOrderRequestData
    {
        public bool flag { get; set; }
    }

    public class passengerInitApiResponse
    {
        public string status { get; set; }

        public string httpstatus { get; set; }

        public passengerInitApiData data { get; set; }
    }

    public class passengerInitApiData
    {
        public string result_code { get; set; }

        public string checkcode { get; set; }

        public object limitTranStr { get; set; }

        /// <summary>
        /// 最后提交jzdhDate格式为2019-08-31#19#00，这个参数在confirmHB接口中需要用到
        /// </summary>
        [Description("当前候补最晚兑现日期")]
        public string jzdhDateE { get; set; }

        public string jzdhDateS { get; set; }

        [Description("当前候补最晚兑现小时，需要将”:” 替换为 “#”")]
        public string jzdhHourE { get; set; }

        public object limitSeatType { get; set; }

        public string isLimitTran { get; set; }

        public string jzdhHourS { get; set; }

        public object hbTrainList { get; set; }
    }

    public class confirmHBResponse
    {
        public string status { get; set; }

        public string httpstatus { get; set; }

        public confirmHBData data { get; set; }
    }

    public class confirmHBData
    {
        [Description("true表示接口请求成功, 否则需要打印messages or validateMessages信息")]
        public bool flag { get; set; }

        public string trace_id { get; set; }

        public bool isAsync { get; set; }
    }

    public class queryQueueResponse
    {
        public string status { get; set; }

        public string httpstatus { get; set; }

        public queryQueueData data { get; set; }
    }

    public class queryQueueData
    {
        [Description("true表示接口请求成功, 否则需要打印messages or validateMessages信息")]
        public bool flag { get; set; }

        public string status { get; set; }

        public bool isAsync { get; set; }
    }
}
