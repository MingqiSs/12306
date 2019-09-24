using _12036ByTicket.Common;
using _12036ByTicket.LogicModel;
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
            _cookie.Add(new Cookie("RAIL_EXPIRATION", "1569666558951", "", "kyfw.12306.cn"));
            _cookie.Add(new Cookie("RAIL_DEVICEID",
               "ZO1a4xzjiZ1eJtmb886bz6Jkkw6pHS7MEl92Q5gasIfpRECaMaAA_9Rf5NPiTCaoIHEwMdf91cjO9PIZVHRQhF0MW9fy9ZzOANOPzgbjitrB4fzZw85dtcnp3ElCRLd5yhaLYeGChvFuGDydLdGbNOTu5I4Tvca9",
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
                var response = HttpClientHelper.PostResponse("http://47.107.170.81:8000/verify/base64/", JsonConvert.SerializeObject(rq));
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
        public static bool Login(string userName, string passWord, string randCode)
        {
            var appId = "otn";

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
                Thread.Sleep(500);
                response = HttpHelper.StringPost( UrlConfig.uamtk, postData, _cookie);
                retDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                if (retDic.ContainsKey("result_code") && retDic["result_code"].Equals("0"))
                {
                    string newapptk = retDic["newapptk"];
                    postData = "tk=" + newapptk;
                    Thread.Sleep(500);
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
            MessageBox.Show(retDic["result_message"]);
            return false;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        //public static string GetUserInfo()
        //{
        //    var appId = "otn";

        //    string postData = string.Format("appid={0}", appId);
        //    var response = HttpHelper.StringPost(DefaultAgent, UrlConfig.uamtkstatic, postData, _cookie);
        //    //todo:{
        //        //                "apptk": null,
        //        //"result_message": "验证通过",
        //        //"name": "屈兴明",
        //        //"result_code": 0,
        //        //"newapptk": "jG_kGMHKgQ_K0WoZWDiYO2henBFPPL0P7sp7XAcgq1q0"
        //        //}
        //    dynamic result = JsonConvert.DeserializeObject(response);
        //    if (result.result_code == 0)
        //    {
        //        return result.name;
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
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
        public static bool SubmitOrder(string secretStr,string from_station,string to_station_name,string train_date)
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
            var response =JsonConvert.DeserializeObject<SubmitOrderResponse>(HttpHelper.StringPost(UrlConfig.submitOrderRequest, postData, _cookie));
            if(response.status=="true"&&response.data=="N")
            {
                return true;
            }
            return false;

        }

        public static string GetinitDc()
        {
            var response = HttpHelper.StringGet(UrlConfig.initDc, _cookie);
            return null;
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

    }
}
