using System;
using System.Linq;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;

namespace Rf.Sites.Queries
{
  public class ContentOfTagQuery : IDbQuery<Content,ContentFragmentViewModel>
  {
    private readonly string tag;

    public ContentOfTagQuery(string tag)
    {
      this.tag = tag;
    }

    public IQueryable<ContentFragmentViewModel> Query(IRepository<Content> repository)
    {
      return from c in repository
             where c.Created < DateTime.Now.ToUniversalTime() && c.Tags.Any(t => t.Name == tag)
             orderby c.Created descending
             select new ContentFragmentViewModel(c.Id, c.Title, c.Created, c.Teaser);
    }
  }
}