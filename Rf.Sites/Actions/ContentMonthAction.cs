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
  public class ContentMonthAction: AbstractPagedContent
  {
    private readonly MonthArgs args;
    private readonly IRepository<Content> repository;

    public ContentMonthAction(MonthArgs args, IRepository<Content> repository) : base(args)
    {
      this.args = args;
      this.repository = repository;
    }

    protected override IQueryable<ContentFragmentViewModel> Model
    {
      get
      {
        var lower = new DateTime(args.Year, args.Month, 1);
        var upper = lower.AddMonths(1);
        return from c in repository
               where c.Created >= lower && c.Created < upper
               orderby c.Created descending
               select new ContentFragmentViewModel(c.Id, c.Title, c.Created, c.Teaser);
      }
    }

    protected override string PaginatorCacheKey
    {
      get { return string.Format("Month.{0}.{1}", args.Year, args.Month); }
    }

    protected override string PageTitle
    {
      get { return string.Format("Content of {0}.{1:0#}", args.Year, args.Month); }
    }

    protected override Func<HtmlHelper, PageInfo, string> ProducePageLink
    {
      get
      {
        return (h, p) =>
          h.ActionLink<ContentMonthAction>(p.Number.ToString(), ArgsFrom.Month(args.Year, args.Month, p.Number));
      }
    }
  }
}