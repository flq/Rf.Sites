using System;
using System.Linq;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;

namespace Rf.Sites.Queries
{
  public class FutureContentQuery : IDbQuery<Content,FutureContentViewModel>
  {
    public IQueryable<FutureContentViewModel> Query(IRepository<Content> repository)
    {
      return from r in repository
             where r.Created > DateTime.Now.ToUniversalTime()
             orderby r.Created
             select new FutureContentViewModel(r.Title, r.Created);
    }
  }
}