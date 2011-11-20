using System;
using Rf.Sites.Entities;
using Rf.Sites.Frame;

namespace Rf.Sites.Features.Models
{
    public class NotYetPublishedVM
    {
        public NotYetPublishedVM(Content model)
        {
            Title = model.Title;
            Keywords = model.MetaKeyWords;
            SetTheTime(model.Created);
        }

        public string Title { get; private set; }
        public string Keywords { get; set; }
        public string WrittenInTime { get; private set; }
        public string TimeUntilPublish { get; private set; }

        private void SetTheTime(DateTime created)
        {
            var p = new PeriodOfTimeOutput(DateTime.UtcNow, created).ToString();
            TimeUntilPublish = p == "today" ? "a small while" :p;
            WrittenInTime = created.ToString(Constants.CommonDateFormat);
        }
    }
}