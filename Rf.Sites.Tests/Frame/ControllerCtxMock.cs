using System.Web;
using System.Web.Mvc;
using Moq;

namespace Rf.Sites.Tests.Frame
{
  public class ControllerCtxMock
  {
    public ControllerContext ControllerContext { get; private set; }
    public Mock<HttpRequestBase> RequestMock { get; private set; }
    public Mock<HttpContextBase> HttpCtxMock { get; private set; }
    public Mock<HttpResponseBase> ResponseMock { get; private set; }

    public ControllerCtxMock(
      ControllerContext controllerContext, 
      Mock<HttpRequestBase> requestMock, 
      Mock<HttpResponseBase> responseMock, 
      Mock<HttpContextBase> httpCtxMock)
    {
      ControllerContext = controllerContext;
      RequestMock = requestMock;
      HttpCtxMock = httpCtxMock;
      ResponseMock = responseMock;
    }
  }
}