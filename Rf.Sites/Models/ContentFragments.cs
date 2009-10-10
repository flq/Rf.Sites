using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Rf.Sites.Models
{
  public class ContentFragments : List<ContentFragmentViewModel>
  {
    public ContentFragments(IEnumerable<ContentFragmentViewModel> items)
      : base(items)
    {
    }

    public string Title { get; set; }
    public int PageCount { get; set; }
    public int CurrentPage { get; set; }

    public Func<HtmlHelper, PageInfo, string> ProduceLinkToAPage { get; set; }

    public IEnumerable<PageInfo> Pages()
    {
      return Enumerable.Range(0, PageCount)
        .Select(p => new PageInfo(p, p == CurrentPage));
    }


  }
}