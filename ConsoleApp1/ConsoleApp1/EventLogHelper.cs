using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ConsoleApp1
{
    public class EventLogHelper
    {
        private static EventLog eventLog;

        static EventLogHelper()
        {
            eventLog = new EventLog();
            eventLog.Source = "Application";
            // 如果要创建新的Source，则需要管理员权限
            //eventLog.Source = "DEMO";
        }

        public static void Information(string message)
        {
            eventLog.WriteEntry(message, EventLogEntryType.Information, 0);
        }
    }
}
