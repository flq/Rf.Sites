using Rf.Sites.Domain;

namespace Rf.Sites.Tests.DataScenarios
{
  public class TagAndContent : AbstractDataScenario
  {
    public Tag Tag { get; private set; }
    public Content Content { get; private set; }

    protected override void inflateScenario(NHibernate.ISession session)
    {
      Tag = entityMaker.CreateTag();
      Content = entityMaker.CreateContent();
      Content.AssociateWithTag(Tag);
      session.Save(Tag);
      session.Save(Content);
    }
  }
}