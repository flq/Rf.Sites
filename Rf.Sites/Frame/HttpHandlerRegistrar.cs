using System;
using System.Web;
using Rf.Sites.Domain.Frame;
using StructureMap.Graph;

namespace Rf.Sites.Frame
{
  public class HttpHandlerRegistrar : ITypeScanner
  {
    public void Process(Type type, PluginGraph graph)
    {
      if (type.ImplementsInterface<IHttpHandler>() && type.HasAttribute<HandlerUrlAttribute>())
        graph.AddType(typeof(IHttpHandler), type, type.GetAttribute<HandlerUrlAttribute>().Url);

    }
  }
}