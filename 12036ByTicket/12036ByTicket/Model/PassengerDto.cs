using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12036ByTicket.Services
{
    public class Normal_passengersItem
    {
        /// <summary>
        /// 向明其
        /// </summary>
        public string passenger_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sex_code { get; set; }
        /// <summary>
        /// 男
        /// </summary>
        public string sex_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string born_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string country_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string passenger_id_type_code { get; set; }
        /// <summary>
        /// 中国居民身份证
        /// </summary>
        public string passenger_id_type_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string passenger_id_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string passenger_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string passenger_flag { get; set; }
        /// <summary>
        /// 成人
        /// </summary>
        public string passenger_type_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mobile_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string phone_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string postalcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string first_letter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string recordCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string total_times { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string index_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string allEncStr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isAdult { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isYongThan10 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isYongThan14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isOldThan60 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gat_born_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gat_valid_date_start { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gat_valid_date_end { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gat_version { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public string notify_for_gat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isExist { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string exMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> two_isOpenClick { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> other_isOpenClick { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Normal_passengersItem> normal_passengers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> dj_passengers { get; set; }
    }

    public class ValidateMessages
    {
    }

    public class PassengerDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string validateMessagesShowId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int httpstatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> messages { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ValidateMessages validateMessages { get; set; }
    }
}
