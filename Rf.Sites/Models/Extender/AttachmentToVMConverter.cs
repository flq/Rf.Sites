using System;
using Rf.Sites.Domain;
using Rf.Sites.Frame;

namespace Rf.Sites.Models.Extender
{
  public class AttachmentToVMConverter : IObjectConverter<Attachment,AttachmentVM>
  {
    public AttachmentVM Convert(Attachment @in)
    {
      return new AttachmentVM(@in);
    }
  }
}