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
        private readonly SyndicationFeed _feed;
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
            
            _feed = new SyndicationFeed(items)
            {
                Id = feedSetup.FeedId,
                Title = new TextSyndicationContent(feedSetup.Title, TextSyndicationContentKind.Plaintext),
                LastUpdatedTime = new DateTimeOffset(lastUpdate),
            };
            _feed.Authors.Add(new SyndicationPerson(null, feedSetup.SiteMasterName, null));
            _feed.Copyright = new TextSyndicationContent(feedSetup.CopyrightNotice);
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
                    formatter = _feed.GetAtom10Formatter(); break;
                default:
                    formatter = _feed.GetRss20Formatter(); break;
            }
            return formatter;
        }
    }
}