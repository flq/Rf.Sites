using System;
using System.Web.Routing;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using Rf.Sites.Bootstrapping;
using StructureMap;

namespace Rf.Sites
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            FubuApplication.For<Bootstrapper>()
                .StructureMap(new Container())
                .Bootstrap(RouteTable.Routes);
        }
    }
}