using System.IO;

namespace Rf.Sites.Handlers
{
  public interface IHttpContext
  {
    string ResponseContentType { get; set; }
    TextWriter ResponseOutput { get; }
    int ResponseStatusCode { get; set; }
    void EndResponse();
  }

  public interface ISetMockHttpContext
  {
    void SetMockContext(IHttpContext context);
  }
}