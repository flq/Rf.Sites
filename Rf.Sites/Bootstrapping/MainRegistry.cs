using Rf.Sites.Frame;
using Rf.Sites.Frame.SiteInfrastructure;
using StructureMap.Configuration.DSL;

namespace Rf.Sites.Bootstrapping
{
    public class MainRegistry : Registry
    {
        public MainRegistry()
        {
            ForSingletonOf<ICache>().Use<WebBasedCache>();
            Scan(s => { s.TheCallingAssembly(); s.ConnectImplementationsToTypesClosing(typeof(IObjectConverter<,>)); });
        }
    }
}