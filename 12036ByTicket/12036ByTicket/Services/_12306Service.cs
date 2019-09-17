using _12036ByTicket.Common;
using _12036ByTicket.LogicModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
        private const string DefaultAgent =
           "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36";
        private static List<StationNames> _stationNames = new List<StationNames>();
        /// <summary>
        /// _cookie 初始化
        /// </summary>
        public static void  Ticket_Init()
        {
            if (_cookie == null)
            {
                _cookie = new CookieContainer();
            }
            var response = HttpHelper.Get(DefaultAgent, string.Format(UrlConfig.left_Ticket_init), _cookie);
            var js = getJs();
            foreach (Cookie cookie in response.Cookies) _cookie.Add(cookie);
            _cookie.Add(new Cookie("RAIL_EXPIRATION", js.RAIL_EXPIRATION, "", "kyfw.12306.cn"));
            _cookie.Add(new Cookie("RAIL_DEVICEID",
                js.RAIL_DEVICEID,
                "", "kyfw.12306.cn"));
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public static Image GetCaptcha()
        {
            try
            {
                if (_cookie == null)
                {
                    _cookie = new CookieContainer();
                }
                getQuery("","","");
                var response = HttpHelper.StringGet(DefaultAgent, string.Format(UrlConfig.captcha, new Random().Next()), _cookie);
                var result = ((dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Split('(')[1].Split(')')[0]))["image"];
                //var fromBase64 = $"data:image/jpg;base64,{result}";
                byte[] data = System.Convert.FromBase64String((string)result);
                MemoryStream ms = new MemoryStream(data);
                return Image.FromStream(ms);

            }
            catch (Exception ex)
            {
                Logger.Error("获取图片验证码失败");
            }
            return null;
        }
        /// <summary>
        /// 获取站点的代码
        /// </summary>
        /// <returns></returns>
        public static List<StationNames> getFavoriteName()
        {
            try
            {              
                var response = System.Web.HttpUtility.UrlDecode(HttpHelper.StringGet(DefaultAgent, UrlConfig.getFavoriteNname, _cookie)).Split('=');
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
        public static List<DataInfo> getQuery(string train_date,string from_station,string to_station)
        {
            try
            {
                
                train_date = "2019-09-16";
                from_station = "长沙南";
                to_station = "深圳北";
                if (_stationNames.Count==0||_stationNames==null)
                {
                    _stationNames = getFavoriteName();
                }
                var from_code = string.Empty;
                var to_code = string.Empty;
                var datas = new List<DataInfo>();
                var fromCode = _stationNames.FirstOrDefault(x => x.name == from_station);
                if(fromCode!=null)
                {
                    from_code = fromCode.code;
                }
                var toCode = _stationNames.FirstOrDefault(x => x.name == to_station);
                if (toCode != null)
                {
                    to_code = toCode.code;
                }

                var response =JsonConvert.DeserializeObject<stationData>(System.Web.HttpUtility.UrlDecode(HttpHelper.StringGet(DefaultAgent, string.Format(UrlConfig.query, train_date, from_code, to_code, "A"), _cookie)));
                if(response.status=="true"&&response.httpstatus=="200")
                {
                    var map = response.data.map;
                    var results = response.data.result;
                    foreach(var result in results)
                    {
                        var lists = result.Split('|');
                        var model = new DataInfo()
                        {
                            sign = lists[0],
                            state = lists[1],
                            trainsNumber = lists[2],
                            currentSchedule = lists[3],
                            departureStation = lists[4],
                            originInput = lists[5],
                            currentArrivalStation = lists[6],
                            currentArrivalStationInput = lists[7],
                            startingTime = lists[8],
                            arrivalTime = lists[9],
                            after = lists[10],
                            isCanBePurchased = lists[11],
                            fastSign = lists[12],
                            currentQueryDate = lists[13],
                            unknown = lists[14],
                            unknown1 = lists[15],
                            unknown2 = lists[16],
                            unknown3 = lists[17],
                            unknown4 = lists[18],
                            unknown5 = lists[19],
                            unknown6 = lists[20],
                            unknown7 = lists[21],
                            unknown8 = lists[22],
                            softSeat = lists[23],
                            unknown9 = lists[24],
                            specialSeat = lists[25],
                            noSeat = lists[26],
                            unknown10 = lists[27],
                            hardSleeper = lists[28],
                            hardSeat = lists[29],
                            secondClass = lists[30],
                            firstClass = lists[31],
                            business = lists[32],
                            stillLie = lists[33],
                            unknown11 = lists[34],
                            unknown12 = lists[35],
                            unknown13 = lists[36],
                            isCanAlternate = lists[37],
                            noWaitingTables = lists[38],
                        };

                        datas.Add(model);
                    }
                    return datas;
                }
                

            }
            catch(Exception ex)
            {
                Logger.Error($"getQuery{ex.Message}_{ex.StackTrace}");
            }
            return null;
        }

        /// <summary>
        /// 获取加密指纹 
        /// </summary>
        /// <returns></returns>
        public static Rail getJs()
        {
            try
            {              
                var response = JsonConvert.DeserializeObject<Rail>(HttpHelper.StringGet(DefaultAgent,UrlConfig.getLogdevice, _cookie));
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
            var response = HttpHelper.StringGet(DefaultAgent, string.Format(UrlConfig.captcha_Check, randCode, timeStamp), _cookie);
            ///**/jQuery19108016482864806321_1554298927290({"result_message":"验证码校验失败","result_code":"5"});
          var dyData= ((dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Split('(')[1].Split(')')[0]));
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
            var response = HttpHelper.Post(DefaultAgent, UrlConfig.login, postData, _cookie);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                foreach (Cookie cookie in response.Cookies)
                {
                    _cookie.Add(cookie);
                }
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    StreamReader responseStreamReader = new StreamReader(responseStream, Encoding.UTF8);
                    responseContent = responseStreamReader.ReadToEnd();
                    responseStreamReader.Close();
                }
            }
            if (string.IsNullOrWhiteSpace(responseContent)) return false;
            // {
            //                "result_message": "登录成功",
            //"uamtk": "0KqvJJKDbWKtKnFkNmIrYwBs7ISoA2ui5e08x6DSl5k51x1x0",
            //"result_code": 0
            // }
            dynamic result = JsonConvert.DeserializeObject(responseContent);
            if (result.result_code == 0)
            {
                return true;
                //登录成功
            }
            //登录失败
            MessageBox.Show($"{result.result_message}");
            return false;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public static string GetUserInfo()
        {
            var appId = "otn";

            string postData = string.Format("appid={0}", appId);
            var response = HttpHelper.StringPost(DefaultAgent, UrlConfig.auth, postData, _cookie);
            //todo:{
                //                "apptk": null,
                //"result_message": "验证通过",
                //"name": "屈兴明",
                //"result_code": 0,
                //"newapptk": "jG_kGMHKgQ_K0WoZWDiYO2henBFPPL0P7sp7XAcgq1q0"
                //}
            dynamic result = JsonConvert.DeserializeObject(response);
            if (result.result_code == 0)
            {
                return result.name;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 获取联系人
        /// </summary>
        /// <returns></returns>
        public static string GetPassenger()
        {
            string postData = string.Format("_json_att={0}", "");
            var response = HttpHelper.StringPost(DefaultAgent, UrlConfig.getPassenger, postData, _cookie);
            //validateMessagesShowId":"_validatorMessage","status":true,"httpstatus":200,"data":{ "isExist":false,"exMsg":"用户未登录","noLogin":"true","normal_passengers":null,"dj_passengers":null},"messages":[],"validateMessages":{}}
          //  if getPassengerDTOsResult.get("data", False) and getPassengerDTOsResult["data"].get("normal_passengers", False):
            dynamic result = JsonConvert.DeserializeObject(response);
            if (result.result_code == 0)
            {
                return result.name;
            }
            else
            {
                return "";
            }
        }
    }
}
