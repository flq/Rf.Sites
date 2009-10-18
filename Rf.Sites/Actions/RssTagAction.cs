using System;
using System.Linq;
using Rf.Sites.Actions.Args;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;

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
      string tag = value.Value;
      var items = (from c in repository
                   where c.Created < DateTime.Now && c.Tags.Any(t=>t.Name == tag)
                   orderby c.Created descending
                   select new ContentFragmentViewModel(c.Id, c.Title, c.Created, c.Teaser))
                   .Take(Environment.FeedItemsPerFeed);
      return new ContentFragments(items) { Title = "Latest Content in " + tag };
    }
  }
}