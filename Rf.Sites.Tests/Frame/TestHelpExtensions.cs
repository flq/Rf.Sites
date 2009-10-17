using System.IO;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;
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
  }
}