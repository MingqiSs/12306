using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12036ByTicket.Model.Dto
{
    public class CerifyCaptchaCodeRP
    {
        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Massage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Data { get; set; }
    }
}
