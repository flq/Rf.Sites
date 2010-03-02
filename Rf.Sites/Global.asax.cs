using System;
using System.Web;
using log4net.Config;
using Rf.Sites.Frame;
using StructureMap;

namespace Rf.Sites
{

  public class MvcApplication : HttpApplication
  {
    static MvcApplication()
    {
      XmlConfigurator.Configure();

      ObjectFactory.Initialize(i => i.AddRegistry<SiteRegistry>());
      ObjectFactory.GetInstance<Startup>();
    }

    protected void Application_Start()
    {
      //ObjectFactory.Initialize(i => i.AddRegistry<SiteRegistry>());
      //ObjectFactory.GetInstance<Startup>();
    }
  }
}