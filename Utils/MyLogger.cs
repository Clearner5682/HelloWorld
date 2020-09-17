using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class MyLoger
    {
        string LocalLogDir;

        public MyLoger()
        {
            LocalLogDir = AppDomain.CurrentDomain.BaseDirectory + "\\Log";
            try
            {
                System.IO.Directory.CreateDirectory(LocalLogDir);
            }
            catch
            {
            }
        }

        public MyLoger(string dir, string catalog)
        {
            _Catelog = catalog;
            LocalLogDir = dir;
            try
            {
                System.IO.Directory.CreateDirectory(LocalLogDir);
            }
            catch
            {
            }
        }

        public MyLoger(string catalog)
        {
            _Catelog = catalog;
            LocalLogDir = AppDomain.CurrentDomain.BaseDirectory + "\\Log";
            try
            {
                System.IO.Directory.CreateDirectory(LocalLogDir);
            }
            catch
            {
            }
        }

        string _Catelog;

        public string Catelog
        {
            get { return _Catelog; }
            set { _Catelog = value; }
        }

        private bool? IsDebugInfo { get; set; }

        private void LogInfo(string msg, string FromModel, string StackTrace, string Errlevel, Exception ex)
        {
            lock (this)
            {
                string Logfile = string.Format("{0}\\{1}{2:yyyy-MM-dd-HH}.log", LocalLogDir, Catelog, System.DateTime.Now);
                string logContent = "";
                if (Errlevel == "Error" || Errlevel == "Debug")
                {
                    logContent = string.Format("MallLogInfo:{0},{1},{2}\r\n{3}\r\nStackTrace:\r\n{4}\r\n\r\n", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), Errlevel, msg, ex, StackTrace);
                }
                else
                {
                    try
                    {
                        logContent = string.Format("MallLog:{0},{1},{2}\r\n{3}\r\n\r\n", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), Errlevel, msg, ex);
                    }
                    catch
                    {
                        try
                        {

                            logContent = string.Format("MallLog:{0},{1},{2}\r\n{3}\r\n\r\n", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), Errlevel, msg, ex.Message);
                        }
                        catch
                        {
                            logContent = "UnKnow error";
                        }
                    }
                }
                try
                {
                    System.IO.File.AppendAllText(Logfile, logContent);
                }
                catch
                {
                }

            }
        }

        public void Info(string msg)
        {
            LogInfo(msg, System.Reflection.Assembly.GetCallingAssembly().FullName, null, "Info", null);
        }

        public void Debug(string msg, Exception ex)
        {
#if DEBUG
            LogInfo(msg, System.Reflection.Assembly.GetCallingAssembly().FullName, Environment.StackTrace, "Debug", ex);
#endif
        }

        public void Error(string msg, Exception ex)
        {
#if DEBUG
            LogInfo(msg, System.Reflection.Assembly.GetCallingAssembly().FullName, Environment.StackTrace, "Error", ex);
#else
            LogInfo(msg, System.Reflection.Assembly.GetCallingAssembly().FullName, null, "Info", ex);
#endif
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="msg"></param>
        public void DebugInfo(string msg)
        {
            if (IsNeedDebugInfo())
                LogInfo(msg, System.Reflection.Assembly.GetCallingAssembly().FullName, null, "Info", null);
        }

        /// <summary>
        /// 是否需要写debuginfo
        /// </summary>
        /// <returns></returns>
        private bool IsNeedDebugInfo()
        {
            return true;
        }
    }
}
