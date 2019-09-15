using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12036ByTicket.LogicModel
{
    public class Rail
    {
        public string RAIL_EXPIRATION { get; set; }

        public string RAIL_DEVICEID { get; set; }
    }

    public class StationNames
    {
        /// <summary>
        /// PS @bjb
        /// </summary>
        public string nameCode { get; set; }

        /// <summary>
        /// PS 北京北
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// PS VAP
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// beijingbei 拼音
        /// </summary>
        public string pinYin { get; set; }

        /// <summary>
        /// 缩写 bjb
        /// </summary>
        public string pinYinInitials { get; set; }

        /// <summary>
        /// 没啥用的 0
        /// </summary>
        public string id { get; set; }
    }
}
