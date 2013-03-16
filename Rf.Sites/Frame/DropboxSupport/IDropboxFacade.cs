using System.Collections.Generic;
using System.IO;
using System.Text;
using DropNet.Models;

namespace Rf.Sites.Frame.DropboxSupport
{
    public interface IDropboxFacade
    {
        IList<MarkdownFile> GetAllUnpublished();

        void UpdatePublishState(IList<MarkdownFile> files);
    }

    public class MarkdownFile
    {
        private readonly MetaData _md;

        public MarkdownFile(MetaData md, byte[] getFile)
        {
            _md = md;
            RawContents = Encoding.Unicode.GetString(getFile);
        }

        public string Name { get { return _md.Name; } }

        public string RawContents { get; private set; }

        public string PublishedName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Name) +
                DropboxFacade.PublishedMarker +
                Path.GetExtension(Name);
            }
        }
    }
}