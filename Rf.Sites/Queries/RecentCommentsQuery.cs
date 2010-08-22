using System;
using NHibernate;
using Rf.Sites.Frame;

namespace Rf.Sites.Queries
{
  public class RecentCommentsQuery : INHibernateQuery
  {
    public IQuery Query(IStatelessSession session)
    {
      var q = session.CreateQuery(
          @"select cnt.Id, c.CommenterName, c.Created, cnt.Title, c.Id from 
            Content cnt join cnt.Comments c
            where c.AwaitsModeration = false
            order by c.Created desc");
      q.SetMaxResults(10);
      return q;
    }
  }
}