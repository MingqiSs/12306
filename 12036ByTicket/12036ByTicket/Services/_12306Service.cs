using _12036ByTicket.Common;
using _12036ByTicket.LogicModel;
using _12036ByTicket.Model;
using _12036ByTicket.Model.Dto;
using Common.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _12036ByTicket.Services
{
    /// <summary>
    /// 12306接口服务
    /// </summary>
   public static class _12306Service
    {
        private static CookieContainer _cookie = null;
        private static List<StationNames> _stationNames = new List<StationNames>();
        public static string UserName = string.Empty;
        /// <summary>
        /// _cookie 初始化
        /// </summary>
        public static void  Ticket_Init()
        {
            if (_cookie == null)
            {
                _cookie = new CookieContainer();
            }
            var response = HttpHelper.Get( string.Format(UrlConfig.left_Ticket_init), _cookie);
            var js = getJs();
            foreach (Cookie cookie in response.Cookies) _cookie.Add(cookie);
            //_cookie.Add(new Cookie("RAIL_EXPIRATION", js.RAIL_EXPIRATION, "", "kyfw.12306.cn"));
            //_cookie.Add(new Cookie("RAIL_DEVICEID",
            //    js.RAIL_DEVICEID,
            //    "", "kyfw.12306.cn"));
            _cookie.Add(new Cookie("RAIL_EXPIRATION", "1570045548523", "", "kyfw.12306.cn"));
            _cookie.Add(new Cookie("RAIL_DEVICEID",
               "M84LAwiMOAXIxRiIzmh6p-ivTJ70vWciy7W2Qd0q4rKe6ReEvpBP3pgAJCTusrK40hL7pj7arLWJqMJHvAVVZfDuaC6wOCIVvyICN9cGuAvis5I0hgoFjTlTCv99OtRmqUuv6DihNRnIWrV164XDfTtp5wl5SinD",
                "", "kyfw.12306.cn"));
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public static string GetCaptcha()
        {
            try
            {
                if (_cookie == null)
                {
                    _cookie = new CookieContainer();
                }               
                var response = HttpHelper.StringGet( string.Format(UrlConfig.captcha, new Random().Next()), _cookie);
                var result = ((dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Split('(')[1].Split(')')[0]))["image"];
                //var fromBase64 = $"data:image/jpg;base64,{result}";
                return result;

            }
            catch (Exception ex)
            {
                Logger.Error($"获取图片验证码失败异常:{ex.ToString()}");
            }
            return null;
        }
        /// <summary>
        /// 云打码
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        public static CerifyCaptchaCodeRP CerifyCaptchaCode(string baseImgStr)
        {
            var resut = new CerifyCaptchaCodeRP();
            try
            {
                var rq = new { imageFile = baseImgStr };
                var response = HttpClientHelper.PostResponse("http://34.97.127.118:8000/verify/base64/", JsonConvert.SerializeObject(rq));
                //{"code":0,"massage":"","data":["1","6"]}
                resut = JsonConvert.DeserializeObject<CerifyCaptchaCodeRP>(response);
            }
            catch (Exception ex)
            {
                Logger.Error($"云打码发生异常:{ex.ToString()}");
            }
            return resut;
        }
        /// <summary>
        /// 获取站点的代码
        /// </summary>
        /// <returns></returns>
        public static List<StationNames> getFavoriteName()
        {
            try
            {
                if (_stationNames != null && _stationNames.Count()>0)
                {
                    return _stationNames;
                }
                var response = System.Web.HttpUtility.UrlDecode(HttpHelper.StringGet( UrlConfig.getFavoriteNname, _cookie)).Split('=');
                var group = response[1].Split('@');
                foreach (var column in group)
                {
                    if (column== "'")
                    {
                        continue;
                    }                       
                    var model = column.Split('|');
                    StationNames names = new StationNames()
                    {

                        nameCode = model[0],
                        name = model[1],
                        code = model[2],
                        pinYin = model[3],
                        pinYinInitials = model[4],
                        id = model[5],
                    };
                    _stationNames.Add(names);
                }
                return _stationNames;
            }
            catch(Exception ex)
            {
                Logger.Error($"getFavoriteName_{ex.Message}_{ex.StackTrace}");
            }
            return null;
        }

        /// <summary>
        /// 查询车次
        /// </summary>
        /// <param name="train_date">乘车日期</param>
        /// <param name="from_station">始发站对应代码</param>
        /// <param name="to_station">终点站对应代码</param>
        /// <returns></returns>
        public static List<QueryTicket> getQuery(string train_date, string from_station, string to_station)
        {
            var tickets = new List<QueryTicket>();
            try
            {
                if (_stationNames.Count == 0 || _stationNames == null)
                {
                    _stationNames = getFavoriteName();
                }
                var from_code = string.Empty;
                var to_code = string.Empty;
                var fromCode = _stationNames.FirstOrDefault(x => x.name == from_station);
                if (fromCode != null)
                {
                    from_code = fromCode.code;
                }
                var toCode = _stationNames.FirstOrDefault(x => x.name == to_station);
                if (toCode != null)
                {
                    to_code = toCode.code;
                }

                var r = HttpHelper.StringGet(string.Format(UrlConfig.query, train_date, from_code, to_code, "A"), _cookie);
                var response = JsonConvert.DeserializeObject<stationData>(r);
                var map = response.data.map;
                if (response.status && response.data.result != null)
                {
                    var results = response.data.result;
                    foreach (var result in results)
                    {
                        QueryTicket ticket = new QueryTicket();
                        string[] item = result.Split('|');
                        ticket.SecretStr = item[0];
                        ticket.Remark = item[1];
                        ticket.Train_No = item[2];
                        ticket.Station_Train_Code = item[3];
                        ticket.From_Station_Name = map[item[6]];
                        ticket.To_Station_Name = map[item[7]];
                        ticket.Start_Time = item[8];
                        ticket.Arrive_Time = item[9];
                        ticket.LastedTime = item[10];
                        ticket.Gr_Num = item[21];
                        ticket.Qt_Num = item[22];
                        ticket.Rw_Num = item[23];
                        ticket.Rz_Num = item[25];
                        ticket.Wz_Num = item[26];
                        ticket.Yw_Num = item[28];
                        ticket.Yz_Num = item[29];
                        ticket.Ze_Num = item[30];
                        ticket.Zy_Num = item[31];
                        ticket.Swz_Num = item[32];
                        ticket.Dw_Num = item[33];
                        tickets.Add(ticket);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"getQuery{ex.Message}_{ex.StackTrace}");
            }
            return tickets;
        }

        /// <summary>
        /// 获取加密指纹 
        /// </summary>
        /// <returns></returns>
        public static Rail getJs()
        {
            try
            {              
                var response = JsonConvert.DeserializeObject<Rail>(HttpHelper.StringGet(UrlConfig.getLogdevice, _cookie));
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error($"getJs{ex.Message}_{ex.StackTrace}");
            }
            return new Rail();
        }
  

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <returns></returns>
        public static bool CheckCaptcha(string randCode)
        {
            if (_cookie == null)
            {
                _cookie = new CookieContainer();
            }
            //
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数
                                                                                 // var url = string.Format(UrlConfig.captcha_Check, randCode, timeStamp);                                        
            var response = HttpHelper.StringGet( string.Format(UrlConfig.captcha_Check, randCode, timeStamp), _cookie);
            ///**/jQuery19108016482864806321_1554298927290({"result_message":"验证码校验失败","result_code":"5"});
          var dyData= ((dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Split('(')[1].Split(')')[0]));
            if (dyData == null)
            {
                MessageBox.Show($"网络异常,请稍后再试!");
                return false;
            } 
            var result_code = dyData["result_code"];
            if ((int)result_code == 4)
            {
                return true;
            }
            else if ((int)result_code == 5)
            {
                MessageBox.Show($"{dyData["result_message"]}");
            }
            return false;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public static bool Login(string userName, string passWord, string randCode,out string msg)
        {
            var appId = "otn";
             msg = "登录失败";
            string postData = string.Format("username={0}&password={1}&answer={2}&appid={3}", userName,
                passWord, randCode, appId);
            string responseContent = string.Empty;
            var response = HttpHelper.StringPost( UrlConfig.login, postData, _cookie);
            var retDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            if (retDic == null)
            {
                MessageBox.Show($"网络异常,请稍后再试!");
                return false;
            }
            if (retDic.ContainsKey("result_code") && retDic["result_code"].Equals("0"))
            {
                postData = "appid=otn";
                Thread.Sleep(100);
                response = HttpHelper.StringPost( UrlConfig.uamtk, postData, _cookie);
                retDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                if (retDic.ContainsKey("result_code") && retDic["result_code"].Equals("0"))
                {
                    string newapptk = retDic["newapptk"];
                    postData = "tk=" + newapptk;
                    Thread.Sleep(100);
                    response = HttpHelper.StringPost( UrlConfig.uamauthclient, postData, _cookie);
                    retDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                    if (retDic.ContainsKey("result_code") && retDic["result_code"].Equals("0"))
                    {
                        UserName = retDic["username"];
                        return true;
                    }
                }
                //登录成功
                return true;
                
            }
            //登录失败
            msg = retDic["result_message"].ToString();
            return false;
        }
        /// <summary>
        /// 获取乘客列表
        /// </summary>
        /// <returns></returns>
        public static List<Normal_passengersItem> GetPassenger()
        {
            var list = new List<Normal_passengersItem>();
            string postData = string.Format("_json_att={0}", "");
            var response = HttpHelper.StringPost( UrlConfig.getPassenger, postData, _cookie);
            //validateMessagesShowId":"_validatorMessage","status":true,"httpstatus":200,"data":{ "isExist":false,"exMsg":"用户未登录","noLogin":"true","normal_passengers":null,"dj_passengers":null},"messages":[],"validateMessages":{}}
          //  if getPassengerDTOsResult.get("data", False) and getPassengerDTOsResult["data"].get("normal_passengers", False):
            var result = JsonConvert.DeserializeObject<PassengerDto>(response);
            if (result.data.isExist&&result.data.normal_passengers!=null)
            {
                list= result.data.normal_passengers;

            }
            return list;
        }
        /// <summary>
        /// 检查用户登录
        /// </summary>
        /// <returns></returns>
        public static bool Check_User()
        {
            var list = new List<Normal_passengersItem>();
            string postData = string.Format("_json_att={0}", "");
            var response = HttpHelper.StringPost( UrlConfig.check_user_url, postData, _cookie);
            dynamic result = JsonConvert.DeserializeObject(response);
            //{"validateMessagesShowId":"_validatorMessage","status":true,"httpstatus":200,"data":{"flag":true},"messages":[],"validateMessages":{}}
            if ((bool)result["status"] ==true)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 下单-预售下单-提交订单
        /// </summary>
        /// <param name="secretStr"></param>
        /// <param name="from_station"></param>
        /// <param name="to_station_name"></param>
        /// <param name="train_date"></param>
        /// <returns></returns>
        public static bool SubmitOrder(string secretStr,string from_station,string to_station_name,string train_date,out string msg)
        {
            var model = new SubmitOrderModel();
            model.purpose_codes = "ADULT";
            model.query_from_station_name = from_station;
            model.query_to_station_name = to_station_name;
            model.secretStr = secretStr;
            model.tour_flag = "dc";
            model.undefined = "";
            model.train_date = train_date;
            model.back_train_date = DateTime.Now.ToString("yyyy-MM-dd");
            var strDictionary = new BaseDictionary()
            {
                {"secretStr",model.secretStr},
                {"train_date",model.train_date },
                {"back_train_date",model.back_train_date },
                {"tour_flag",model.tour_flag },
                {"purpose_codes",model.purpose_codes},
                {"query_from_station_name",model.query_from_station_name },
                {"query_to_station_name",model.query_to_station_name },
                {"undefined",model.undefined },
            };
            var postData = strDictionary.GetParmarStr();
          var r = HttpHelper.StringPost(UrlConfig.submitOrderRequest, postData, _cookie);
            var response =JsonConvert.DeserializeObject<SubmitOrderResponse>(r);
              msg = response.messages.ToString();
            if (response.status=="true"&&response.data=="N")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 下单-预售下单-进入订单生成页
        /// </summary>
        /// <returns></returns>
        public static ticketInfoForPassengerForm GetinitDc()
        {
            var response = HttpHelper.StringGet(UrlConfig.initDc, _cookie).Split('\n');
            var strToken = response[11].Split('=');
            var token = Regex.Replace(Regex.Replace(strToken[1], @"'", ""), @";", "").Trim();
            // orderRequestDTO  ticketInfoForPassengerForm 暂时不用 用到的时候再说
            //var orderDTO = response[1639];
            //var s3 = orderDTO.Split('=');
            //var orderRequestDTO = Regex.Replace(s3[1], @";", "").Trim();
            var ticketInfo = response[1637].Split('=');
            var ticketInfoForPassengerForm = Regex.Replace(ticketInfo[1], @";", "").Trim();
            var from = JsonConvert.DeserializeObject<ticketInfoForPassengerForm>(ticketInfoForPassengerForm);
            from.token = token;
            return from;
        }

        /// <summary>
        /// 下单-预售下单-校验订单信息
        /// </summary>
        /// <param name="passengers"></param>
        /// <param name="buySeat"></param>
        /// <param name="token"></param>
        /// <param name="passengerTicketStr"></param>
        /// <param name="oldPassengerStr"></param>
        /// <returns></returns>
        public static checkOrderInfoResponseData checkOrderInfo(List<Normal_passengersItem> passengers, string buySeat, string token,
              out string passengerTicketStr, out string oldPassengerStr)
        {
            //座位编号,0,票类型,乘客名,证件类型,证件号,手机号码,保存常用联系人(Y或N),allEncStr(这个是获取联系人里面返回的)
            //  passengerTicketStr = "1,0,1,尹瑶,1,4302***********515,13147077217,N,2831edc444ab8ac170ee85fbffb111c494e1ed0062ee2c3097b28666246696d9bfecc63ac71ea346407ea45de99ae59b" + "_";
            //  oldPassengerStr = "尹瑶,1,4302***********515,1" + "_";
            passengerTicketStr = string.Empty;
            oldPassengerStr = string.Empty;
            foreach (var passenger in passengers)
            {
                string passengerticket = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", buySeat, "0", "1",
                    passenger.passenger_name, passenger.passenger_id_type_code, passenger.passenger_id_no,
                    passenger.mobile_no,"N", passenger.allEncStr,"_");
                passengerTicketStr = passengerTicketStr + passengerticket + "_";

                string oldPassenger = string.Format("{0},{1},{2},{3}", passenger.passenger_name,
                    passenger.passenger_id_type_code, passenger.passenger_id_no, passenger.passenger_type);
                oldPassengerStr = oldPassengerStr + oldPassenger + "_";
            }

            var orderInfo = new checkOrderInfoResponseData();
            var strDictionary = new BaseDictionary()
            {
                {"bed_level_order_num","000000000000000000000000000000"},
                {"passengerTicketStr",passengerTicketStr },
                {"oldPassengerStr",oldPassengerStr },
                {"tour_flag","dc" },
                {"randCode","" },
                {"cancel_flag","2" },
                {"_json_att","" },
                {"REPEAT_SUBMIT_TOKEN",token },
            };
            var postData = strDictionary.GetParmarStr();
            var responses = HttpHelper.StringPost(UrlConfig.checkOrderInfo, postData, _cookie);
            var response = JsonConvert.DeserializeObject<checkOrderInfoResponse>(responses);
            if (response.httpstatus == "200" && response.status == "true")
            {
                return orderInfo = response.data;
            }
            return orderInfo;
        }



        /// <summary>
        /// 下单-预售下单-订单排队
        /// </summary>
        /// <param name="train_date">乘车日期【中国标准时间】</param>
        /// <param name="train_no">车次</param>
        /// <param name="stationTrainCode">车次代号</param>
        /// <param name="seatType">坐席代码</param>
        /// <param name="fromStationTelecode">起始车站</param>
        /// <param name="toStationTelecode">到达车站</param>
        /// <param name="leftTicket">字符串</param>
        /// <param name="purpose_codes">列车位置</param>
        /// <param name="train_location">乘客编号代码</param>
        /// <param name="REPEAT_SUBMIT_TOKEN">token</param>
        /// <returns></returns>
        public static getQueueCountResponseData GetQueueCount(string train_date,string seatType, ticketInfoForPassengerForm from)
        {
            train_date = Convert.ToDateTime(train_date).ToUniversalTime().ToString("r");
            var data = from.queryLeftTicketRequestDTO;
            var responseData = new getQueueCountResponseData();
            var strDictionary = new BaseDictionary()
            {
                {"train_date",train_date},
                {"train_no",data.train_no },
                {"stationTrainCode",data.station_train_code },
                {"seatType",seatType },
                {"fromStationTelecode",data.from_station },
                {"toStationTelecode",data.to_station },
                {"leftTicket",from.leftTicketStr },
                {"purpose_codes",from.purpose_codes },
                {"train_location",from.train_location },
                {"REPEAT_SUBMIT_TOKEN",from.token },
            };
            var postData = strDictionary.GetParmarStr();
            var responses =JsonConvert.DeserializeObject<getQueueCountResponse>(HttpHelper.StringPost(UrlConfig.getQueueCount, postData, _cookie));
            if(responses.httpstatus=="200"&&responses.status=="true")
            {
                return responseData=responses.data;
            }
            return responseData;
        }



        public static bool  confirmSingleForQueue(string passengerTicketStr,string oldPassengerStr, ticketInfoForPassengerForm from)
        {
            var isOk = false;
            var strDictionary = new Dictionary<string, object>
            {
                {"passengerTicketStr",passengerTicketStr},
                {"oldPassengerStr",oldPassengerStr },
                {"purpose_codes",from.purpose_codes },
                {"key_check_isChange",from.key_check_isChange },
                {"leftTicketStr",from.leftTicketStr },
                {"train_location",from.train_location },
                {"seatDetailType","" },
                {"roomType","00" },
                {"dwAll","N" },
                {"whatsSelect",1 },
                {"_json_at","" },
                {"randCode","" },
                {"choose_seats","" },
                {"REPEAT_SUBMIT_TOKEN",from.token },
            };
            var postData = BaseDictionary.GetParmarStrs(strDictionary);
            var r = HttpHelper.StringPost(UrlConfig.confirmSingleForQueue, postData, _cookie);
            var responses =JsonConvert.DeserializeObject<confirmSingleForQueueResponse>(r);
            if(responses.status=="true"&&responses.httpstatus=="200")
            {
                isOk = responses.data.submitStatus;
            }
            return isOk;
        }
        /// <summary>
        /// 下单-预售下单-等待出票
        /// </summary>
        /// <returns></returns>
        public static queryOrderWaitTimeResponseData queryOrderWaitTime()
        {
            var random = RandomHelper.GenerateRandomCode(13);
            var strDictionary = new BaseDictionary()
            {
                {"random",random},//13位随机数
                {"tourFlag","dc" },//成人或者学生
                {"_json_att","" },
            };
            var postData = strDictionary.GetParmarStr();
            var responses = HttpHelper.StringPost(UrlConfig.queryOrderWaitTime, postData, _cookie);
            var response =JsonConvert.DeserializeObject<queryOrderWaitTimeResponse>(responses);
            if (response.httpstatus == "200" && response.status == "true")
            {
                if (Convert.ToInt32(response.data.waitTime) > 1000)
                {
                    //go to 如果等待时长超过1000S，放弃排队，去订单中心取消此订单
                }
                return response.data;
            }
            return null;
        }

        /// <summary>
        /// 下单-预售下单-等待出票 重复10次
        /// </summary>
        /// <returns></returns>
        public static queryOrderWaitTimeResponseData taskqueryOrderWaitTime()
        {
            var order = new queryOrderWaitTimeResponseData();
            var isContinue = true;

            for (int i = 0; i < 10; i++)
            {
                while (isContinue)
                {
                    Thread.Sleep(1000);
                    var orderInfo = queryOrderWaitTime();
                    if (!string.IsNullOrEmpty(orderInfo.orderId))
                    {
                        isContinue = false;
                        order = orderInfo;
                        continue;
                    }

                    if (i == 9)
                    {
                        isContinue = false;
                    }
                }
            }

            return order;
        }

        /// <summary>
        /// 获取图片对应坐标
        /// </summary>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public static string GetCoords(int offsets)
        {
            #region MyRegion
            //    if ofset == '1':
            //    offsetsY = 77
            //    offsetsX = 40
            //elif ofset == '2':
            //    offsetsY = 77
            //    offsetsX = 112
            //elif ofset == '3':
            //    offsetsY = 77
            //    offsetsX = 184
            //elif ofset == '4':
            //    offsetsY = 77
            //    offsetsX = 256
            //elif ofset == '5':
            //    offsetsY = 149
            //    offsetsX = 40
            //elif ofset == '6':
            //    offsetsY = 149
            //    offsetsX = 112
            //elif ofset == '7':
            //    offsetsY = 149
            //    offsetsX = 184
            //elif ofset == '8':
            //    offsetsY = 149
            //    offsetsX = 256
            #endregion
            string coords = string.Empty;
            switch (offsets)
            {
                case 1:
                    coords = "40,77";
                    break;
                case 2:
                    coords = "112,77";
                    break;
                case 3:
                    coords = "184,77";
                    break;
                case 4:
                    coords = "256,77";
                    break;
                case 5:
                    coords = "40,149";
                    break;
                case 6:
                    coords = "112,149";
                    break;
                case 7:
                    coords = "184,149";
                    break;
                case 8:
                    coords = "256,149";
                    break;
            }
            return coords;
        }
        /// <summary>
        /// 获取座位
        /// </summary>
        /// <returns></returns>
        public static List<SeatTypeDto> GetSeatType(string seatName="")
        {
            #region MyRegion
            // '一等座': 'M',
            //'特等座': 'P',
            //'二等座': 'O',
            //'商务座': 9,
            //'硬座': 1,
            //'无座': 1,
            //'软座': 2,
            //'软卧': 4,
            //'硬卧': 3,
            #endregion
            var list = new List<SeatTypeDto>();
            list.Add(new SeatTypeDto { SeatName = "特等座", SeatCode = "P" });
            list.Add(new SeatTypeDto { SeatName = "商务座", SeatCode = "9" });
            list.Add(new SeatTypeDto { SeatName = "一等座", SeatCode = "M" });
            list.Add(new SeatTypeDto { SeatName = "二等座", SeatCode = "O" });
            list.Add(new SeatTypeDto { SeatName = "硬座", SeatCode = "1" });
            list.Add(new SeatTypeDto { SeatName = "无座", SeatCode = "1" });
            list.Add(new SeatTypeDto { SeatName = "软座", SeatCode = "2" });
            list.Add(new SeatTypeDto { SeatName = "软卧", SeatCode = "4" });
            list.Add(new SeatTypeDto { SeatName = "硬卧", SeatCode = "3" });

            if(!string.IsNullOrEmpty(seatName))
            {
              list= list.Where(q => q.SeatName == seatName).ToList();
            }
            return list;
        }
    }
  
}
