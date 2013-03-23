using System;
using System.Threading.Tasks;
using Rf.Sites.Frame;
using Rf.Sites.Frame.CloudStorageSupport;
using WebBackgrounder;
using System.Linq;

namespace Rf.Sites.Bootstrapping
{
    public class CheckCloudForNewPosts : IJob
    {
        private readonly Func<ICloudStorageFacade> _cloud;
        private readonly Func<StoreMarkdownFileToDb> _storer;
        

        public CheckCloudForNewPosts(Func<ICloudStorageFacade> cloud, Func<StoreMarkdownFileToDb> storer, SiteSettings settings)
        {
            _cloud = cloud;
            _storer = storer;
            Interval = settings.CloudPollInterval;
        }

        public Task Execute()
        {
            return new Task(() =>
            {
                var cloud = _cloud();
                var files = cloud.GetAllUnpublished();

                if (!files.Any())
                    return;

                using (var db = _storer())
                {
                    db.Store(files);
                }

                cloud.UpdatePublishState(files.Where(f => f.HasBeenStoredLocally).ToList());
            });
        }

        public string Name { get { return "Check cloud for new blog posts."; } }
        public TimeSpan Interval { get; private set; }
        public TimeSpan Timeout { get { return TimeSpan.FromMinutes(5); } }
    }
}