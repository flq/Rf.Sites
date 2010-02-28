using System;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Rf.Sites.Domain.Frame;
using Spark.Web.Mvc;
using StructureMap;
using Environment=Rf.Sites.Frame.Environment;
using System.Linq;

namespace Rf.Sites
{
  public class Startup
  {

    public Startup(IContainer cnt)
    {
      ControllerBuilder.Current
        .SetControllerFactory(cnt.GetInstance<IControllerFactory>());
      SparkEngineStarter.RegisterViewEngine(ViewEngines.Engines);

      var sparkViewFactory = ViewEngines.Engines.OfType<SparkViewFactory>().SingleOrDefault();
      usePrecompilation(sparkViewFactory);

      Paginator.SetPaginatorCountCache(cnt.GetInstance<ICache>());

      registerRoutes(RouteTable.Routes);
      Environment = cnt.GetInstance<Environment>();
    }

    public static Environment Environment { get; private set; }

    [Conditional("COMPILEDVIEWS")]
    private static void usePrecompilation(SparkViewFactory factory)
    {
      factory.Engine.LoadBatchCompilation(Assembly.Load("Rf.Sites.Views"));
    }

    private static void registerRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
      routes.IgnoreRoute("files/.*");

      routes.MapRoute(
        "Jump",
        "go/{val1}",
        new {controller = "Content", action = "Entry", val1 = "", val2 = "", val3 = ""},
        new {controller = @"[^\.]*"});


      routes.MapRoute(
        "Default",
        "{controller}/{action}/{val1}/{val2}/{val3}",
        new {controller = "Home", action = "Index", val1 = "", val2 = "", val3 = ""},
        new {controller = @"[^\.]*"}
        );
    }
  }
}