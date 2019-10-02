using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _12036ByTicket.Common
{
  public   class PramHelper
    {
        /// <summary>
        /// 反射得到实体类的字段名称和值
        /// var dict = GetProperties(model);
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="t">实例化</param>
        /// <returns></returns>
        public static Dictionary<object, object> GetProperties<T>(T t)
        {
            var ret = new Dictionary<object, object>();
            if (t == null) { return null; }
            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0) { return null; }
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    ret.Add(name, value);
                }
            }       
            return ret ;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dic"></param>
        public void RemoveNullKey(Dictionary<object, object> dic)
        {
            string keyNames = string.Empty;
            foreach (string key in dic.Keys) if (dic[key] == null || dic[key].ToString() == "") keyNames += key + ",";
            string[] str_keyNames = keyNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string key in str_keyNames) dic.Remove(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramsMap"></param>
        /// <returns></returns>
        public static String GetParamSrc(Dictionary<object, object> paramsMap)
        {
            var vDic = (from objDic in paramsMap orderby objDic.Key ascending select objDic);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<object, object> kv in vDic)
            {
                object pkey = kv.Key;
                object pvalue = kv.Value;
                str.Append(pkey + "=" + pvalue + "&");
            }
            String result = str.ToString().Substring(0, str.ToString().Length - 1);
            return result;
        }

    }
}
