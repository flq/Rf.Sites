using System;

namespace Rf.Sites.Models
{
  public class ContentFragmentViewModel
  {
    public ContentFragmentViewModel(int id, string title, DateTime created, string teaser)
    {
      Id = id;
      Title = title;
      Created = created;
      Teaser = teaser;
    }

    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Teaser { get; private set; }

    public string WrittenInTime
    {
      get { return Created.ToString("dd.MM.yyyy - HH:mm"); }
    }

    public DateTime Created { get; private set; }
  }
}