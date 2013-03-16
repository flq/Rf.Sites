using System;
using System.Threading.Tasks;
using Rf.Sites.Frame;
using Rf.Sites.Frame.DropboxSupport;
using WebBackgrounder;

namespace Rf.Sites.Bootstrapping
{
    public class CheckDropboxForNewPosts : IJob
    {
        private readonly Func<ICloudStorageFacade> _dropbox;

        public CheckDropboxForNewPosts(Func<ICloudStorageFacade> dropbox)
        {
            _dropbox = dropbox;
        }

        public Task Execute()
        {
            return new Task(() => {});
            //var unpublishedFiles = _dropbox().GetAllUnpublished();
        }

        public string Name { get { return "Check dropbox for new blog posts."; } }
        public TimeSpan Interval { get { return TimeSpan.FromHours(1); } }
        public TimeSpan Timeout { get { return TimeSpan.FromMinutes(5); } }
    }
}