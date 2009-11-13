using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using Moq;
using Rf.Sites.Frame;

namespace Rf.Sites.Tests.Frame
{
  public static class TestHelpExtensions
  {
    public static T GetModelFromAction<T>(this ActionResult actionResult)
    {
      return (T) ((ViewResultBase) actionResult).ViewData.Model;
    }

    public static string GetResponseWriterOutput(this IResponseWriter writer)
    {
      MemoryStream ms = new MemoryStream();
      TextWriter w = new StreamWriter(ms);
      writer.WriteTo(w);
      ms.Seek(0, SeekOrigin.Begin);
      return new StreamReader(ms).ReadToEnd();
    }

    public static SyndicationFeed GetAsSyndicationFeed(this string feed)
    {
      return SyndicationFeed.Load(XmlReader.Create(new StringReader(feed)));
    }

    public static ControllerCtxMock StartControllerContextMock()
    {
      // Courtesy of Phil Haack through stackoverflow
      var request = new Mock<HttpRequestBase>();
      request.Setup(r => r.HttpMethod).Returns("GET");
      var response = new Mock<HttpResponseBase>();
      var mockHttpContext = new Mock<HttpContextBase>();
      mockHttpContext.Setup(c => c.Request).Returns(request.Object);
      mockHttpContext.Setup(c => c.Response).Returns(response.Object);

      var routeValues = new RouteValueDictionary()
        {
          {"controller", "Home"},
          {"action", "Index"},
          {"val1", ""},
          {"val2", ""},
          {"val3", ""}
        };
      var routeHandler = new Mock<IRouteHandler>(MockBehavior.Strict);
      
      var data = new RouteData
                   {
                     Route = new Route("{controller}/{action}/{val1}/{val2}/{val3}", routeValues, routeHandler.Object)
                   };

      var controllerContext = new ControllerContext(mockHttpContext.Object
      , data, new Mock<ControllerBase>().Object);
      
      return new ControllerCtxMock(controllerContext, request, response, mockHttpContext);
    }

  }
}