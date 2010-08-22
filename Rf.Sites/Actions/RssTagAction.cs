using System.Linq;
using Rf.Sites.Actions.Args;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Queries;

namespace Rf.Sites.Actions
{
  public class RssTagAction : AbstractFeed
  {
    private readonly ValueArgs value;
    private readonly IRepository<Content> repository;

    public RssTagAction(ValueArgs value, IRepository<Content> repository)
    {
      this.value = value;
      this.repository = repository;
    }

    protected override ContentFragments produceFragments()
    {
      var tag = value.Value;
      var items = new ContentOfTagQuery(tag)
        .Query(repository)
        .Take(Environment.FeedItemsPerFeed);
      return new ContentFragments(items) { Title = "Latest Content in " + tag };
    }
  }
}