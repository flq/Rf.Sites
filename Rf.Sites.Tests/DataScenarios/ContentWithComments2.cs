using System;
using System.Collections.Generic;
using NHibernate;
using Rf.Sites.Domain;

namespace Rf.Sites.Tests.DataScenarios
{
  public class ContentWithComments2 : AbstractDataScenario
  {
    public List<Content> Contents { get; private set; }

    protected override void inflateScenario(ISession session)
    {
      Contents = new List<Content>();

      var cnt = entityMaker.CreateContent("B");
      cnt.AddComment(new Comment { CommenterName = "A", Created = DateTime.Now.Subtract(TimeSpan.FromDays(3)), Body = "Hello"});
      cnt.AddComment(new Comment { CommenterName = "B", Created = DateTime.Now.Subtract(TimeSpan.FromDays(2)), Body = "Hello", AwaitsModeration = true});
      cnt.AddComment(new Comment { CommenterName = "C", Created = DateTime.Now.Subtract(TimeSpan.FromDays(1)), Body = "Hello"});
      Contents.Add(cnt);

      foreach (var c in Contents)
        session.Save(c);
    }
  }
}