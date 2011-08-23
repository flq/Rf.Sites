using System;
using Rf.Sites.Entities;

namespace Rf.Sites.Features.Models
{
    public class AttachmentVM
    {
        private const int MegaByteInBytes = 1024 * 1024;

        public AttachmentVM(Attachment attachment)
        {
            setUp(attachment);
        }

        private void setUp(Attachment attachment)
        {
            if (attachment == null) return;
            Name = attachment.Name;
            Path = attachment.Path;
            var size = attachment.Size;
            double newSize = mByteThreshold(size) ? (double)size / MegaByteInBytes : size / 1024.0;
            Size = Math.Round(newSize, 2).ToString().Replace(",", ".");
            SizeDimension = mByteThreshold(size) ? "MB" : "KB";
        }

        public string Name { get; private set; }
        
        private string _path;
        
        public string Path
        {
            get { return CombinePathWithDropzone(); }
            set { _path = value; }
        }
        public string Size { get; private set; }

        public string SizeDimension { get; private set; }

        public string DropZone { get; set; }

        private static bool mByteThreshold(int size)
        {
            return size > MegaByteInBytes;
        }

        private string CombinePathWithDropzone()
        {
            if (DropZone == null)
                return _path;

            if (!DropZone.StartsWith("/"))
                DropZone = "/" + DropZone;

            if (!_path.StartsWith("/"))
                _path = "/" + _path;

            return DropZone + _path;
        }
    }
}