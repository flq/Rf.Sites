using System;

namespace Rf.Sites.Features.Models
{
    public class ContentTeaserVM
    {
        public ContentTeaserVM(int id, string title, DateTime created, string teaser)
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
            get { return Created.ToString("dd.MM.yyyy"); }
        }

        public DateTime Created { get; private set; }
    }
}