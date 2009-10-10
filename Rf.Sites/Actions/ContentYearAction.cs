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
  public class ContentYearAction : AbstractPagedContent
  {
    private readonly YearArgs args;
    private readonly IRepository<Content> repository;

    public ContentYearAction(YearArgs args, IRepository<Content> repository) : base(args)
    {
      this.args = args;
      this.repository = repository;
    }

    protected override IQueryable<ContentFragmentViewModel> Model
    {
      get
      {
        var lower = new DateTime(args.Year, 1, 1);
        var upper = new DateTime(args.Year, 12, 31);
        return
          from c in repository
          where c.Created >= lower && c.Created <= upper
          orderby c.Created descending 
          select new ContentFragmentViewModel(c.Id, c.Title, c.Created, c.Teaser);
      }
    }

    protected override string PaginatorCacheKey
    {
      get { return "Year." + args.Year; }
    }

    protected override string PageTitle
    {
      get { return "Content of " + args.Year; }
    }

    protected override Func<HtmlHelper, PageInfo, string> ProducePageLink
    {
      get
      {
        return (h, p) =>
          h.ActionLink<ContentYearAction>(p.Number.ToString(), ArgsFrom.Year(args.Year, p.Number));
      }
    }
  }
}