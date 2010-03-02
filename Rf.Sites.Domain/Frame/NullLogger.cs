using System;

namespace Rf.Sites.Domain.Frame
{
  public class NullLogger : ILog
  {
    public void Info(string info, params object[] args)
    {
      
    }

    public void Debug(string info, params object[] args)
    {
      
    }

    public void Warning(string info, params object[] args)
    {
      
    }

    public void Error(string info, params object[] args)
    {
      
    }

    public void Error(Exception x)
    {
      
    }
  }
}