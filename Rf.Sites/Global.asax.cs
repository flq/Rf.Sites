using System;
using System.Web;
using Rf.Sites.Frame;
using StructureMap;

namespace Rf.Sites
{

  public class MvcApplication : HttpApplication
  {

    protected void Application_Start()
    {
      ObjectFactory.Initialize(i => i.AddRegistry<SiteRegistry>());
      ObjectFactory.GetInstance<Startup>();
    }

    void Application_BeginRequest(object source, EventArgs e)
    {
      HttpApplication app = (HttpApplication)source;
      HttpContext context = app.Context;
      // Attempt to peform first request initialization
      FirstRequestInitialization.Initialize(context);
    }

    static class FirstRequestInitialization
    {
      private static bool initializedAlready = false;
      private static readonly object @lock = new object();
      // Initialize only on the first request
      public static void Initialize(HttpContext context)
      {
        if (initializedAlready)
          return;
        lock (@lock)
        {
          if (initializedAlready)
            return;
          Startup.ConstructUrlRoot(context.Request);
          initializedAlready = true;
        }
      }
    }
  }
}