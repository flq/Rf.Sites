using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace Rf.Sites.Models
{
  // ReSharper disable InconsistentNaming
  /// <summary>
  /// Used for JSON transmission
  /// </summary>
  public class ChronicsNode
  {

    public string id;
    public string text;
    public bool expanded;
    public bool hasChildren;

    [ScriptIgnore] public int count = 1;

    public void Increment()
    {
      count++;
    }
  }
  // ReSharper restore InconsistentNaming
}