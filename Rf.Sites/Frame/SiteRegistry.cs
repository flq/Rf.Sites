using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.MetaWeblogApi;
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
      //ForSingletonOf<Environment>().TheDefault.IsThis(Startup.Environment);
      ForSingletonOf<Environment>()
        .TheDefault.Is
        .ConstructedBy(() => (Environment) ConfigurationManager.GetSection("Environment"));
      

      Scan(s =>
      {
        s.AssemblyContainingType<Entity>();
        s.LookForRegistries();
      });

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

      ForRequestedType<IObjectConverter<Comment, CommentVM>>()
        .TheDefaultIsConcreteType<CommentToVMConverter>();
      ForRequestedType<IObjectConverter<Attachment, AttachmentVM>>()
        .TheDefaultIsConcreteType<AttachmentToVMConverter>();
      ForRequestedType<IObjectConverter<Post, Content>>()
        .TheDefaultIsConcreteType<PostToContentConverter>();
      ForRequestedType<IObjectConverter<Content, Post>>()
        .TheDefaultIsConcreteType<ContentToPostConverter>();
      ForRequestedType<IMediaStorage>()
        .TheDefaultIsConcreteType<MediaStorage>();

      ForRequestedType<IAction>()
        .MissingNamedInstanceIs.TheInstanceNamed("unknown");
      Scan(s =>
             {
               s.AssemblyContainingType<SiteRegistry>();
               s.AddAllTypesOf<IAction>()
                 .NameBy(t => t.Name.Replace("Action", "").ToLowerInvariant());
               s.AddAllTypesOf(typeof (IVmExtender<>));
             });

      ForRequestedType<IHttpHandler>()
        .MissingNamedInstanceIs.TheInstanceNamed("unknown");
      Scan(s=>
             {
               s.AssemblyContainingType<SiteRegistry>();
               s.With<HttpHandlerRegistrar>();
             });
      
    }

  }
}