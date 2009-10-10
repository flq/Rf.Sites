using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using StructureMap;

namespace Rf.Sites
{

  public class MvcApplication : HttpApplication
  {

    protected void Application_Start()
    {
      ObjectFactory.Initialize(i =>
                                 {
                                   i.AddRegistry<SiteRegistry>();
                                   i.AddRegistry<DomainRegistry>();
                                 });
      ObjectFactory.GetInstance<Startup>();
    }
  }
}