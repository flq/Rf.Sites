using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Urls;
using Rf.Sites.Entities;
using Rf.Sites.Frame;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Models
{
    public class ContentVM
    {
        private readonly MediaSettings _mediaSettings;
        private readonly SiteSettings _siteSettings;

        public ContentVM(
          Content content,
          MediaSettings mediaSettings, 
          SiteSettings siteSettings, 
          ServerVariables vars,
          IUrlRegistry registry)
        {
            _mediaSettings = mediaSettings;
            _siteSettings = siteSettings;
            if (registry != null && vars != null)
              AbsoluteUrlToContent = registry.BuildAbsoluteUrlTemplate(vars, r => r.UrlFor(new ContentId(content.Id)));
            if (content != null)
              MapData(content);
        }

        public string AbsoluteUrlToContent { get; private set; }

        public string DisqusSiteIdentifier { get { return _siteSettings.DisqusSiteIdentifier; } }

        public string ContentId { get; private set; }

        public string Title { get; private set; }

        public string JsonTitle
        {
            get { return HtmlTags.JsonUtil.ToJson(Title); }
        }

        public string Keywords { get; private set; }

        public string WrittenInTime { get; private set; }

        public string WrittenInPeriod { get; private set; }

        public string Body { get; set; }

        public bool NeedsCodeHighlighting { get; set; }

        public int AttachmentCount { get; private set; }

        public int CommentCount { get; set; }

        public IEnumerable<string> Tags { get; private set; }

        public IEnumerable<AttachmentVM> Attachments { get; private set; }


        private void MapData(Content content)
        {
            MainData(content);
            SetTimeInfo(content);
            HandleAttachments(content);
            
            Tags = content.Tags != null ? content.Tags.Select(t => t.Name) : new string[] { };
        }

        private void MainData(Content content)
        {
            ContentId = content.Id.ToString();
            Title = content.Title;
            Body = content.Body;

            new CodeHighlightExtension().Inspect(this);

            Keywords = content.MetaKeyWords;
        }

        private void SetTimeInfo(Entity content)
        {
            WrittenInTime = content.Created.ToString(Constants.CommonDateFormat);
            var o = new PeriodOfTimeOutput(content.Created, DateTime.Now).ToString();
            WrittenInPeriod = string.Format("{0}{1}", o, o == "today" ? "" : " ago");
        }

        private void HandleAttachments(Content content)
        {
            AttachmentCount = content.AttachmentCount;

            if (AttachmentCount > 0)
            {
                var converter = new AttachmentConverter(_mediaSettings);
                Attachments = content.Attachments.Select(converter.Convert);
            }
        }
    }
}