using System;
using System.Text;
using log4net;
using ILog=Rf.Sites.Domain.Frame.ILog;
using ILog4Net = log4net.ILog;

namespace Rf.Sites.Frame
{
  public class Log4NetLogger : ILog
  {
    private static readonly ILog4Net logger = LogManager.GetLogger("Default");

    public void Info(string info, params object[] args)
    {
      logger.InfoFormat(info, args);
    }

    public void Debug(string info, params object[] args)
    {
      logger.DebugFormat(info, args);
    }

    public void Warning(string info, params object[] args)
    {
      logger.WarnFormat(info, args);
    }

    public void Error(string info, params object[] args)
    {
      logger.ErrorFormat(info, args);
    }

    public void Error(Exception x)
    {
      x = findDeepest(x);
      var s = string.Format("{0}: {1} - {2}", x.GetType().Name, x.Message, x.StackTrace);
      logger.Error(s.Substring(0, 3999));
    }

    private static Exception findDeepest(Exception x)
    {
      return x.InnerException != null ? findDeepest(x.InnerException) : x;
    }
  }
}