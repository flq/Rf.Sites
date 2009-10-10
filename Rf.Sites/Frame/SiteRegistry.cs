using System.Web.Mvc;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Spark;
using Spark.Web.Mvc;
using StructureMap.Configuration.DSL;

namespace Rf.Sites.Frame
{
  public class SiteRegistry : Registry
  {
    public SiteRegistry()
    {
      ForRequestedType<IControllerFactory>()
        .TheDefaultIsConcreteType<ControllerFactory>();
      ForRequestedType<IViewEngine>()
        .TheDefaultIsConcreteType<SparkViewFactory>();
      ForRequestedType<ISparkSettings>()
        .TheDefault.Is.ConstructedBy(() => new SparkSettings()
                                             .AddNamespace("System.Linq")
                                             .AddNamespace("Rf.Sites.Frame")
                                             .AddNamespace("Rf.Sites.Actions"));

      //SelectConstructor(() => new SparkViewFactory());

      ForRequestedType<IController>()
        .TheDefaultIsConcreteType<ActionDispatcher>();
      ForRequestedType<IActionInvoker>()
        .TheDefaultIsConcreteType<UrlToClassActionInvoker>();

      ForSingletonOf<ICache>()
        .TheDefaultIsConcreteType<WebBasedCache>();

      ForRequestedType<IAction>()
        .MissingNamedInstanceIs.TheInstanceNamed("unknown");

      Scan(s =>
             {
               s.AssemblyContainingType<SiteRegistry>();
               s.AddAllTypesOf<IAction>()
                 .NameBy(t => t.Name.Replace("Action", "").ToLowerInvariant());
             });
      Scan(s=>
             {
               s.AssemblyContainingType<Entity>();
               s.LookForRegistries();
             });
    }
  }
}