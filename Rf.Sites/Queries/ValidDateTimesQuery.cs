using System;
using System.Linq;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;

namespace Rf.Sites.Queries
{
  public class ValidDateTimesQuery : IDbQuery<Content,DateTime>
  {
    public IQueryable<DateTime> Query(IRepository<Content> repository)
    {
      return from c in repository where c.Created < DateTime.Now.ToUniversalTime() select c.Created;
    }
  }
}