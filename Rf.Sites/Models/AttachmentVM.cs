using System;
using Rf.Sites.Domain;
using Rf.Sites.Frame;

namespace Rf.Sites.Models
{
  public class AttachmentVM
  {
    private const int megaByteInBytes = 1024*1024;

    public AttachmentVM(Attachment attachment, IVmExtender<AttachmentVM>[] extender)
    {
      setUp(attachment);
      extender.Apply(this);
    }

    private void setUp(Attachment attachment)
    {
      if (attachment == null) return;
      Name = attachment.Name;
      Path = attachment.Path;
      int size = attachment.Size;
      double newSize = mByteThreshold(size) ? (double)size / megaByteInBytes : size / 1024.0;
      Size = Math.Round(newSize, 2).ToString().Replace(",", ".");
      SizeDimension = mByteThreshold(size) ? "MB" : "KB";
    }

    public string Name { get; private set; }
    public string Path { get; set; }
    public string Size { get; private set; }

    public string SizeDimension { get; private set; }

    private static bool mByteThreshold(int size)
    {
      return size > megaByteInBytes;
    }
  }
}