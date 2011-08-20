using Rf.Sites.Frame;
using StructureMap.Configuration.DSL;

namespace Rf.Sites.Bootstrapping
{
    public class MainRegistry : Registry
    {
        public MainRegistry()
        {
            Scan(s => { s.TheCallingAssembly(); s.ConnectImplementationsToTypesClosing(typeof(IObjectConverter<,>)); });
        }
    }
}