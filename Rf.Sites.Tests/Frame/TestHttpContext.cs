using System;
using System.IO;
using System.Text;
using Rf.Sites.Handlers;

namespace Rf.Sites.Tests.Frame
{
  public class TestHttpContext : IHttpContext
  {
    private bool endResponseWasCalled;
    private StringWriter writer;

    public TestHttpContext()
    {
      writer = new StringWriter();
    }

    public string ResponseContentType { get; set; }
    public TextWriter ResponseOutput { get { return writer; } }
    public int ResponseStatusCode { get; set; }
    public void EndResponse()
    {
      endResponseWasCalled = true;
    }

    public bool EndResponseWasCalled
    {
      get { return endResponseWasCalled; }
    }

    public StringBuilder OutputOfHandler
    {
      get { return writer.GetStringBuilder(); }
    }
  }
}