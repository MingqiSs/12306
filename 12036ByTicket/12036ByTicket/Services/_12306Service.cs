﻿using _12036ByTicket.Common;
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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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
        public static void Ticket_Init(string cookieStr)
        {
            if (_cookie == null)
            {
                _cookie = new CookieContainer();
            }
            if (_cookie.Count == 0)
            {
                var response = HttpHelper.Get(string.Format(UrlConfig.left_Ticket_init), _cookie);
                foreach (Cookie cookie in response.Cookies) _cookie.Add(cookie);

                ///初始化cookie
                //var js = GetJs();
                //_cookie.Add(new Cookie("RAIL_EXPIRATION", js.RAIL_EXPIRATION, "", "kyfw.12306.cn"));
                //_cookie.Add(new Cookie("RAIL_DEVICEID",
                // js.RAIL_DEVICEID, "", "kyfw.12306.cn"));

                string[] cookstr = cookieStr.Split(';');
                foreach (string str in cookstr)
                {
                    string[] cookieNameValue = str.Split('=');
                    if (cookieNameValue[0].Trim() == "RAIL_EXPIRATION" || cookieNameValue[0].Trim() == "RAIL_DEVICEID")
                    {
                        _cookie.Add(new Cookie(cookieNameValue[0].Trim(), cookieNameValue[1].Trim(), "", "kyfw.12306.cn"));
                    }
                }

                //_cookie.Add(new Cookie("RAIL_EXPIRATION", "1571290413014", "", "kyfw.12306.cn"));
                //_cookie.Add(new Cookie("RAIL_DEVICEID",
                //   "P5tZtyDuDXprQSTtbaN5n1ObGAYshM1K5EL_D-1Og_ye2D_xFWHwlQfQK9Pn7JsO0JbqasfqHMiH3BB8mGgXdPMJYTeb_iiwVgcVuZF_pj6vji-hvEKfVgyxw4lOeWsLAd6yxaenbGCJMBnaulieOEIdW8NA-hWk",
                //    "", "kyfw.12306.cn"));
            }

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
                var response = HttpHelper.StringGet(string.Format(UrlConfig.captcha, new Random().Next()), _cookie);
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
        /// 用户授权
        /// </summary>
        /// <returns></returns>
        public static bool GetUamtkstatic()
        {
            var appId = "otn";

            string postData = string.Format("appid={0}", appId);
            var response = HttpHelper.Post(UrlConfig.uamtkstatic, postData, _cookie);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取站点的代码
        /// </summary>
        /// <returns></returns>
        public static List<StationNames> getFavoriteName()
        {
            try
            {
                if (_stationNames != null && _stationNames.Count() > 0)
                {
                    return _stationNames;
                }
                var response = System.Web.HttpUtility.UrlDecode(HttpHelper.StringGet(UrlConfig.getFavoriteNname, _cookie)).Split('=');
                var group = response[1].Split('@');
                foreach (var column in group)
                {
                    if (column == "'")
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
            catch (Exception ex)
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

                var r = HttpHelper.StringGet(string.Format(UrlConfig.query, train_date, from_code, to_code, ""), _cookie);
                var response = JsonConvert.DeserializeObject<stationData>(r);
                var map = response.data.map;
                if (response.status && response.data.result != null)
                {
                    var results = response.data.result;
                    foreach (var result in results)
                    {
                        QueryTicket ticket = new QueryTicket();
                        string[] item = result.Split('|');
                        var isWait = item[37] == "1" ;//是否有候补
                        ticket.SecretStr = item[0];
                        ticket.Remark = item[1];
                        ticket.Train_No = item[2];
                        ticket.Station_Train_Code = item[3];
                        ticket.From_Station_Name = map[item[6]];
                        ticket.To_Station_Name = map[item[7]];
                        ticket.Start_Time = item[8];
                        ticket.Arrive_Time = item[9];
                        ticket.LastedTime = item[10];
                        ticket.Gr_Num = GetTicketSeatName(item[21],isWait) ;
                        ticket.Qt_Num = GetTicketSeatName(item[22], isWait);
                        ticket.Rw_Num = GetTicketSeatName(item[23], isWait);
                        ticket.Rz_Num = GetTicketSeatName(item[25], isWait);
                        ticket.Wz_Num = GetTicketSeatName(item[26], isWait);
                        ticket.Yw_Num = GetTicketSeatName(item[28], isWait);
                        ticket.Yz_Num = GetTicketSeatName(item[29], isWait);
                        ticket.Ze_Num = GetTicketSeatName(item[30], isWait);
                        ticket.Zy_Num = GetTicketSeatName(item[31], isWait);
                        ticket.Swz_Num = GetTicketSeatName(item[32], isWait);
                        ticket.Dw_Num = GetTicketSeatName(item[33], isWait);
                        ticket.IsWait = item[37];
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
        /// 查询车次
        /// </summary>
        /// <param name="train_date">乘车日期</param>
        /// <param name="from_station">始发站对应代码</param>
        /// <param name="to_station">终点站对应代码</param>
        /// <returns></returns>
        public static List<NewQueryTicket> GetQuery(string train_date, string from_station, string to_station)
        {
            var tickets = new List<NewQueryTicket>();
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

                var r = HttpHelper.StringGet(string.Format(UrlConfig.query, train_date, from_code, to_code, ""), _cookie);
                var response = JsonConvert.DeserializeObject<stationData>(r);
                var map = response.data.map;
                if (response.status && response.data.result != null)
                {
                    var results = response.data.result;
                    foreach (var result in results)
                    {
                        NewQueryTicket ticket = new NewQueryTicket();
                        string[] item = result.Split('|');
                        var isWait = item[37] == "1";//是否有候补
                        ticket.SecretStr = item[0];
                        ticket.Remark = item[1];
                        ticket.Train_No = item[2];
                        ticket.Station_Train_Code = item[3];
                        ticket.Station_Name = map[item[6]]+ System.Environment.NewLine + map[item[7]];
                        ticket.S_time = item[8] + System.Environment.NewLine + item[9];
                        ticket.LastedTime = item[10];
                        ticket.Gr_Num = GetTicketSeatName(item[21], isWait);
                        ticket.Qt_Num = GetTicketSeatName(item[22], isWait);
                        ticket.Rw_Num = GetTicketSeatName(item[23], isWait);
                        ticket.Rz_Num = GetTicketSeatName(item[25], isWait);
                        ticket.Wz_Num = GetTicketSeatName(item[26], isWait);
                        ticket.Yw_Num = GetTicketSeatName(item[28], isWait);
                        ticket.Yz_Num = GetTicketSeatName(item[29], isWait);
                        ticket.Ze_Num = GetTicketSeatName(item[30], isWait);
                        ticket.Zy_Num = GetTicketSeatName(item[31], isWait);
                        ticket.Swz_Num = GetTicketSeatName(item[32], isWait);
                        ticket.Dw_Num = GetTicketSeatName(item[33], isWait);
                        ticket.IsWait = item[37];
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
        private static string GetTicketSeatName(string seat,bool isWait)
        {
            return seat == "无" && isWait ? "候补" : seat;
        }
        /// <summary>
        /// 获取加密指纹 
        /// </summary>
        /// <returns></returns>
        public static Rail GetJs()
        {
           var rdata= new Rail();
            try
            {
                var script = HttpHelper.StringGet(UrlConfig.getJS, _cookie);
                var result = Regex.Match(script, @"algID\\x3d(.*?)\\x26").Groups["1"];
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数
                //var data= Get_hash_code_params(out  string hashCode);
                //data["timestamp"] = timeStamp;
                //var src= PramHelper.GetParamSrc(data);
                //var url = $"https://kyfw.12306.cn/otn/HttpZF/logdevice?algID={result.ToString()}&hashCode={hashCode}&{src}";

                var data = Get_hash_code_params(out string hashCode);
                var url = $"https://kyfw.12306.cn/otn/HttpZF/logdevice?algID={result.ToString()}&hashCode={hashCode}&{data}&timestamp={timeStamp}";

                var response = HttpHelper.StringGet(url, _cookie);

                if (response.IndexOf("callbackFunction") >= 0)
                {
                    response= response.Split('(')[1].Split(')')[0].Replace("'",string.Empty);
                    var r = JsonConvert.DeserializeObject<logdevice>(response);
                 
                   rdata.RAIL_EXPIRATION = r.exp;
                  rdata.RAIL_DEVICEID = r.dfp;
                    return rdata;
                }
       
            }
            catch (Exception ex)
            {
                Logger.Error($"getJs{ex.Message}_{ex.StackTrace}");
            }
            return new Rail();
        }
        #region 获取加密指纹处理
        private static Dictionary<object, object> Get_hash_code_params1(out string hashCode)
        {
            var parm = new Dictionary<object, object>();
            #region desc 12306解析出来的数据
            //"[{"key":"adblock","value":"0"}," +
            // "{"key":"browserLanguage","value":"zh - CN"}," +
            //    "{"key":"cookieCode","value":"FGGXbCxK2hrKDYXLQausT6kXhPzixKe_"}," +
            //    "{"key":"cookieEnabled","value":"1"},
            //{ "key":"custID","value":"133"}," +
            //    "{"key":"doNotTrack","value":"unknown"}," +
            //    "{"key":"flashVersion","value":0}," +
            //    "{"key":"javaEnabled","value":"0"}," +
            //    "{"key":"jsFonts","value":"49a9fbfe2beb0490836324ceb234fef4"}," +
            //    "{"key":"localCode","value":3232235853}," +
            //    "{"key":"mimeTypes","value":"52d67b2a5aa5e031084733d5006cc664"}," +
            //    "{"key":"os","value":"Win32"},
            //{ "key":"platform","value":"WEB"}," +
            //    "{"key":"plugins","value":"d22ca0b81584fbea62237b14bd04c866"}," +
            //    "{"key":"scrAvailSize","value":"1040x1920"}," +
            //    "{"key":"srcScreenSize","value":"24xx1080x1920"},
            //{ "key":"storeDb","value":"i1l1o1s1"}," +
            //    "{"key":"timeZone","value":-8}," +
            //    "{"key":"touchSupport","value":"99115dfb07133750ba677d055874de87"}," +
            //    "{"key":"userAgent","value":"Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 67.0.3396.99 Safari / 537.36"}," +
            //    "{"key":"webSmartID","value":"378d1309184d0ecf2116f28a5caa05ba"}]"
            #endregion
            var data = new
            {
                adblock = "0",
                browserLanguage = "zh-CN",
                // cookieCode = "FGH3w5gCR83DsyZAceWHAvBC4kmTuBL4",
                cookieEnabled = "1",
                custID = "133",
                doNotTrack = "unknown",
                flashVersion = "0",
                javaEnabled = "0",
                jsFonts = "7416052cf2ec6f9a40e1b45fb8683f03",
                localCode = "192.168.31.223",
                mimeTypes = "e1c479675e139ccb951667adec9fffa4",
                os = "Win32",
                platform = "WEB",
                plugins = "cbe873f9ff34e65b847ce16819a51b7e",
                scrAvailSize = "1040x1920",
                srcScreenSize = "24xx1080x1920",
                storeDb = "i1l1o1s1",
                timeZone = "-8",
                touchSupport = "99115dfb07133750ba677d055874de87",
                userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36",
                webSmartID = "33c042018566d5c28deab6c606096f50",
            };
            var data_trans = new
            {
                adblock = "FMQw",
                appMinorVersion = "qBVW",
                appcodeName = "qT7b",
                browserLanguage = "q4f3",
                browserName = "-UVA",
                browserVersion = "d435",
                //  cookieCode = "VySQ",
                cookieEnabled = "VPIf",
                cpuClass = "Md7A",
                doNotTrack = "VEek",
                flashVersion = "dzuS",
                hasLiedBrowser = "2xC5",
                hasLiedLanguages = "j5po",
                hasLiedOs = "ci5c",
                hasLiedResolution = "3neK",
                historyList = "kU5z",
                indexedDb = "3sw-",
                javaEnabled = "yD16",
                jsFonts = "EOQP",
                localCode = "lEnu",
                localStorage = "XM7l",
                mimeTypes = "jp76",
                online = "9vyE",
                openDatabase = "V8vl",
                os = "hAqN",
                plugins = "ks0Q",
                scrAvailHeight = "88tV",
                scrAvailSize = "TeRS",
                scrAvailWidth = "E-lJ",
                scrColorDepth = "qmyu",
                scrDeviceXDPI = "3jCe",
                scrHeight = "5Jwy",
                scrWidth = "ssI5",
                sessionStorage = "HVia",
                srcScreenSize = "tOHY",
                storeDb = "Fvje",
                systemLanguage = "e6OK",
                timeZone = "q5aJ",
                touchSupport = "wNLf",
                userAgent = "0aew",
                userLanguage = "hLzX",
                webSmartID = "E3gR",
            };
            var p = PramHelper.GetProperties(data);
            var tp = PramHelper.GetProperties(data_trans);
            var d = string.Empty;
            foreach (var item in p)
            {
                d += $"{item.Key}{item.Value}";
                var key = (tp.Any(q => q.Key.ToString() == item.Key.ToString())) ? tp[item.Key] : item.Key;
                parm[key] = item.Value;
            }
            var d_len = d.Length;
            var dArr = d.ToCharArray();

            for (var e = 0; e < (d_len / 2); e++)
            {
                if (e % 2 == 0)
                {
                    var f = dArr[e].ToString();
                    dArr[e] = dArr[d_len - 1 - e];
                    dArr[d_len - 1 - e] = dArr[e];
                }
            }
            d = string.Join("", dArr);
            d_len = d.Length;
            var c = "";
            for (var e = 0; e < d_len; e++)
            {
                var f = System.Text.Encoding.Unicode.GetBytes(new char[] { dArr[e] })[0];
                c = Encoding.Unicode.GetString(new byte[] { 0 }) + Encoding.Unicode.GetString(new byte[] { (byte)(f + 1) });
            }
            d = Encode_string(c);
            d_len = d.Length;
            var d_f = d_len % 3 == 0 ? d_len / 3 : (d_len / 3) + 1;
            if (d_len < 3)
            {
                var e = d.Substring(0, 1 * d_f);
                var f = d.Substring(1 * d_f, 2 * d_f);
                d = d.Substring(2 * d_f, d_len) + e + f;
            }
            d_len = d.Length;
            d_f = d_len % 3 == 0 ? d_len / 3 : (d_len / 3) + 1;
            if (d_len >= 3)
            {
                d = d.Substring(2 * d_f, d_len - 2 * d_f) + d.Substring(0, d_f) + d.Substring(d_f, d_f);
                // d = d.Substring(0, 1 * d_f) + d.Substring(1 * d_f, 2 * d_f) + d.Substring(2 * d_f, d_len);
            }
            //  d = Encode_data_str(d);
            //d = Encode_data_str(d);
            //d = Encode_data_str(d);
            var data_str = Encode_string(d);
            // hashCode = "A4S453b4O6k_Wttu_c3BlTYKQrxOUDgJy-Xuywi9Tok";
            hashCode = data_str;
            //排序
            var parms = (from objDic in parm orderby objDic.Value ascending select objDic).ToDictionary(pair => pair.Key, pair => pair.Value);

            return parm;
        }
        private static string Get_hash_code_params(out string hashCode)
        {
            var parm = new Dictionary<object, object>();
            var data = new
            {
                adblock = "0",
                browserLanguage = "zh-CN",
                cookieCode = "FGGXbCxK2hrKDYXLQausT6kXhPzixKe_",
                cookieEnabled = "1",
                custID = "133",
                doNotTrack = "unknown",
                flashVersion = "0",
                javaEnabled = "0",
                jsFonts = "49a9fbfe2beb0490836324ceb234fef4",
                localCode = "192.168.1.77",
                mimeTypes = "52d67b2a5aa5e031084733d5006cc664",
                os = "Win32",
                platform = "WEB",
                plugins = "d22ca0b81584fbea62237b14bd04c866",
                scrAvailSize = "1040x1920",
                srcScreenSize = "24xx1080x1920",
                storeDb = "i1l1o1s1",
                timeZone = "-8",
                touchSupport = "99115dfb07133750ba677d055874de87",
                userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36",
                webSmartID = "378d1309184d0ecf2116f28a5caa05ba",
            };
            var p = PramHelper.GetProperties(data);

            #region  调用js获取hashCode
            string path = System.Environment.CurrentDirectory + "/Static/GetJS.js";
            string str2 = File.ReadAllText(path);
            var a = new StringBuilder();
            foreach (var i in p)
            {
                a.Append("{" + $"'key':'{i.Key}',value:'{i.Value}'" + "},");
            }
            var aa = a.ToString().TrimEnd(',');
          //  aa = "{'key':'adblock','value':'0'},{'key':'browserLanguage','value':'zh-CN'},{'key':'cookieCode','value':'FGGXbCxK2hrKDYXLQausT6kXhPzixKe_'},{'key':'cookieEnabled','value':'1'},{'key':'custID','value':'133'},{'key':'doNotTrack','value':'unknown'},{'key':'flashVersion','value':0},{'key':'javaEnabled','value':'0'},{'key':'jsFonts','value':'49a9fbfe2beb0490836324ceb234fef4'},{'key':'mimeTypes','value':'d41d8cd98f00b204e9800998ecf8427e'},{'key':'os','value':'Win32'},{'key':'platform','value':'WAP'},{'key':'plugins','value':'d41d8cd98f00b204e9800998ecf8427e'},{'key':'timeZone','value':-8},{'key':'touchSupport','value':'43b910dd8545535a83b0e957bed55965'},{'key':'userAgent','value':'Mozilla/5.0 (Linux; Android 5.0; SM-G900P Build/LRX21T) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Mobile Safari/537.36'},{'key':'wapSmartID','value':'ba73e2cb3c15f90e0abad2f996f0e9b4'},{'key':'storeDb','value':'i1l1o1s1'},{'key':'srcScreenSize','value':'24xx640x360'},{'key':'scrAvailSize','value':'640x360'},{'key':'localCode','value':3232235853}";
            string fun = string.Format(@"hashAlg([{0}],'','')", aa);
            string result = Logic.ExecuteScript(fun, str2);
            var ccc = result.Split('|');
            var data_str = Encode_string(ccc[1]);
            result = Logic.ExecuteScript(string.Format(@"Ra('{0}')", data_str), str2);
            data_str = Encode_string(result);
            #endregion
            hashCode = data_str;
            return ccc[0];
        }
        private static string Encode_data_str(string d)
        {
            var b = d.Length;
            if (b % 2 == 0)
                return d.Substring((b / 2), b - (b / 2)) + d.Substring(0, (b / 2));
            else
            {
                return d.Substring((b / 2 + 1), b - (b / 2 + 1)) + d.Substring((b / 2 + 1)) + d.Substring(0, (b / 2));
            }
        }
        private static string Encode_string(string str)
        {     
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);
            var result = System.Convert.ToBase64String(hash);
            return result.Replace('+', '-').Replace('/', '_').Replace("=", "");
        }
        #endregion
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
            if (!string.IsNullOrEmpty(response))
            {
                var result = JsonConvert.DeserializeObject<PassengerDto>(response);
                if (result.data.isExist && result.data.normal_passengers != null)
                {
                    list = result.data.normal_passengers;

                }
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
            try
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
                Logger.Info($"下单-预售下单-提交订单_SubmitOrder参数:{postData},时间{DateTime.Now.ToString()}");
                var r = HttpHelper.StringPost(UrlConfig.submitOrderRequest, postData, _cookie);
                Logger.Info($"下单-预售下单-提交订单_SubmitOrder返回时间:{DateTime.Now.ToString()}");
                var response = JsonConvert.DeserializeObject<SubmitOrderResponse>(r);
                msg = response.messages.ToString();
                if (response.status == "true" && response.data == "N")
                {
                    return true;
                }

               
            }
            catch (Exception ex)
            {
                Logger.Error($"下单-预售下单-提交订单:{ex.ToString()}");
                msg = "下单-预售下单-提交订单发生异常";//返回给用户的错误,
            }
            return false;
        }

        /// <summary>
        /// 下单-预售下单-进入订单生成页
        /// </summary>
        /// <returns></returns>
        public static ticketInfoForPassengerForm GetinitDc( out string msg)
        {
            var model = new ticketInfoForPassengerForm();
             msg = string.Empty;
            try
            {
                Logger.Info($"下单-预售下单-进入订单生成页_GetinitDc参数,时间{DateTime.Now.ToString()}");
                var response = HttpHelper.StringGet(UrlConfig.initDc, _cookie).Split('\n');
                Logger.Info($"下单-预售下单-进入订单生成页_GetinitDc参数,返回时间{DateTime.Now.ToString()}");
                var strToken = response[11].Split('=');
                var token = Regex.Replace(Regex.Replace(strToken[1], @"'", ""), @";", "").Trim();
                // orderRequestDTO  ticketInfoForPassengerForm 暂时不用 用到的时候再说
                //var orderDTO = response[1639];
                //var s3 = orderDTO.Split('=');
                //var orderRequestDTO = Regex.Replace(s3[1], @";", "").Trim();
                var ticketInfo = response[1637].Split('=');
                var ticketInfoForPassengerForm = Regex.Replace(ticketInfo[1], @";", "").Trim();
                 model = JsonConvert.DeserializeObject<ticketInfoForPassengerForm>(ticketInfoForPassengerForm);
                model.token = token;
            }
            catch (Exception ex)
            {
                Logger.Error($"下单-预售下单-进入订单生成页发生错误:{ex.ToString()}");
                msg = "下单-预售下单-进入订单生成页,网络异常"; //返回给用户的错误
            }
            return model;
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
              out string passengerTicketStr, out string oldPassengerStr, out string msg)
        {
            msg = string.Empty;
            var orderInfo = new checkOrderInfoResponseData();
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
                passengerTicketStr = passengerTicketStr + passengerticket;

                string oldPassenger = string.Format("{0},{1},{2},{3}", passenger.passenger_name,
                    passenger.passenger_id_type_code, passenger.passenger_id_no, passenger.passenger_type);
                oldPassengerStr = oldPassengerStr + oldPassenger + "_";
            }

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
            try
            {
                Logger.Info($"下单-预售下单-校验订单信息_checkOrderInfo参数{postData},时间{DateTime.Now.ToString()}");
                var responses = HttpHelper.StringPost(UrlConfig.checkOrderInfo, postData, _cookie);
                Logger.Info($"下单-预售下单-校验订单信息_checkOrderInfo返回时间{DateTime.Now.ToString()}");
                var response = JsonConvert.DeserializeObject<checkOrderInfoResponse>(responses);
                if (response.httpstatus == "200" && response.status == "true")
                {
                    var data = JsonConvert.DeserializeObject<checkOrderInfoResponseData>(response.data.ToString());
                    if(!string.IsNullOrEmpty(data.ifShowPassCodeTime))
                    {
                        Logger.Info($"下单-预售下单-校验订单信息_checkOrderInfo安全等待时间{data.ifShowPassCodeTime},时间{DateTime.Now.ToString()}");
                        Thread.Sleep(Convert.ToInt32(data.ifShowPassCodeTime));
                        Logger.Info($"下单-预售下单-校验订单信息_checkOrderInfo安全等待时间结束,时间{DateTime.Now.ToString()}");
                    }
                    return orderInfo = data;
                }
                else
                {
                    throw new Exception(response.data.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"下单-预售下单-进入订单生成页发生错误:{ex.ToString()}");
                msg = "下单-预售下单-校验订单信息,网络异常"; //返回给用户的错误
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
        public static getQueueCountResponseData GetQueueCount(string train_date,string seatType, ticketInfoForPassengerForm from,out string msg)
        {
            //train_date = Convert.ToDateTime(train_date).ToUniversalTime().ToString("r");
            var myDTFI = new System.Globalization.CultureInfo("en-US", false).DateTimeFormat;
      
            train_date = Convert.ToDateTime(train_date).ToString("ddd MMM dd yyyy", myDTFI) + " 00:00:00 GMT+0800(中国标准时间)";
            train_date = HttpUtility.UrlEncode(train_date, Encoding.UTF8);
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
            try
            {
                Logger.Info($"下单-预售下单-订单排队_GetQueueCount参数{postData},时间{DateTime.Now.ToString()}");
                var r = HttpHelper.StringPost(UrlConfig.getQueueCount, postData, _cookie);
                Logger.Info($"下单-预售下单-订单排队_GetQueueCount返回时间{DateTime.Now.ToString()}");
                var responses = JsonConvert.DeserializeObject<getQueueCountResponse>(r);
                if (responses.httpstatus == "200" && responses.status == "true")
                {
                    msg = "下单-预售下单-订单排队中";

                    return responseData =JsonConvert.DeserializeObject<getQueueCountResponseData>(responses.data.ToString());
                }
                else
                {
                    throw new Exception(responses.data.ToString());
                }
                
            }
            catch (Exception ex)
            {
                Logger.Error($"下单-预售下单-进入订单生成页发生错误:{ex.ToString()}");
                msg = "下单-预售下单-订单排队"+ex.Message; //返回给用户的错误
            }
            msg = "下单-预售下单-订单排队错误";
            return responseData;
        }

        /// <summary>
        /// 确认是否下单成功
        /// </summary>
        /// <param name="passengerTicketStr"></param>
        /// <param name="oldPassengerStr"></param>
        /// <param name="from"></param>
        /// <returns></returns>

        public static bool  confirmSingleForQueue(string passengerTicketStr,string oldPassengerStr, ticketInfoForPassengerForm from,out string msg)
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
            var msgs = string.Empty;
            try
            {
                Logger.Info($"确认是否下单成功_confirmSingleForQueue参数{postData},时间{DateTime.Now.ToString()}");
                var r = HttpHelper.StringPost(UrlConfig.confirmSingleForQueue, postData, _cookie);
                Logger.Info($"确认是否下单成功_confirmSingleForQueue返回时间{DateTime.Now.ToString()}");
               var responses = JsonConvert.DeserializeObject<confirmSingleForQueueResponse>(r);
               
                if (responses.status == "true" && responses.httpstatus == "200")
                {
                    var data =JsonConvert.DeserializeObject<confirmSingleForQueueData>(responses.data.ToString());
                    isOk = data.submitStatus;
                    msg = "确定下单成功";
                }
                else
                {
                    throw new Exception(responses.data.ToString());
                }
              
            }
            catch (Exception ex)
            {
                Logger.Error($"确认是否下单成功,发生错误:{ex.ToString()}");
                 msg = "下单-预售下单-确认是否下单成功" + ex.Message; //返回给用户的错误
            }
          
            return isOk;
        }
        /// <summary>
        /// 下单-预售下单-等待出票
        /// </summary>
        /// <returns></returns>
        public static queryOrderWaitTimeResponseData queryOrderWaitTime(out string msg)
        {
            var random = RandomHelper.GenerateRandomCode(13);
            var strDictionary = new BaseDictionary()
            {
                {"random",random},//13位随机数
                {"tourFlag","dc" },//成人或者学生
                {"_json_att","" },
            };
            var postData = strDictionary.GetParmarStr();
            try
            {
                Logger.Info($"下单-预售下单-等待出票_queryOrderWaitTime参数{postData},时间{DateTime.Now.ToString()}");
                var responses = HttpHelper.StringPost(UrlConfig.queryOrderWaitTime, postData, _cookie);
                Logger.Info($"下单-预售下单-等待出票_queryOrderWaitTime返回时间{DateTime.Now.ToString()}");
                var response = JsonConvert.DeserializeObject<queryOrderWaitTimeResponse>(responses);
                if (response.httpstatus == "200" && response.status == "true")
                {
                    var data = JsonConvert.DeserializeObject<queryOrderWaitTimeResponseData>(response.data.ToString());
                    if (Convert.ToInt32(data.waitTime) > 1000)
                    {
                        msg = "等待时长超过1000S，放弃排队，去订单中心取消此订单";
                        //go to 如果等待时长超过1000S，放弃排队，去订单中心取消此订单
                    }
                    //else
                    //{
                    //    Thread.Sleep(Convert.ToInt32(data.waitTime));
                    //}
                    msg = "购票成功";
                    return data;
                }
                else
                {
                    throw new Exception(response.data.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.Error($" 下单-预售下单-等待出票:{ex.ToString()}");
                msg =string.Format("等待出票出错{0},{1}", ex.Message,ex.StackTrace);
            }
            msg = "出票失败";
            return null;
        }

        /// <summary>
        /// 下单-预售下单-等待出票 重复10次
        /// </summary>
        /// <returns></returns>
        public static queryOrderWaitTimeResponseData taskqueryOrderWaitTime(out string msg)
        {
            var order = new queryOrderWaitTimeResponseData();
            var isContinue = true;
            msg = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                while (isContinue)
                {
                    Thread.Sleep(1000);
                    var orderInfo = queryOrderWaitTime(out msg);
                    if (!string.IsNullOrEmpty(orderInfo.orderId))
                    {
                        isContinue = false;
                        order = orderInfo;
                        i = 9;
                        break;
                    }

                    if (i == 9)
                    {
                        isContinue = false;
                        break;
                    }
                }
            }
            msg = JsonConvert.SerializeObject(order); 
            return order;
        }

        /// <summary>
        /// 候补-人脸验证
        /// </summary>
        /// <param name="secretStr">订单加密字符串</param>
        /// <param name="seatType">坐席</param>
        /// <returns></returns>
        public static bool chechFace(string secretStr,string seatType)
        {
            var isOk = false;
            //secretList 订单加密字符串+ “#” + 坐席 + 竖划线
            var secretList = string.Format("{0}#{1}|", secretStr, seatType);
            var strDictionary = new BaseDictionary
            {
                {"secretList",secretList},
                {"_json_att","" },
            };
            var postData = strDictionary.GetParmarStr();
            try
            {
                var responses = HttpHelper.StringPost(UrlConfig.chechFace, postData, _cookie);
                var response = JsonConvert.DeserializeObject<chechFaceResponse>(responses);
                if (response.httpstatus == "200" && response.status == "true")
                {
                    //需判断face_check_code和face_flag条件都满足才行
                    if (response.data.face_flag==true&&response.data.face_check_code=="12")
                    {
                        //只有返回12的时候才可以下候补订单
                        return isOk=true;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Logger.Error($" 候补-人脸验证:{ex.ToString()}");
            }

            return isOk;
        }

        /// <summary>
        /// 候补-加入候补列表
        /// </summary>
        /// <param name="secretStr">订单加密字符串</param>
        /// <param name="seatType">坐席号</param>
        /// <returns></returns>
        public static List<getSuccessResponseRateData> getSuccessRate(string secretStr, string seatCode)
        {
            // secretList 加密字符串 +#+坐席号（坐席号说明见查询api）
            var secretList = string.Format("{0}#{1}", secretStr, seatCode);
            var strDictionary = new BaseDictionary
            {
                {"secretList",secretList},
                {"_json_att","" },
            };
            var postData = strDictionary.GetParmarStr();
            try
            {
                var responses = HttpHelper.StringPost(UrlConfig.getSuccessRate, postData, _cookie);
                var response = JsonConvert.DeserializeObject<getSuccessRateResponse>(responses);
                if (response.httpstatus == "200" && response.status == "true")
                {
                    return response.data.flag;

                }
            }
            catch (Exception ex)
            {
                Logger.Error($" 候补-加入候补列表:{ex.ToString()}");
            }

            return null;
        }

        /// <summary>
        /// 候补-提交候补订单
        /// </summary>
        /// <param name="secretStr">订单加密字符串</param>
        /// <param name="seatCode">坐席号</param>
        /// <returns></returns>
        public static bool submitOrderRequest(string secretStr, string seatCode)
        {
            // secretList 加密字符串+ # + 坐席号+ 竖划线
            var isOk = false;
            var secretList = string.Format("{0}#{1}|", secretStr, seatCode);
            var strDictionary = new BaseDictionary
            {
                {"secretList",secretList},
                {"_json_att","" },
            };
            var postData = strDictionary.GetParmarStr();
            try
            {
                var responses = HttpHelper.StringPost(UrlConfig.submitAfterNateOrderRequest, postData, _cookie);
                var response = JsonConvert.DeserializeObject<submitOrderRequestResponse>(responses);
                if (response.httpstatus == "200" && response.status == "true")
                {
                    return isOk=response.data.flag;

                }
            }
            catch (Exception ex)
            {
                Logger.Error($" 候补-提交候补订单:{ex.ToString()}");
            }

            return isOk;
        }

        /// <summary>
        /// 候补-获取候补信息
        /// </summary>
        /// <returns></returns>
        public static passengerInitApiData passengerInitApi()
        {
            try
            {
                var responses = HttpHelper.StringGet(UrlConfig.passengerInitApi, _cookie);
                var response = JsonConvert.DeserializeObject<passengerInitApiResponse>(responses);
                if (response.httpstatus == "200" && response.status == "true")
                {
                    return   response.data;

                }
            }
            catch (Exception ex)
            {
                Logger.Error($" 候补-获取候补信息:{ex.ToString()}");
            }

            return null;
        }

        /// <summary>
        /// 候补-提交候补信息
        /// </summary>
        /// <param name="passengerInfo">乘客信息</param>
        /// <param name="jzParam">passengerInitApi中获取的jzdhDate</param>
        /// <param name="hbTrain">车次代号+”,”+坐次编号+ “#”</param>
        /// <param name="lkParam">”</param>
        /// <returns></returns>
        public static confirmHBData confirmHB(string passengerInfo, string jzParam, string hbTrain, string lkParam = "")
        {
            // 备注： 多个乘客信息ex: 1#文贤平#1#4305*77X#bf6ae40d3655ae7eff005ee21d95876b38ab97a8031b464bc2f74a067e3ec957;
            // 1#仓静#1#4301*025#525bee00da98414fffbee6d44d1c15f02ed9388b376db9a1d5d361f9a9d1c60b;
            //单个乘客信息ex: 1#XXXX#1#*77X#bf6ae40d3655ae7eff005ee21d95876b38ab97a8031b464bc2f74a067e3ec957;
            //hbTrain ex: (单个)5l000G177230,O# （多个） 5l000G137752,O#5l000G137752,M#
            var isOk = false;
            var strDictionary = new BaseDictionary
            {
                {"passengerInfo",passengerInfo},
                {"jzParam",jzParam },
                {"hbTrain",hbTrain },
                {"lkParam","" },
            };
            var postData = strDictionary.GetParmarStr();
            try
            {
                var responses = HttpHelper.StringPost(UrlConfig.confirmHB, postData, _cookie);
                var response = JsonConvert.DeserializeObject<confirmHBResponse>(responses);
                if (response.httpstatus == "200" && response.status == "true")
                {
                    return response.data;

                }
            }
            catch (Exception ex)
            {
                Logger.Error($" 候补-提交候补信息:{ex.ToString()}");
            }

            return null;
        }

        /// <summary>
        /// 候补-候补排队
        /// </summary>
        /// <returns></returns>
        public static queryQueueData queryQueue()
        {
            try
            {
                var responses = HttpHelper.StringPost(UrlConfig.queryQueue, "", _cookie);
                var response = JsonConvert.DeserializeObject<queryQueueResponse>(responses);
                if (response.httpstatus == "200" && response.status == "true")
                {
                    return response.data;

                }
            }
            catch (Exception ex)
            {
                Logger.Error($" 候补-候补排队:{ex.ToString()}");
            }

            return null;
        }
        /// <summary>
        /// 查询我的订单
        /// </summary>
        /// <returns></returns>
        public static queryMyOrderNoCompleteData queryMyOrderNoComplete()
        {

            var strDictionary = new BaseDictionary
            {
                {"_json_att","" },
            };
            var postData = strDictionary.GetParmarStr();
            try
            {
                var responses = HttpHelper.StringPost(UrlConfig.queryMyOrderNoComplete, postData, _cookie);
                var response = JsonConvert.DeserializeObject<queryMyOrderNoCompleteResponse>(responses);
                if (response.httpstatus == "200" && response.status == "true")
                {
                    var Data = JsonConvert.DeserializeObject<queryMyOrderNoCompleteData>(response.data.ToString());
                    return Data;

                }
            }
            catch (Exception ex)
            {
                Logger.Error($" 查询我的订单:{ex.ToString()}");
            }

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
