using System;
using FubuCore.Configuration;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Rf.Sites.Bootstrapping
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            Scan(s=> { s.TheCallingAssembly(); s.Convention<WireUpSettings>(); });
        }

        public class WireUpSettings : IRegistrationConvention
        {
            public void Process(Type type, Registry registry)
            {
                if (!type.IsAbstract && type.Name.EndsWith("Settings"))
                {
                    registry.For(type)
                        .LifecycleIs(InstanceScope.Singleton)
                        .Use(ctx => ctx.GetInstance<ISettingsProvider>().SettingsFor(type));
                }
            }
        }
    }
}