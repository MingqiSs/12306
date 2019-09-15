using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace Common.Logic
{
    /// <summary>
    /// 扩展键值对类
    /// </summary>
    public class BaseDictionary : Dictionary<string, string>
    {
        public BaseDictionary()
        {

        }

        /// <summary>
        ///当前排序  
        /// </summary>
        public void Sort(SortEnum pse)
        {
            List<string> listKeys = this.Keys.ToList();
            listKeys.Sort(new SortCamparer(pse));
            BaseDictionary pd = new BaseDictionary();
            foreach (string r in listKeys)
            {
                pd.Add(r, this[r]);
            }
            this.Clear();
            foreach (string r in listKeys)
            {
                this.Add(r, pd[r]);
            }
            pd.Clear();
            pd = null;
        }

        /// <summary>
        /// 按keys值排序，排除keys之外的
        /// </summary>
        public BaseDictionary Sort(string[] keys)
        {
            List<string> listKeys = keys.ToList();
            BaseDictionary pd = new BaseDictionary();
            foreach (string r in listKeys)
            {
                pd.Add(r, this[r]);
            }
            return pd;
        }

        /// <summary>
        /// 获取参数形式字符串
        /// </summary>
        /// <returns></returns>
        public string GetParmarStr()
        {
            StringBuilder result = new StringBuilder();
            this.Aggregate(result, (s, b) => s.Append(b.Key + "=" + b.Value + "&"));
            return result.ToString().TrimEnd('&');
        }

        /// <summary>
        /// 转换 QueryString 参数值
        /// </summary>
        public static BaseDictionary TransQueryString(string msg)
        {
           BaseDictionary par = new BaseDictionary();
            string[] split = msg.Split(new Char[] { '&' });
            for (int i = 0; i < split.Length; i++)
            {
                int pos = split[i].IndexOf('=');
                if (pos > 0)
                {
                    if (pos < split[i].Length - 1)
                    {
                        par.Add(split[i].Substring(0, pos), split[i].Substring(pos + 1));
                    }
                    else
                    {
                        par.Add(split[i].Substring(0, pos), "");
                    }
                }
            }
            return par;
        }

        /// <summary>
        /// 参数按照ASCII码从小到大排序
        /// </summary>
        /// <param name="paramsMap"></param>
        /// <returns></returns>
        public static String getParamSrc(Dictionary<string, string> paramsMap)
        {
            var vDic = (from objDic in paramsMap orderby objDic.Key ascending select objDic);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in vDic)
            {
                string pkey = kv.Key;
                string pvalue = kv.Value;
                str.Append(pkey + "=" + pvalue + "&");
            }

            String result = str.ToString().Substring(0, str.ToString().Length - 1);
            return result;
        }

    }
    public enum SortEnum
    {
        /// <summary>
        /// 降序
        /// </summary>
        Asc = 0,
        /// <summary>
        /// 升序
        /// </summary>
        Desc = 1
    }

    /// <summary>
    /// 比較器，用於排序
    /// </summary>
    public class SortCamparer : IComparer<string>
    {
        private SortEnum pse { get; set; }
        public SortCamparer(SortEnum p)
        {
            pse = p;
        }

        public int Compare(string obj1, string obj2)
        {
            if (pse == SortEnum.Asc)
            {
                if (string.Compare(obj1, obj2, StringComparison.Ordinal) > 0)
                    return 1;
                else if (string.Compare(obj1, obj2, StringComparison.Ordinal) < 0)
                    return -1;
            }
            else
            {
                if (string.Compare(obj1, obj2, StringComparison.Ordinal) > 0)
                    return -1;
                else if (string.Compare(obj1, obj2, StringComparison.Ordinal) < 0)
                    return 1;
            }
            return 0;
        }
    }
}
