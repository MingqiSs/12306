using _12036ByTicket.Common;
using Newtonsoft.Json;
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
            foreach (Cookie cookie in response.Cookies) _cookie.Add(cookie);
            _cookie.Add(new Cookie("RAIL_EXPIRATION", "1568666175873","", "kyfw.12306.cn"));
            _cookie.Add(new Cookie("RAIL_DEVICEID",
                "VZcpy-vSck_0i1MmFcc4Y-nKkT30oO52BZugg_aaWyXYGCIonpZSV1RFVKZZm7CnOUHn407QsQXnACiI6fuexbGdMmIb9mw9_VjEENIkuykE4WqgHp4yaUjsWowNiYoYDVex-iwf6UOaheyjD4ZKPh3Yo3Ubpr0c",
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
        /// 校验验证码
        /// </summary>
        /// <returns></returns>
        public static bool CheckCaptcha(string randCode)
        {
            try
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
                var result_code = ((dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Split('(')[1].Split(')')[0]))["result_code"];
                if ((int)result_code == 4)
                {
                    return true;
                }
                else if ((int)result_code == 5)
                {
                    MessageBox.Show("验证码校验失败！");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"校验验证码失败{ex}");
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
                passWord, randCode,appId);
            string response = HttpHelper.StringPost(DefaultAgent, UrlConfig.login, postData, _cookie);
            if (string.IsNullOrWhiteSpace(response)) return false;
            // {
            //                "result_message": "登录成功",
            //"uamtk": "0KqvJJKDbWKtKnFkNmIrYwBs7ISoA2ui5e08x6DSl5k51x1x0",
            //"result_code": 0
            // }
            dynamic result = JsonConvert.DeserializeObject(response);
            if (result.result_code == 0)
            {
                return true;
                //登录成功
            }
            else {
                return true;
                //登录失败
            }
            return false;
        }

       

    }
}
