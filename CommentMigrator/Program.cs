using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Simple.Data;

namespace CommentMigrator
{
    class Program
    {

        static XNamespace content = "http://purl.org/rss/1.0/modules/content/";
        static XNamespace dsq = "http://www.disqus.com/";
        static XNamespace dc = "http://purl.org/dc/elements/1.1/";
        static XNamespace wp = "http://wordpress.org/export/1.0/";

        static void Main(string[] args)
        {
            var db = Database.Opener.OpenConnection(@"Server=.\SQLEXPRESS;Database=RfSite;Trusted_Connection=True;");
            
            XElement doc = GetRoot();

            dynamic currentContentId = 0;
            XElement item = null;

            var q = db.Comments.Query()
                .Join(db.Content).On(db.Comments.ContentId == db.Content.Id)
                .Select(
                   db.Comments.ContentId, 
                   db.Comments.CommenterName, 
                   db.Comments.CommenterEmail, 
                   db.Comments.CommenterWebsite,
                   db.Comments.IsFromSiteMaster,
                   db.Comments.Created,
                   db.Comments.Body,
                   db.Comments.Id, 
                   db.Content.Title,
                   db.Content.Created.As("ContentCreated")
                 )
                .OrderByContentId();

            foreach (var comment in q.ToList())
            {
                var fromSiteMaster = comment.IsFromSiteMaster;
                if (comment.ContentId != currentContentId)
                {
                    item = new XElement("item", new XElement("title", comment.Title), new XElement("link", "http://realfiction.net/go/" + comment.ContentId));
                    doc.Add(item);
                    currentContentId = comment.ContentId;
                }
                item.Add(
                  new XElement(content + "encoded", new XCData("")),
                  new XElement(dsq + "thread_identifier", comment.ContentId),
                  new XElement(wp + "post_date_gmt", comment.ContentCreated.ToString("yyyy-MM-dd HH:mm:ss")),
                  new XElement(wp + "comment_status", "open"),
                  new XElement(wp + "comment", 
                    new XElement(wp + "comment_id", comment.Id),
                    new XElement(wp + "comment_author", comment.CommenterName),
                    new XElement(wp + "comment_email", fromSiteMaster ? "fquednau@gmail.com" : comment.CommenterEmail),
                    new XElement(wp + "comment_author_url", comment.CommenterWebsite),
                    new XElement(wp + "comment_author_IP", ""),
                    new XElement(wp + "comment_date_gmt", comment.Created.ToString("yyyy-MM-dd HH:mm:ss")),
                    new XElement(wp + "comment_content", new XCData(comment.Body)),
                    new XElement(wp + "comment_approved", "1"),
                    new XElement(wp + "comment_parent", "")
                ));
            }

            using (var fs = File.Open("export.xml", FileMode.Create))
            using (var xw = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
            {
                doc.WriteTo(xw);
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static XElement GetRoot()
        {
            return new XElement("rss",
                                new XAttribute(XNamespace.Xmlns + "content", "http://purl.org/rss/1.0/modules/content/"),
                                new XAttribute(XNamespace.Xmlns + "dsq", "http://www.disqus.com/"),
                                new XAttribute(XNamespace.Xmlns + "dc", "http://purl.org/dc/elements/1.1/"),
                                new XAttribute(XNamespace.Xmlns + "wp", "http://wordpress.org/export/1.0/")
                );
        }
    }
}
