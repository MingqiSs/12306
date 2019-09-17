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
    public class HttpHelper
    {
        public static HttpWebResponse Get(string agent, string url, CookieContainer cookie)
        {
            try
            {
                ServicePointManager.Expect100Continue = false;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = agent;
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.Method = "GET";
                //request.Referer = "https://kyfw.12306.cn/otn/resources/login.html";
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
        public static string StringGet(string agent, string url, CookieContainer cookie)
        {
            Stream queryStream = Get(agent, url, cookie).GetResponseStream();
            StreamReader queryReader = new StreamReader(queryStream, Encoding.UTF8);
            string content = queryReader.ReadToEnd();
            queryReader.Close();
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
        public static HttpWebResponse Post(string agent, string url, string data, CookieContainer cookie)
        {
            HttpWebResponse response = null;

            Logger.Info($"请求地址:{url},data:{data}");
            ServicePointManager.Expect100Continue = false;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = agent;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Method = "POST";
            request.KeepAlive = true;
            request.CookieContainer = cookie;
            if (!string.IsNullOrEmpty(data))
            {
                string postDataStr = data;
                byte[] postData = Encoding.UTF8.GetBytes(postDataStr);
                request.ContentLength = postData.Length;
                var requestStream = request.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
            }
            response = (HttpWebResponse)request.GetResponse();
            return response;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string StringPost(string agent, string url, string data, CookieContainer cookie)
        {
            string responseContent = "";
            try
            {
                Logger.Info($"请求地址:{url},data:{data}");
                ServicePointManager.Expect100Continue = false;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = agent;
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.Method = "POST";
                request.KeepAlive = true;
                request.CookieContainer = cookie;
                if (!string.IsNullOrEmpty(data))
                {
                    string postDataStr = data;
                    byte[] postData = Encoding.UTF8.GetBytes(postDataStr);
                    request.ContentLength = postData.Length;
                    var requestStream = request.GetRequestStream();
                    requestStream.Write(postData, 0, postData.Length);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    StreamReader responseStreamReader = new StreamReader(responseStream, Encoding.UTF8);
                    responseContent = responseStreamReader.ReadToEnd();
                    responseStreamReader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"请求异常错误:{ex.ToString()}");
            }
            return responseContent;
        }
    }
}
