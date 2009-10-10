using System;
using System.Linq;
using System.Web.Mvc;
using Rf.Sites.Actions.Args;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Models;

namespace Rf.Sites.Frame
{
  public abstract class AbstractPagedContent : AbstractAction
  {
    private readonly IPageArgs args;

    protected AbstractPagedContent(IPageArgs args)
    {
      this.args = args;
    }

    public override ActionResult Execute()
    {
      var p = Paginator.For(Model)
        .SetPageToLoad(args.Page)
        .SetCacheKey(PaginatorCacheKey)
        .Get();

      return createResult(
        new ContentFragments(p.GetPage())
          {
            Title = PageTitle,
            PageCount = p.PageCount,
            CurrentPage = p.Page,
            ProduceLinkToAPage = ProducePageLink
          }, Constants.ContentListViewName);
    }

    protected abstract IQueryable<ContentFragmentViewModel> Model { get; }
    protected abstract string PaginatorCacheKey { get; }
    protected abstract string PageTitle { get; }
    protected abstract Func<HtmlHelper, PageInfo, string> ProducePageLink { get; }
  }
}