using System.Linq;
using NHibernate;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;

namespace Rf.Sites.Frame
{

  public interface IDbQuery<RTY,RES> where RTY : Entity
  {
    IQueryable<RES> Query(IRepository<RTY> repository);
  }

  public interface IDbResolve<RTY,RES> where RTY : Entity
  {
    RES Query(IRepository<RTY> repository);
  }

  public interface INHibernateQuery
  {
    IQuery Query(IStatelessSession session);
  }
}