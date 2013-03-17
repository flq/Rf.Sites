using System.Collections.Generic;

namespace Rf.Sites.Frame.CloudStorageSupport
{
    public interface ICloudStorageFacade
    {
        IList<MarkdownFile> GetAllUnpublished();

        void UpdatePublishState(IList<MarkdownFile> files);
    }
}