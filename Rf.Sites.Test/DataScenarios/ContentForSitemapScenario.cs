using System;
using Rf.Sites.Entities;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Test.Frame;

namespace Rf.Sites.Test.DataScenarios
{
    public class ContentForSitemapScenario : IDataScenario<Content>
    {
        public void Accept(IRepository<Content> repository)
        {
            var entityMaker = new EntityMaker();
            var cnt = entityMaker.CreateContent("A");
            cnt.Created = DateTime.Now.Subtract(TimeSpan.FromDays(1));
            repository.Add(cnt);

            cnt = entityMaker.CreateContent("B");
            cnt.Created = DateTime.Now.Subtract(TimeSpan.FromDays(60));
            repository.Add(cnt);

            cnt = entityMaker.CreateContent("C");
            cnt.Created = new DateTime(2005, 5, 5);
            repository.Add(cnt);
        }
    }
}
