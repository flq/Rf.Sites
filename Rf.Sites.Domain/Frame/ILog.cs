using System;

namespace Rf.Sites.Domain.Frame
{
  public interface ILog
  {
    void Info(string info, params object[] args);
    void Debug(string info, params object[] args);
    void Warning(string info, params object[] args);
    void Error(string info, params object[] args);
    void Error(Exception x);
  }
}