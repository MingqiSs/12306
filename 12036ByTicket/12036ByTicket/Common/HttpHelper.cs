﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _12036ByTicket.Common
{
  public  class HttpHelper
    {
        public static Stream Get(string agent, string url, CookieContainer cookie)
        {
            try
            {
                ServicePointManager.Expect100Continue = false;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = agent;
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.Method = "GET";
                request.KeepAlive = true;
                request.CookieContainer = cookie;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
               
                return responseStream;
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
            Stream queryStream = Get(agent, url, cookie);
            StreamReader queryReader = new StreamReader(queryStream, Encoding.UTF8);
            string content = queryReader.ReadToEnd();
            queryReader.Close();
            return content;
        }

        /// <summary>
        /// 返回json
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static HttpJsonEntity<Dictionary<string, string>> Post(string agent, string url, string data, CookieContainer cookie)
        {
            string responseContent = StringPost(agent, url, data, cookie);
            HttpJsonEntity<Dictionary<string, string>> retDic =
                null;
            return retDic;
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
             }
            return responseContent;
        }
    }
}
