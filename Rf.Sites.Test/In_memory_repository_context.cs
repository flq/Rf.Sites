using System.Collections.Specialized;
using FubuMVC.Core.Urls;
using Moq;
using Rf.Sites.Entities;
using Rf.Sites.Frame;
using Rf.Sites.Frame.SiteInfrastructure;
using Rf.Sites.Test.Frame;

namespace Rf.Sites.Test
{
    internal class In_memory_repository_context<T> where T : Entity
    {
        protected readonly InMemoryRepository<T> Repository = new InMemoryRepository<T>();
        protected readonly Mock<IUrlRegistry> Reg;
        protected SiteSettings SiteSettings;
        protected ServerVariables ServerVariables;

        public In_memory_repository_context()
        {
            Reg = new Mock<IUrlRegistry>();
            SiteSettings = new SiteSettings();
            ServerVariables = new ServerVariables(new NameValueCollection());
        }

        protected void ApplyData<D>() where D : IDataScenario<T>, new()
        {
            var d = new D();
            d.Accept(Repository);
        }
    }
}