using System;

namespace Rf.Sites.Actions.Args
{
  public class NullArgs : ActionArgs
  {
    protected override object makeObject()
    {
      return new object();
    }
  }
}