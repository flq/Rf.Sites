using System.Collections.Generic;
using NHibernate;
using Rf.Sites.Domain;

namespace Rf.Sites.Tests.DataScenarios
{
  public class AFewTagsAndNumerousContent : AbstractDataScenario
  {
    protected override void inflateScenario(ISession session)
    {
      var l = new List<Content>();

      for (int i = 0; i < 20; i++)
      {
        l.Add(entityMaker.CreateContent());
      }

      var tag1 = entityMaker.CreateTag();
      var tag2 = entityMaker.CreateTag();
      var tag3 = entityMaker.CreateTag();
      session.Save(tag1);
      session.Save(tag2);
      session.Save(tag3);

      for (int i = 0; i < 3; i++)
        l[i].AssociateWithTag(tag1);
      for (int i = 3; i < 9; i++)
        l[i].AssociateWithTag(tag2);
      for (int i = 9; i < 20; i++)
        l[i].AssociateWithTag(tag3);

      foreach (var c in l)
        session.Save(c);
    }
  }
}