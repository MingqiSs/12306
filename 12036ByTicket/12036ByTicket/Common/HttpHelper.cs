using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _12036ByTicket.Common
{
    /// <summary>
    /// 请求12306接口http帮助类
    /// </summary>
    public class HttpHelper
    {
        private static string contentType = "application/x-www-form-urlencoded";
        //private static string accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
        private static string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
        private static string referer = "https://kyfw.12306.cn/otn/resources/login.html";
        public static HttpWebResponse Get( string url, CookieContainer cookie)
        {
            try
            {
                Logger.Info($"请求地址:{url},请求方式:Get");
                ServicePointManager.Expect100Continue = false;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = userAgent;
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.Method = "GET";
                request.Referer = referer;
                request.KeepAlive = true;
                request.CookieContainer = cookie;
                return (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 返回string类型
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string StringGet( string url, CookieContainer cookie)
        {
            string content = string.Empty;
            var response = Get(url, cookie);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream queryStream = response.GetResponseStream();

                StreamReader queryReader = new StreamReader(queryStream, Encoding.UTF8);
                 content = queryReader.ReadToEnd();
                queryReader.Close();
                Logger.Info($"请求地址:{url},response:{content}");
            }
            return content;
        }

        /// <summary>
        /// post
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static HttpWebResponse Post( string url, string data, CookieContainer cookie)
        {
            Logger.Info($"请求地址:{url},请求方式:Post,参数:{data}");
            ServicePointManager.Expect100Continue = false;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = userAgent;
            request.ContentType = contentType;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Referer = referer;
            request.CookieContainer = cookie;
            if (!string.IsNullOrEmpty(data))
            {
                string postDataStr = data;
                byte[] postData = Encoding.UTF8.GetBytes(postDataStr);
                request.ContentLength = postData.Length;
                var requestStream = request.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
            }
           return (HttpWebResponse)request.GetResponse();
           
              
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string StringPost(string url, string data, CookieContainer cookie)
        {
            string responseContent =string.Empty;
            try
            {
               var response= Post(url, data, cookie);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        StreamReader responseStreamReader = new StreamReader(responseStream, Encoding.UTF8);
                        responseContent = responseStreamReader.ReadToEnd();
                        responseStreamReader.Close();
                        Logger.Info($"请求接口:{url},response:{responseContent}");
                    }
                }
               
            }
            catch (Exception ex)
            {
                Logger.Error($"请求异常错误:{ex.ToString()}");
                Logger.Error($"请求接口:{url},response:{responseContent}");
            }
            return responseContent;
        }
    }
}
