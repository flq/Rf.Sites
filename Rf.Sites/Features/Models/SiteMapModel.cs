using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Rf.Sites.Entities;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Models
{
    public class SiteMapModel : IStreamOutput
    {
        private readonly string _urlTemplate;
        private readonly List<Url> _urls;

        public SiteMapModel(string urlTemplate, IQueryable<Content> contents)
        {
            _urlTemplate = urlTemplate;
            _urls = contents.Select(c => new Url { Id = c.Id, LastUpdate = c.Created }).ToList();
        }

        string IStreamOutput.MimeType
        {
            get { return "application/xml"; }
        }

        void IStreamOutput.Accept(TextWriter textWriter)
        {
            var s = new XmlWriterSettings
                        {
                            Indent = true, 
                            ConformanceLevel = ConformanceLevel.Document, 
                            NamespaceHandling = NamespaceHandling.OmitDuplicates
                        };
            using (var xw = XmlWriter.Create(textWriter, s))
            {
                writeStartDocument(xw);
                foreach (var u in _urls)
                    u.WriteTo(xw, _urlTemplate);
                xw.WriteEndElement();
            }
        }

        private static void writeStartDocument(XmlWriter writer)
        {
            writer.WriteProcessingInstruction("xml", @"version=""1.0"" encoding=""UTF-8""");
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
            writer.WriteAttributeString("xmlns", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");
        }

        private class Url
        {
            public int Id { get; set; }
            public DateTime LastUpdate { get; set; }

            
            public void WriteTo(XmlWriter writer, string urlTemplate)
            {
                writer.WriteStartElement("url");
                writer.WriteElementString("loc", urlTemplate.Replace("{Id}", Id.ToString()));
                writer.WriteElementString("lastmod", LastUpdate.ToString("yyyy-MM-dd"));
                writer.WriteElementString("changefreq", getChangeFrequency());
                writer.WriteElementString("priority", GetPriority());
                writer.WriteEndElement();
            }

            private string GetPriority()
            {
                return "0.8";
            }

            private string getChangeFrequency()
            {
                var difference = DateTime.Now - LastUpdate;
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