using System.Collections.Specialized;
using FubuMVC.Core.Urls;
using Moq;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame;
using Rf.Sites.Frame.SiteInfrastructure;
using Rf.Sites.Test.Frame;

namespace Rf.Sites.Test
{
    internal class In_memory_repository_context<T> where T : Entity
    {
        protected readonly InMemoryRepository<T> Repository = new InMemoryRepository<T>();
        private readonly Mock<IUrlRegistry> _reg;

        public In_memory_repository_context()
        {
            _reg = new Mock<IUrlRegistry>();
        }

        protected void ApplyData<D>() where D : IDataScenario<T>, new()
        {
            var d = new D();
            d.Accept(Repository);
        }

        protected ContentVM GetContentVm(Content model)
        {
            var cVm = new ContentVM(model, new SiteSettings(), new ServerVariables(new NameValueCollection()), _reg.Object);
            return cVm;
        }
    }
}