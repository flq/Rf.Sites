using System;

namespace Rf.Sites.Frame
{
  /// <summary>
  /// URL must end on ".site" to ensure that factory and handler fir together
  /// </summary>
  public class HandlerUrlAttribute : Attribute
  {
    public string Url { get; set; }
  }
}