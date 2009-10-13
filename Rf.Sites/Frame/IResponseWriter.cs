using System.IO;

namespace Rf.Sites.Frame
{
  /// <summary>
  /// General abstraction for delievered data that is written directly e.g. to the 
  /// HTTP response stream
  /// </summary>
  public interface IResponseWriter
  {
    string ContentType { get; }
    void WriteTo(Stream stream);
  }
}