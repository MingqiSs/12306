using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12036ByTicket.Common
{
    public class Logger
    {
        private static Logger _sLogger;

        private readonly ILog _logger;
        private readonly string _title = string.Empty;

        #region 静态方法
        public static void Init(ILog logger, string title)
        {
            _sLogger = new Logger(logger, title);
            _sLogger.WirteInfo("采集日志初始化完成。");
        }

        public static void Error(string error)
        {
            Log.WirteError(error);
        }

        public static void Info(string info)
        {
            Log.WirteInfo(info);
        }

        private static Logger Log => _sLogger ?? (_sLogger = new Logger());

        #endregion 静态方法

        protected Logger()
        {
            _logger = LogManager.GetLogger("InfoLog");
        }
        protected Logger(ILog logger, string titel)
        {
            _title = titel;
            _logger = logger;
        }

        #region 方法
        protected void WirteError(string error)
        {
            _logger.Error(_title + ":" + error);
        }

        protected void WirteInfo(string info)
        {
            _logger.Info(_title + ":" + info);
        }
        #endregion 方法
    }
}
