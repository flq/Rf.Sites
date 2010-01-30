using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.MetaWeblogApi;
using Rf.Sites.Models;
using Rf.Sites.Models.Extender;
using Spark.Web.Mvc;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Rf.Sites.Frame
{
  public class SiteRegistry : Registry
  {
    public SiteRegistry()
    {
      ForSingletonOf<Environment>()
        .Use((Environment) ConfigurationManager.GetSection("Environment") ?? new Environment());
      ForSingletonOf<DrupalUrlMap>()
        .Use((DrupalUrlMap)ConfigurationManager.GetSection("DrupalUrlMap") ?? new DrupalUrlMap());
        
      Scan(s =>
      {
        s.AssemblyContainingType<Entity>();
        s.LookForRegistries();
      });


      For<IControllerFactory>().Use<ControllerFactory>();
      For<IViewEngine>().Use<SparkViewFactory>();

      SelectConstructor(() => new SparkViewFactory());

      SetAllProperties(s=>s.OfType<IContainer>());
      SetAllProperties(s => s.OfType<Environment>());

      For<IController>().Use<ActionDispatcher>();
      For<IActionInvoker>().Use<UrlToClassActionInvoker>();

      ForSingletonOf<ICache>().Use<WebBasedCache>();

      For<IObjectConverter<Comment, CommentVM>>()
        .Use<CommentToVMConverter>();
      For<IObjectConverter<Attachment, AttachmentVM>>()
        .Use<AttachmentToVMConverter>();
      For<IObjectConverter<Post, Content>>()
        .Use<PostToContentConverter>();
      For<IObjectConverter<Content, Post>>()
        .Use<ContentToPostConverter>();
      For<IMediaStorage>()
        .Use<MediaStorage>();

      For<IAction>()
        .MissingNamedInstanceIs.TheInstanceNamed("unknown");
      Scan(s =>
             {
               s.AssemblyContainingType<SiteRegistry>();
               s.AddAllTypesOf<IAction>()
                 .NameBy(t => t.Name.Replace("Action", "").ToLowerInvariant());
               s.AddAllTypesOf(typeof (IVmExtender<>));
             });

      For<IHttpHandler>()
        .MissingNamedInstanceIs.TheInstanceNamed("unknown");
      Scan(s=>
             {
               s.AssemblyContainingType<SiteRegistry>();
               s.With<HttpHandlerRegistrar>();
             }); 
    }
  }
}