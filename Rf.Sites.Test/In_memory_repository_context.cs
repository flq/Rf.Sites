using NUnit.Framework;
using Rf.Sites.Entities;
using Rf.Sites.Test.Frame;

namespace Rf.Sites.Test
{
    internal class In_memory_repository_context<T> where T : Entity
    {
        protected readonly InMemoryRepository<T> Repository = new InMemoryRepository<T>();

        protected void ApplyData<D>() where D : IDataScenario<T>, new()
        {
            var d = new D();
            d.Accept(Repository);
        }
    }
}