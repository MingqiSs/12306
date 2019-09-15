using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12036ByTicket.Common
{
   public static class UrlConfig
    {

        private static string baseUrl = "https://kyfw.12306.cn";

        public static string left_Ticket_init = $"{baseUrl}/otn/leftTicket/init";
        /// <summary>
        /// 登录验证码
        /// </summary>
        public static string captcha =  baseUrl +"/passport/captcha/captcha-image64?login_site=E&module=login&rand=sjrand&{0}&callback=jQuery19108016482864806321_1554298927290&_=1554298927293";
        /// <summary>
        /// 验证码校验
        /// </summary>
        public static string captcha_Check = baseUrl+ "/passport/captcha/captcha-check?callback=jQuery19108016482864806321_1554298927290&answer={0}&rand=sjrand&login_site=E&_={1}";
        /// <summary>
        /// 登陆
        /// </summary>
        public static string login = $"{baseUrl}/passport/web/login";

    }
}
