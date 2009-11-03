using System.Web.Mvc;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Models;
using Rf.Sites.Models.Extender;
using Spark;
using Spark.Web.Mvc;
using StructureMap;
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

      SelectConstructor(() => new SparkViewFactory());

      SetAllProperties(s=>s.OfType<IContainer>());
      SetAllProperties(s => s.OfType<Environment>());

      ForRequestedType<IController>()
        .TheDefaultIsConcreteType<ActionDispatcher>();
      ForRequestedType<IActionInvoker>()
        .TheDefaultIsConcreteType<UrlToClassActionInvoker>();

      ForSingletonOf<ICache>()
        .TheDefaultIsConcreteType<WebBasedCache>();

      ForRequestedType<IAction>()
        .MissingNamedInstanceIs.TheInstanceNamed("unknown");

      ForSingletonOf<Environment>().TheDefault.IsThis(Startup.Environment);

      ForRequestedType<IObjectConverter<Comment, CommentVM>>()
        .TheDefaultIsConcreteType<CommentToVMConverter>();
      ForRequestedType<IObjectConverter<Attachment, AttachmentVM>>()
        .TheDefaultIsConcreteType<AttachmentToVMConverter>();

      Scan(s =>
             {
               s.AssemblyContainingType<SiteRegistry>();
               s.AddAllTypesOf<IAction>()
                 .NameBy(t => t.Name.Replace("Action", "").ToLowerInvariant());
               s.AddAllTypesOf(typeof (IVmExtender<>));
             });
      Scan(s=>
             {
               s.AssemblyContainingType<Entity>();
               s.LookForRegistries();
             });
    }
  }
}