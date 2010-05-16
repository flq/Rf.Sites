using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rf.Sites.Models;

namespace Rf.Sites.Actions.Recent
{
  public abstract class UrlBasedList<T> : IEnumerable<T>
  {
    public Uri BaseURL { get; set; }

    protected UrlBasedList(Uri baseUrl)
    {
      BaseURL = baseUrl;
    }

    public abstract IEnumerator<T> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }

  public class ContentFragmentList : UrlBasedList<ContentFragmentViewModel>
  {
    private readonly IEnumerable<ContentFragmentViewModel> list;

    public ContentFragmentList(IEnumerable<ContentFragmentViewModel> list, Uri baseUrl) : base(baseUrl)
    {
      this.list = list;
    }

    public override IEnumerator<ContentFragmentViewModel> GetEnumerator()
    {
      return list.GetEnumerator();
    }
  }

  public class CommentList : UrlBasedList<CommentFragment>
  {
    private readonly IEnumerable data;

    public CommentList(IEnumerable data, Uri baseUrl) : base(baseUrl)
    {
      this.data = data;
    }

    public override IEnumerator<CommentFragment> GetEnumerator()
    {
      return data
        .OfType<object[]>()
        .Select(o => new CommentFragment(o[1].ToString(), (int)o[0], (int)o[4], (DateTime)o[2], o[3].ToString()))
        .GetEnumerator();
    }
  }
}