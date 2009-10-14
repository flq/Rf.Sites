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
  }
}