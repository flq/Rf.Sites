using System;
using System.Linq;
using FubuMVC.Core;
using Rf.Sites.Frame.CloudStorageSupport;

namespace Rf.Sites.Features
{
    public class PublishEndpoint
    {
        private readonly Func<ICloudStorageFacade> _cloud;
        private readonly Func<StoreMarkdownFileToDb> _storer;

        public PublishEndpoint(Func<ICloudStorageFacade> cloud, Func<StoreMarkdownFileToDb> storer)
        {
            _cloud = cloud;
            _storer = storer;
        }

        [UrlPattern("trigger_publish")]
        public string Trigger()
        {
            int phase = 0;
            try
            {
                var cloud = _cloud();
                var files = cloud.GetAllUnpublished();

                phase = 1;

                if (!files.Any())
                    return "OK (1)";

                using (var db = _storer())
                {
                    db.Store(files);
                }

                phase = 2;

                cloud.UpdatePublishState(files.Where(f => f.HasBeenStoredLocally).ToList());

                return "OK";
            }
            catch (Exception x)
            {
                return string.Format("{0}:{1} ({2})", x.GetType().Name, x.Message, phase);
            }
        }
    }
}