using System;
using Rf.Sites.Domain;
using Rf.Sites.Frame;

namespace Rf.Sites.Models.Extender
{
  public class CommentToVMConverter : IObjectConverter<Comment,CommentVM>
  {
    private readonly IVmExtender<CommentVM>[] extender;

    public CommentToVMConverter(IVmExtender<CommentVM>[] extender)
    {
      this.extender = extender;
    }

    public CommentVM Convert(Comment @in)
    {
      return new CommentVM(@in, extender);
    }
  }
}