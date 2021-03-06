﻿using System;
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
        public static string uamtkstatic = $"{baseUrl}/passport/web/auth/uamtk-static";

        public static string loginCode = "https://kyfw.12306.cn/passport/captcha/captcha-image";

        /// <summary>
        /// 登录步骤之获取用户名
        /// appid 写死 otn
        /// </summary>
        public static string uamtk = $"{baseUrl}/passport/web/auth/uamtk";
        /// <summary>
        /// 登录步骤之获取用户名
        /// appid 写死 otn
        /// </summary>
        public static string uamauthclient = $"{baseUrl}/otn/uamauthclient";

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
        public static string getLogdevice = $"{baseUrl}/otn/HttpZF/logdevice";

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
        //public static string getLogdevice = "https://12306-rail-id.pjialin.com/";

        /// <summary>
        /// 查询车次
        /// </summary>
        public static string query =baseUrl+ "/otn/leftTicket/query{3}?leftTicketDTO.train_date={0}&leftTicketDTO.from_station={1}&leftTicketDTO.to_station={2}&purpose_codes=ADULT";

        /// <summary>
        /// 获取的浏览器session
        /// </summary>
        public static string left_Ticket_init = $"{baseUrl}/otn/leftTicket/init?linktypeid=dc";
        /// <summary>
        /// 获取联系人
        /// </summary>
        public static string getPassenger = $"{baseUrl}/otn/confirmPassenger/getPassengerDTOs";
        /// <summary>
        /// 检查用户登录
        /// </summary>
        public static string check_user_url = $"{baseUrl}/otn/login/checkUser";
        /// <summary>
        /// 下单-预售下单-进入订单生成页
        /// </summary>
        public static string initDc = $"{baseUrl}/otn/confirmPassenger/initDc";

        /// <summary>
        /// 下单-预售下单-等待出票
        /// </summary>
        public static string queryOrderWaitTime = $"{baseUrl}/otn/confirmPassenger/queryOrderWaitTime?random={0}&tourFlag=dc&_json_att=";

        /// <summary>
        /// 下单-预售下单-订单排队
        /// </summary>
        public static string getQueueCount = $"{baseUrl}/otn/confirmPassenger/getQueueCount";

        /// <summary>
        /// 下单-预售下单-校验订单信息
        /// </summary>
        public static string checkOrderInfo = $"{baseUrl}/otn/confirmPassenger/checkOrderInfo";

        /// <summary>
        /// 下单-预售下单-提交订单
        /// </summary>
        public static string submitOrderRequest = $"{baseUrl}/otn/leftTicket/submitOrderRequest";

        /// <summary>
        /// 下单-确认订单
        /// </summary>
        public static string confirmSingleForQueue = $"{baseUrl}/otn/confirmPassenger/confirmSingleForQueue";

        /// <summary>
        /// 候补排队
        /// </summary>
        public static string queryQueue = $"{baseUrl}/otn/afterNate/queryQueue";

        /// <summary>
        /// 人脸验证
        /// </summary>
        public static string chechFace = $"{baseUrl}/otn/afterNate/chechFace";

        /// <summary>
        /// 提交候补信息
        /// </summary>
        public static string confirmHB = $"{baseUrl}/otn/afterNate/confirmHB";

        /// <summary>
        /// 获取候补信息
        /// </summary>
        public static string passengerInitApi = $"{baseUrl}/otn/afterNate/passengerInitApi";

        /// <summary>
        /// 提交候补订单
        /// </summary>
        public static string submitAfterNateOrderRequest = $"{baseUrl}/otn/afterNate/submitOrderRequest";

        /// <summary>
        /// 加入候补列表
        /// </summary>
        public static string getSuccessRate = $"{baseUrl}/otn/afterNate/getSuccessRate";


        /// <summary>
        /// 查询我的订单
        /// </summary>
        public static string queryMyOrderNoComplete = $"{baseUrl}/otn/queryOrder/queryMyOrderNoComplete";
    }
}
