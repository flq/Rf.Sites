using System.Web.Routing;

namespace Rf.Sites.Actions.Args
{
  public abstract class ActionArgs : IArgToRoute
  {
    public RouteValueDictionary ToDictionary()
    {
      var obj = makeObject();
      return new RouteValueDictionary(obj);
    }

    protected abstract object makeObject();
    protected object makeObject(object value1)
    {
      return new { val1 = value1 };
    }

    protected object makeObject(object value1, object value2)
    {
      return new { val1 = value1, val2 = value2 };
    }

    protected object makeObject(object value1, object value2, object value3)
    {
      return new {val1 = value1, val2 = value2, val3 = value3};
    }
  }
}