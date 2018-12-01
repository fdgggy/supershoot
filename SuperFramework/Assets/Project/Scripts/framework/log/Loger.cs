using NLog;

/// <summary>
/// Loger 用于主线程日志，网络日志用另一个log打印，不然会有资源抢占的问题
/// </summary>
public static class Loger
{
    private static readonly NLog.Logger log = LoggerFactory.GetLogger(typeof(Loger).Name);
    public static void Debug(string format, params object[] args)
    {
        log.Debug(format, args);
    }
    public static void Info(string format, params object[] args)
    {
        log.Info(format, args);
    }
    public static void Warn(string format, params object[] args)
    {
        log.Warn(format, args);
    }
    public static void Error(string format, params object[] args)
    {
        log.Error(format, args);
    }
}
