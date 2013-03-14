using System.Collections.Generic;
using System.Text;
using DropNet.Models;

namespace Rf.Sites.Frame.DropboxSupport
{
    public interface IDropboxFacade
    {
        IList<MarkdownFile> GetAllUnpublished();
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
    }
}