using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rf.Sites.Frame.CloudStorageSupport
{

    /// <summary>
    /// For some local testing
    /// </summary>
    public class FileSystemFacade : ICloudStorageFacade
    {
        private readonly string _path;

        public FileSystemFacade(FileSystemAsCloudSettings settings)
        {
            _path = settings.StoragePath;
        }

        public IList<MarkdownFile> GetAllUnpublished()
        {
            return Directory.GetFiles(_path, "*.md")
                .Where(s => s.IndexOf(DropboxFacade.PublishedMarker) == -1)
                .Select(s => new MarkdownFile(Path.GetFileName(s), File.ReadAllBytes(s)))
                .ToList();
        }

        public void UpdatePublishState(IList<MarkdownFile> files)
        {
            foreach (var f in files)
            {
                var p = Path.Combine(_path, f.Name);
                var fi = new FileInfo(p);
                fi.MoveTo(Path.Combine(_path, f.PublishedName));
            }
        }
    }

    public class FileSystemAsCloudSettings
    {
        public string StoragePath { get; set; }
    }
}