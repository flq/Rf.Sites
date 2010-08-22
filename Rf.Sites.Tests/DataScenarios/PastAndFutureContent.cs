using System;
using System.Collections.Generic;
using NHibernate;
using Rf.Sites.Domain;

namespace Rf.Sites.Tests.DataScenarios
{
  public class PastAndFutureContent : AbstractDataScenario
  {
    public List<Content> Contents { get; private set; }

    protected override void inflateScenario(ISession session)
    {
      Contents = new List<Content>();

      var cnt = entityMaker.CreateContent("A");
      cnt.Created = DateTime.Now.ToUniversalTime().Subtract(TimeSpan.FromHours(1));
      Contents.Add(cnt);

      cnt = entityMaker.CreateContent("B");
      cnt.Created = DateTime.Now.ToUniversalTime().Subtract(TimeSpan.FromHours(2));
      Contents.Add(cnt);

      cnt = entityMaker.CreateContent("C");
      cnt.Created = DateTime.Now.ToUniversalTime().ToUniversalTime().AddHours(2);
      Contents.Add(cnt);

      foreach (var c in Contents)
        session.Save(c);
    }
  }
}