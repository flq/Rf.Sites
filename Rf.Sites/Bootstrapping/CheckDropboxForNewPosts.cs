using System;
using System.Threading.Tasks;
using Rf.Sites.Frame;
using WebBackgrounder;

namespace Rf.Sites.Bootstrapping
{
    public class CheckDropboxForNewPosts : IJob
    {
        private readonly Func<IDropboxFacade> _dropbox;

        public CheckDropboxForNewPosts(Func<IDropboxFacade> dropbox)
        {
            _dropbox = dropbox;
        }

        public Task Execute()
        {
            
        }

        public string Name { get { return "Check dropbox for new blog posts."; } }
        public TimeSpan Interval { get { return TimeSpan.FromHours(1); } }
        public TimeSpan Timeout { get { return TimeSpan.FromMinutes(5); } }
    }
}