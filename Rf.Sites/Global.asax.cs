using System;
using System.Web.Routing;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using Rf.Sites.Bootstrapping;

namespace Rf.Sites
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            FubuApplication.For<Bootstrapper>()
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
    }
}