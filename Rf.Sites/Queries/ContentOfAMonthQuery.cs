using System;
using System.Linq;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;

namespace Rf.Sites.Queries
{
  public class ContentOfAMonthQuery : IDbQuery<Content,ContentFragmentViewModel>
  {
    private readonly DateTime lower;
    private readonly DateTime upper;

    public ContentOfAMonthQuery(DateTime lowerMonthLimit)
    {
      lower = lowerMonthLimit;
      upper = lower.AddMonths(1);
    }

    public IQueryable<ContentFragmentViewModel> Query(IRepository<Content> repository)
    {
      return from c in repository
             where c.Created >= lower && c.Created < upper && c.Created < DateTime.Now.ToUniversalTime()
             orderby c.Created descending
             select new ContentFragmentViewModel(c.Id, c.Title, c.Created, c.Teaser);
    }
  }
}