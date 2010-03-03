using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rf.Sites.Actions.Recent
{
  public class CommentList : IEnumerable<CommentFragment>
  {
    private readonly IEnumerable data;
    public Uri BaseURL { get; set; }

    public CommentList(IEnumerable data, Uri baseUrl)
    {
      this.data = data;
      BaseURL = baseUrl;
    }

    public IEnumerator<CommentFragment> GetEnumerator()
    {
      return data
        .OfType<object[]>()
        .Select(o => new CommentFragment(o[1].ToString(), (int)o[0], (DateTime) o[2], o[3].ToString()))
        .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}