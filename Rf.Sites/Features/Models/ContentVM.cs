using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Urls;
using MarkdownSharp;
using Rf.Sites.Entities;
using Rf.Sites.Frame;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Models
{
    public class ContentVM
    {
        private readonly SiteSettings _siteSettings;

        public ContentVM(
          Content content,
          SiteSettings siteSettings, 
          ServerVariables vars,
          IUrlRegistry registry)
        {
            _siteSettings = siteSettings;

            if (content == null) return;

            var url = registry != null && vars != null ? registry.BuildAbsoluteUrlTemplate(vars, r => r.UrlFor(new ContentId(content.Id))) : null;
            CommentData = new CommentDataVM(
                content.Id, 
                url,
                _siteSettings.DisqusSiteIdentifier, 
                _siteSettings.DisqusDeveloperMode,
                HtmlTags.JsonUtil.ToJson(content.Title));
            MapData(content);
        }

        public CommentDataVM CommentData { get; private set; }

        public string ContentId { get; private set; }

        public string Title { get; private set; }

        public string Keywords { get; private set; }

        public string WrittenInTime { get; private set; }

        public string WrittenInPeriod { get; private set; }

        public string Body { get; set; }

        public bool NeedsCodeHighlighting { get; set; }

        public int AttachmentCount { get; private set; }

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
            Body = content.IsMarkdown.HasValue && (bool)content.IsMarkdown ? 
                new Markdown().Transform(content.Body) :
                content.Body;

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
                var converter = new AttachmentConverter(_siteSettings);
                Attachments = content.Attachments.Select(converter.Convert);
            }
        }
    }

    public class CommentDataVM
    {
        public CommentDataVM(int id, string buildAbsoluteUrlTemplate, string disqusSiteIdentifier, string disqusDeveloperMode, string titleInJson)
        {
            Identifier = id.ToString();
            AbsoluteUrlToContent = buildAbsoluteUrlTemplate;
            DisqusSiteIdentifier = disqusSiteIdentifier;
            DisqusDeveloperMode = disqusDeveloperMode;
            Title = titleInJson;
        }

        public string Identifier { get; private set; }

        public string AbsoluteUrlToContent { get; private set; }

        public string DisqusSiteIdentifier { get; private set; }

        /// <summary>
        /// Required for local testing
        /// </summary>
        public string DisqusDeveloperMode { get; private set; }

        public string Title { get; private set; }
    }
}