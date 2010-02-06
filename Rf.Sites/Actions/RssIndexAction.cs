using System;
using System.Linq;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;

namespace Rf.Sites.Actions
{
  public class RssIndexAction : AbstractFeed
  {
    private readonly IRepository<Content> repository;

    public RssIndexAction(IRepository<Content> repository)
    {
      this.repository = repository;
    }

    protected override ContentFragments produceFragments()
    {
      var items = (from c in repository
                   where c.Created < DateTime.Now.ToUniversalTime()
                   orderby c.Created descending
                   select new ContentFragmentViewModel(c.Id, c.Title, c.Created, c.Teaser))
                   .Take(Environment.FeedItemsPerFeed);

      return new ContentFragments(items) { Title = "Latest Content"};
    }
  }
}