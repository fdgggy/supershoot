using System;
using System.Diagnostics;
using System.Text;

namespace NLog
{
    public class Logger
    {
        public event LogDelegate OnLog;
        private StringBuilder stringBuilder = new StringBuilder();

        public delegate void LogDelegate(Logger logger, LogLevel logLevel, string message);

        public LogLevel logLevel { get; set; }

        public string name { get; private set; }

        public Logger(string name)
        {
            this.name = name;
        }

        #region debug
        public void Trace(string format, params object[] args)
        {
            string res = GetInfoStr(LogLevel.Trace, format, args);
            log(LogLevel.Trace, res);
        }

        public void Debug(string format, params object[] args)
        {
            string res = GetInfoStr(LogLevel.Debug, format, args);
            log(LogLevel.Debug, res);
        }

        public void Info(string format, params object[] args)
        {
            string res = GetInfoStr(LogLevel.Info, format, args);
            log(LogLevel.Info, res);
        }
        #endregion debug

        public void Warn(string format, params object[] args)
        {
            string res = GetInfoStr(LogLevel.Warn, format, args);
            log(LogLevel.Warn, res);
        }

        public void Error(string format, params object[] args)
        {
            string res = GetInfoStr(LogLevel.Error, format, args);
            log(LogLevel.Error, res);
        }

        public void Fatal(string format, params object[] args)
        {
            string res = GetInfoStr(LogLevel.Fatal, format, args);
            log(LogLevel.Fatal, res);
        }

        public void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new NLogAssertException(message);
            }
        }

        void log(LogLevel logLvl, string message)
        {
            if (OnLog != null && logLevel <= logLvl)
            {
                OnLog(this, logLvl, message);
            }
        }

        private string GetInfoStr(LogLevel logLvl, string format, object[] args)
        {
            if (string.IsNullOrEmpty(format))
            {
                return "Log format string Error";
            }

            stringBuilder.Remove(0, stringBuilder.Length);

            if (args.Length <= 0)
            {
                stringBuilder.Append(format);
            }
            else
            {
                stringBuilder.AppendFormat(format, args);
            }

            if (logLvl >= LogLevel.Error)   //注意，在移动平台获取不到堆栈信息，只能这样获取
            {
                StackTrace trace = new StackTrace(true);
                stringBuilder.Append(trace.ToString());
            }

            return stringBuilder.ToString();
        }
    }

    public class NLogAssertException : Exception
    {
        public NLogAssertException(string message) : base(message)
        {
        }
    }
}

