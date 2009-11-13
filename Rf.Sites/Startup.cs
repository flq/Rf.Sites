using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Rf.Sites.Domain.Frame;
using Spark.Web.Mvc;
using StructureMap;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites
{
  public class Startup
  {
    /// <summary>
    /// Used when there is no HTTP context available,
    /// e.g. in tests
    /// </summary>
    private const string fallbackAbsoluteRoot = "http://localhost";

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
        string url = fallbackAbsoluteRoot;
        if (HttpContext.Current != null && HttpContext.Current.Request != null)
        {
          // from stackoverflow
          var req = HttpContext.Current.Request;
          url = string.Format("{0}://{1}", req.Url.Scheme, req.Url.Authority);
        }

        return new Environment
                 {
                   AbsoluteBaseUrl = new Uri(url),
                   SiteTitle = "realfiction",

                   SiteMasterName = "Frank Quednau",
                   SiteMasterEmail = "fquednau@gmail.com",
                   SiteMasterWebPage = "http://frankquednau.de",
                   SiteMasterPassword = "pwd"/*"b264c030-1546-4a10-8b53-ca398a9de939"*/,
                   
                   CopyrightNotice = "All content hosted by this site is written by F Quednau. Reproduction only under consent",

                   DropZoneUrl = "files",
                   BaseDirectory = AppDomain.CurrentDomain.BaseDirectory,

                   FeedItemsPerFeed = 10,
                   ItemsPerPage = 5
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