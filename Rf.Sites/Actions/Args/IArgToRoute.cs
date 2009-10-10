using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace Rf.Sites.Actions.Args
{
  public interface IArgToRoute
  {
    RouteValueDictionary ToDictionary();
  }
}