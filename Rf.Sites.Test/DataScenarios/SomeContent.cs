using System;
using Rf.Sites.Entities;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Test.Frame;

namespace Rf.Sites.Test.DataScenarios
{
    internal class SomeContent : IDataScenario<Content>
    {
        public void Accept(IRepository<Content> repository)
        {
            var em = new EntityMaker();
            repository.Add(em.CreateContent(1, "A title", DateTime.Now));
        }
    }
}