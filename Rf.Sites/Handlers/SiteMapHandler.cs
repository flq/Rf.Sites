using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;
using NHibernate;
using System.Linq;
using Rf.Sites.Actions;
using Rf.Sites.Frame;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites.Handlers
{
  [HandlerUrl(Url = "sitemap.site")]
  public class SiteMapHandler : AbstractHandler
  {
    private readonly ISessionFactory factory;
    private readonly string contentEntryUrl;

    public SiteMapHandler(Environment environment, ISessionFactory factory)
    {
      this.factory = factory;
      contentEntryUrl = new Uri(environment.ApplicationBaseUrl, FrameUtilities.RelativeUrlToAction<ContentEntryAction>()).ToString();
    }

    protected override void processRequest()
    {
      Context.ResponseContentType = "application/xml";

      using (XmlTextWriter writer = new XmlTextWriter(Context.ResponseOutput))
      using (var s = factory.OpenStatelessSession())
      {
        var q = s.CreateQuery(
          @"select cnt.Id, cnt.CommentCount, cnt.Created, max(cmt.Created) from 
            Content cnt left outer join cnt.Comments cmt
            group by cnt.Id, cnt.CommentCount, cnt.Created");
        writeStartDocument(writer);

        foreach (var row in q.List())
        {
          var url = new Url(contentEntryUrl, new Row(row));
          url.WriteTo(writer);
        }

        writer.WriteEndElement();
      }
    }

    private static void writeStartDocument(XmlTextWriter writer)
    {
      writer.WriteProcessingInstruction("xml", @"version=""1.0"" encoding=""UTF-8""");
      writer.WriteStartElement("urlset");
      writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
      writer.WriteAttributeString("xmlns:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");
    }

    private class Row
    {
      public readonly int Id;
      public readonly int CommentCount;
      public readonly DateTime LastUpdate;


      public Row(object row)
      {
        var columns = (object[]) row;
        Id = (int)columns[0];
        CommentCount = (int) columns[1];
        
        List<DateTime> dateTimes = new List<DateTime> {(DateTime) columns[2]};
        if (CommentCount > 0)
          dateTimes.Add((DateTime) columns[3]);
        LastUpdate = dateTimes.Max();
      }
    }

    private class Url
    {
      private readonly string baseURL;
      private readonly Row row;

      public Url(string baseUrl, Row row)
      {
        baseURL = baseUrl;
        this.row = row;
      }

      public void WriteTo(XmlWriter writer)
      {
        writer.WriteStartElement("url");
        writer.WriteElementString("loc", baseURL + "/" + row.Id);
        writer.WriteElementString("lastmod", row.LastUpdate.ToString("yyyy-MM-dd"));
        writer.WriteElementString("changefreq", getChangeFrequency());
        writer.WriteElementString("priority", getPriority());
        writer.WriteEndElement();
      }

      private string getPriority()
      {
        switch (row.CommentCount)
        {
          case 0: return "0.5";
          case 1:
          case 2: return "0.6";
          case 3:
          case 4:
          case 5: return "0.7";
          default: return "0.8";
        }
      }

      private string getChangeFrequency()
      {
        var difference = DateTime.Now - row.LastUpdate;
        if (difference <= TimeSpan.FromHours(1))
          return "hourly";
        if (difference <= TimeSpan.FromDays(1))
          return "daily";
        if (difference <= TimeSpan.FromDays(7))
          return "weekly";
        if (difference <= TimeSpan.FromDays(30))
          return "monthly";
        if (difference <= TimeSpan.FromDays(365))
          return "yearly";
        return "never";
      }
    }
  }
}