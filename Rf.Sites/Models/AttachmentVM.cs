using System;
using Rf.Sites.Domain;

namespace Rf.Sites.Models
{
  public class AttachmentVM
  {
    private readonly Attachment attachment;
    private const int megaByteInBytes = 1024*1024;

    public AttachmentVM(Attachment attachment)
    {
      this.attachment = attachment;
    }

    public string Name { get { return attachment.Name; } }
    public string Path { get { return attachment.Path; } }
    public string Size
    {
      get
      {
        int size = attachment.Size;
        double newSize = mByteThreshold() ? (double)size / megaByteInBytes : size / 1024.0;
        return Math.Round(newSize, 2).ToString().Replace(",", ".");
      }
    }

    public string SizeDimension
    {
      get
      {
        return mByteThreshold() ? "MB" : "KB";
      }
    }

    private bool mByteThreshold()
    {
      return attachment.Size > megaByteInBytes;
    }
  }
}