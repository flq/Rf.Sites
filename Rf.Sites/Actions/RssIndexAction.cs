using System;
using System.Linq;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Queries;

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
      var items = new ContentInDescendingOrderQuery()
        .Query(repository)
        .Take(Environment.FeedItemsPerFeed);

      return new ContentFragments(items) { Title = "Latest Content"};
    }
  }
}