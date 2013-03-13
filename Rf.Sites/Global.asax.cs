using System;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using Rf.Sites.Bootstrapping;

namespace Rf.Sites
{
    public class Global : System.Web.HttpApplication
    {
        private static FubuRuntime _runtime;

        protected void Application_Start(object sender, EventArgs e)
        {
            _runtime = FubuApplication.For<Bootstrapper>()
                .StructureMapObjectFactory(
                    ix =>
                        {
                            ix.AddRegistry<AppSettingProviderRegistry>();
                            ix.Scan(s =>
                                        {
                                            s.AssemblyContainingType<Global>();
                                            s.LookForRegistries();
                                        });
                        })
                .Bootstrap();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (_runtime != null)
                _runtime.Dispose();
        }
    }
}