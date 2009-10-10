using NHibernate;
using NHibernate.Context;
using StructureMap;

namespace Rf.Sites.Domain.Frame
{
  public class WebNHSessionContext : ICurrentSessionContext
  {
    public WebNHSessionContext(ISessionFactory factory)
    {
      // We will let structuremap create and keep the session
    }

    public ISession CurrentSession()
    {
      return ObjectFactory.GetInstance<ISession>();
    }
  }
}