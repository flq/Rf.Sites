using System;
using Rf.Sites.Domain;
using Rf.Sites.Frame;

namespace Rf.Sites.Models.Extender
{
  public class AttachmentToVMConverter : IObjectConverter<Attachment,AttachmentVM>
  {
    private readonly IVmExtender<AttachmentVM>[] extender;

    public AttachmentToVMConverter(IVmExtender<AttachmentVM>[] extender)
    {
      this.extender = extender;
    }

    public AttachmentVM Convert(Attachment @in)
    {
      return new AttachmentVM(@in, extender);
    }
  }
}