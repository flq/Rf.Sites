using NHibernate;

namespace Rf.Sites.Tests.DataScenarios
{
  public abstract class AbstractDataScenario
  {
    protected readonly EntityMaker entityMaker = new EntityMaker();

    public void ExecuteScenario(ISession session)
    {
      using (var t = session.BeginTransaction())
      {
        inflateScenario(session);
        t.Commit();
      }
      session.Clear();
    }

    protected virtual void inflateScenario(ISession session)
    {
      
    }
  }
}