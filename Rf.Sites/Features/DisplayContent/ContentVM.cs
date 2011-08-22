using System;
using System.Collections.Generic;
using System.Linq;
using Rf.Sites.Entities;
using Rf.Sites.Frame;

namespace Rf.Sites.Features.DisplayContent
{
    public class ContentVM
    {
        private readonly MediaSettings _settings;
        private bool commentingDisabled;

        public ContentVM(
          Content content,
          MediaSettings settings)
        {
            _settings = settings;
            if (content != null)
              MapData(content);
        }

        public string ContentId { get; private set; }

        public string Title { get; private set; }

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
            
            commentingDisabled = content.CommentingDisabled;
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
                var converter = new AttachmentConverter(_settings);
                Attachments = content.Attachments.Select(converter.Convert);
            }
        }
    }
}