using System;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Models
{
    public class FeedModel : IStreamOutput
    {
        private readonly SyndicationFeed feed;
        private readonly FeedType _feedTypeToGenerate;

        public FeedModel(FeedSetup feedSetup)
        {
            var items = feedSetup.Content
              .Select(vm =>
                new SyndicationItem(vm.Title, vm.Teaser, new Uri(feedSetup.UrlTemplate.Replace("{Id}", vm.Id.ToString())))
                {
                    Id = vm.Id.ToString(),
                    LastUpdatedTime = new DateTimeOffset(vm.Created)
                });

            var lastUpdate = feedSetup.Content.Max(vm => vm.Created);
            
            feed = new SyndicationFeed(items)
            {
                Id = feedSetup.FeedId,
                Title = new TextSyndicationContent(feedSetup.Title, TextSyndicationContentKind.Plaintext),
                LastUpdatedTime = new DateTimeOffset(lastUpdate),
            };
            feed.Authors.Add(new SyndicationPerson(null, feedSetup.SiteMasterName, null));
            feed.Copyright = new TextSyndicationContent(feedSetup.CopyrightNotice);
            _feedTypeToGenerate = feedSetup.FeedType;
        }

        public string MimeType
        {
            get { return "application/rss+xml"; }
        }

        public void Accept(TextWriter textWriter)
        {
            var s = new XmlWriterSettings { Indent = true };
            var formatter = GetFormatter();

            using (var xw = XmlWriter.Create(textWriter, s))
                formatter.WriteTo(xw);
        }

        private SyndicationFeedFormatter GetFormatter()
        {
            SyndicationFeedFormatter formatter;
            switch (_feedTypeToGenerate)
            {
                case FeedType.Atom:
                    formatter = feed.GetAtom10Formatter(); break;
                default:
                    formatter = feed.GetRss20Formatter(); break;
            }
            return formatter;
        }
    }
}