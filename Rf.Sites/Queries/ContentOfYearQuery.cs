using System;
using System.Linq;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;

namespace Rf.Sites.Queries
{
  public class ContentOfYearQuery : IDbQuery<Content, ContentFragmentViewModel>
  {
    private readonly DateTime lower;
    private readonly DateTime upper;

    public ContentOfYearQuery(int year)
    {
      lower = new DateTime(year, 1, 1);
      upper = new DateTime(year, 12, 31);
    }

    public IQueryable<ContentFragmentViewModel> Query(IRepository<Content> repository)
    {
      return from c in repository
          where c.Created >= lower && c.Created <= upper && c.Created < DateTime.Now.ToUniversalTime()
          orderby c.Created descending 
          select new ContentFragmentViewModel(c.Id, c.Title, c.Created, c.Teaser);
    }
  }
}