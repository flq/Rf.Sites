using System;
using System.Linq;
using System.Web.Mvc;
using Rf.Sites.Actions.Args;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Queries;

namespace Rf.Sites.Actions
{
  public class ContentIndexAction : AbstractPagedContent
  {
    private readonly IRepository<Content> repository;

    public ContentIndexAction(PageArgs args, IRepository<Content> repository)
      : base(args)
    {
      this.repository = repository;
    }

    protected override IQueryable<ContentFragmentViewModel> Model
    {
      get
      {
        return new ContentInDescendingOrderQuery().Query(repository);
      }
    }

    protected override string PaginatorCacheKey
    {
      get { return "AllContent"; }
    }

    protected override string PageTitle
    {
      get { return "The full Log"; }
    }

    protected override Func<HtmlHelper, PageInfo, string> ProducePageLink
    {
      get { 
        return (h, p) => 
          h.ActionLink<ContentIndexAction>(p.Number.ToString(), ArgsFrom.Page(p.Number)); 
      }
    }
  }
}