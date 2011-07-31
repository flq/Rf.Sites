using NHibernate;
using NHibernate.Context;
using StructureMap;

namespace Rf.Sites.Frame
{
  public class WebNhSessionContext : ICurrentSessionContext
  {
    public WebNhSessionContext(ISessionFactory factory)
    {
      // We will let structuremap create and keep the session
    }

    public ISession CurrentSession()
    {
      return ObjectFactory.GetInstance<ISession>();
    }
  }
}