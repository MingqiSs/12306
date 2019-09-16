using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12036ByTicket.Common
{
   public static class UrlConfig
    {

        public static string baseUrl = "https://kyfw.12306.cn";
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
        /// <summary>
        /// 授权
        /// </summary>
        public static string auth = $"{baseUrl}/passport/web/auth/uamtk-static";

        public static string loginCode = "https://kyfw.12306.cn/passport/captcha/captcha-image";

        /// <summary>
        /// 登录步骤之获取用户名
        /// appid 写死 otn
        /// </summary>
        public static string getUserName = $"{baseUrl}/passport/web/auth/uamtk";

        /// <summary>
        /// 获取的浏览器session,为代码入口
        /// </summary>
        public static string getSession = $"{baseUrl}/otn/leftTicket/init";

        /// <summary>
        /// 获取加密指纹
        /// </summary>
        public static string getJS = $"{baseUrl}/otn/HttpZF/GetJS";
        ///// <summary>
        ///// 获取浏览器指纹
        ///// </summary>
        //public static string getLogdevice = $"{baseUrl}/otn/HttpZF/logdevice";

        /// <summary>
        /// 检查用户是否登陆
        /// </summary>
        public static string checkUser = $"{baseUrl}/otn/login/checkUser";

        /// <summary>
        /// 获取站点信息 需要UTF转码
        /// </summary>
        public static string getFavoriteNname = "https://www.12306.cn/index/script/core/common/station_name_v10037.js";

        /// <summary>
        /// 获取浏览器指纹
        /// </summary>
        public static string getLogdevice = "https://12306-rail-id.pjialin.com/";

        /// <summary>
        /// 查询车次
        /// </summary>
        public static string query =baseUrl+ "/otn/leftTicket/query{3}?leftTicketDTO.train_date={0}&leftTicketDTO.from_station={1}&leftTicketDTO.to_station={2}&purpose_codes=ADULT";

        /// <summary>
        /// 获取的浏览器session
        /// </summary>
        public static string left_Ticket_init = $"{baseUrl}/otn/leftTicket/init";
        /// <summary>
        /// 获取联系人
        /// </summary>
        public static string getPassenger = $"{baseUrl}/otn/confirmPassenger/getPassengerDTOs";
    }
}
