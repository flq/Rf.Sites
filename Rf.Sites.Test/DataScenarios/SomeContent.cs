using System;
using System.Linq;
using Rf.Sites.Entities;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Test.Frame;
using NHibernate.Linq;
using Rf.Sites.Test.Support;

namespace Rf.Sites.Test.DataScenarios
{
    internal class SomeContent : IDataScenario<Content>
    {
        public void Accept(IRepository<Content> repository)
        {
            var em = new EntityMaker();
            repository.Add(em.CreateContent(1, "A title", new DateTime(2010,1,1)));
        }
    }

    internal class FunnyTitle : IDataScenario<Content>
    {
        public void Accept(IRepository<Content> repository)
        {
            var em = new EntityMaker();
            repository.Add(em.CreateContent(1, "Lord's best item is \" - da crazy shizzle's", new DateTime(2010, 1, 1)));
        }
    }

    internal class SomeFutureContent : IDataScenario<Content>
    {
        public void Accept(IRepository<Content> repository)
        {
            var em = new EntityMaker();
            repository.Add(em.CreateContent(1, "A title",DateTime.Now.AddDays(1)));
        }
    }

    internal class MarkdownContent : IDataScenario<Content>
    {
        public void Accept(IRepository<Content> repository)
        {
            var em = new EntityMaker();
            var content = em.CreateContent(1, "A title", DateTime.Now.AddDays(-1));
            content.SetBody("# Hello World" + Environment.NewLine);
            content.IsMarkdown = true;
            repository.Add(content);
        }
    }

    internal class MarkdownContentWithFSharpCode : IDataScenario<Content>
    {
        public void Accept(IRepository<Content> repository)
        {
            var em = new EntityMaker();
            var content = em.CreateContent(1, "A title", DateTime.Now.AddDays(-1));
            content.SetBody(DataMother.Markdown4());
            content.IsMarkdown = true;
            repository.Add(content);
        }
    }

    internal class Content_10 : IDataScenario<Content>
    {
        public void Accept(IRepository<Content> repository)
        {
            var em = new EntityMaker();
            foreach (var _ in Enumerable.Range(0,10))
                repository.Add(em.CreateContent());
        }
    }
}