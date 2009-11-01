using System;
using System.Linq;
using System.Web.Mvc;
using Rf.Sites.Actions.Args;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;

namespace Rf.Sites.Actions
{
  public class ContentTagAction : AbstractPagedContent
  {
    private readonly ValueArgs args;
    private readonly IRepository<Content> repository;

    public ContentTagAction(ValueArgs args, IRepository<Content> repository) : base(args)
    {
      this.args = args;
      this.repository = repository;
    }

    protected override IQueryable<ContentFragmentViewModel> Model
    {
      get
      {
        return from c in repository
               where c.Tags.Any(t => t.Name == args.Value)
               orderby c.Created descending 
               select new ContentFragmentViewModel(c.Id, c.Title, c.Created, c.Teaser);
      }
    }

    protected override string PaginatorCacheKey
    {
      get { return "Tag." + args.Value; }
    }

    protected override string PageTitle
    {
      get { return args.Value; }
    }

    protected override Func<HtmlHelper, PageInfo, string> ProducePageLink
    {
      get
      {
        return (h, p) =>
          h.ActionLink<ContentTagAction>(p.Number.ToString(), ArgsFrom.Value(PageTitle, p.Number));
      }
    }
  }
}