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
    /// <summary>
    /// Used when there is no HTTP context available,
    /// e.g. in tests
    /// </summary>
    private const string fallbackAbsoluteRoot = "http://localhost";

    private static Environment environment;

    public Startup(IContainer cnt)
    {
      ControllerBuilder.Current
        .SetControllerFactory(cnt.GetInstance<IControllerFactory>());
      SparkEngineStarter.RegisterViewEngine(ViewEngines.Engines);

      var sparkViewFactory = ViewEngines.Engines.OfType<SparkViewFactory>().SingleOrDefault();
      usePrecompilation(sparkViewFactory);

      Paginator.SetPaginatorCountCache(cnt.GetInstance<ICache>());

      registerRoutes(RouteTable.Routes);
      environment = cnt.GetInstance<Environment>();
    }

    public static Environment Environment
    {
      get
      {
        return environment;
      }
    }

    public static void ConstructUrlRoot(HttpRequest request)
    {
      
      Environment.ApplicationBaseUrl = new Uri(string.Format("{0}://{1}", request.Url.Scheme, request.Url.Authority));
    }

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
        "Default",
        "{controller}/{action}/{val1}/{val2}/{val3}",
        new { controller = "Home", action = "Index", val1 = "", val2 = "", val3 = "" },
        new { controller = @"[^\.]*" }
        );
    }
  }
}