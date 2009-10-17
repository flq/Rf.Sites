using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Rf.Sites.Domain.Frame;
using Spark.Web.Mvc;
using StructureMap;

namespace Rf.Sites.Frame
{
  public class Startup
  {
    public Startup(IContainer cnt)
    {
      ControllerBuilder.Current
        .SetControllerFactory(cnt.GetInstance<IControllerFactory>());
      SparkEngineStarter.RegisterViewEngine(ViewEngines.Engines);

      Paginator.SetPaginatorCountCache(cnt.GetInstance<ICache>());

      registerRoutes(RouteTable.Routes);
    }

    public static Environment Environment
    {
      get
      {
        // from stackoverflow
        // {0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, urlHelper.Content("~"));
        return new Environment
                 {
                   AbsoluteBaseUrl = new Uri("http://realfiction.net"), // this needs some thinking about
                   AuthorName = "Frank Quednau",
                   CopyrightNotice = "All content hosted by this site is written by F Quednau. Reproduction only under consent",
                   FeedItemsPerFeed = 10
                 };
      }
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