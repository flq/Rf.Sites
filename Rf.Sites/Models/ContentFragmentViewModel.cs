using System;

namespace Rf.Sites.Models
{
  public class ContentFragmentViewModel
  {
    private readonly DateTime created;

    public ContentFragmentViewModel(int id, string title, DateTime created, string teaser)
    {
      Id = id;
      Title = title;
      this.created = created;
      Teaser = teaser;
    }

    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Teaser { get; private set; }

    public string WrittenInTime
    {
      get { return created.ToString("dd.MM.yyyy - hh:mm"); }
    }
  }
}