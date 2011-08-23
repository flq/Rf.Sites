﻿using System;
using System.Linq;
using Rf.Sites.Entities;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Test.Frame;
using NHibernate.Linq;

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

    internal class SomeFutureContent : IDataScenario<Content>
    {
        public void Accept(IRepository<Content> repository)
        {
            var em = new EntityMaker();
            repository.Add(em.CreateContent(1, "A title",DateTime.Now.AddDays(1)));
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