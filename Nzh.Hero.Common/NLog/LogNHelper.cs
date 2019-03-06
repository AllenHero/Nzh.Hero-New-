using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Common.NLog
{
    public class LogNHelper
    {
        private static Logger logger = null;

        static LogNHelper()
        {
            logger = LogManager.GetLogger("HeroLog:");
        }

        public static void Info(string message)
        {
            if (logger.IsInfoEnabled)
            {
                logger.Info(message);
            }
        }

        public static void Exception(string message)
        {
            if (logger.IsErrorEnabled)
            {
                logger.Error(message);
            }
        }

        public static void Exception(Exception ex)
        {
            if (logger.IsErrorEnabled)
            {
                logger.Error(ex);
            }
        }

        public static void Warn(string message)
        {
            if (logger.IsWarnEnabled)
            {
                logger.Warn(message);
            };
        }

        public static void Trace(string message)
        {
            if (logger.IsTraceEnabled)
            {
                logger.Trace(message);
            }
        }

        public static void Debug(string message)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug(message);
            }
        }

        public static void Fatal(string message)
        {
            if (logger.IsFatalEnabled)
            {
                logger.Fatal(message);
            }
        }
    }
}
