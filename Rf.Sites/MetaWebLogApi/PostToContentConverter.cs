using System;
using System.Linq;
using System.Web;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;

namespace Rf.Sites.MetaWeblogApi
{
  public class PostToContentConverter : IObjectConverter<Post,Content>
  {
    private readonly IRepository<Tag> tagRepository;

    public PostToContentConverter(IRepository<Tag> tagRepository)
    {
      this.tagRepository = tagRepository;
    }

    public Content Convert(Post post)
    {
      var tags = post.categories;

      var usedTags = from t in tagRepository
        where tags.Contains(t.Name)
        select t;

      var c = new Content
                {
                  Title = HttpUtility.HtmlDecode(post.title), 
                  // Appears that WLW sends the publish date as UTC already.
                  Created = post.dateCreated != DateTime.MinValue ? 
                  post.dateCreated : DateTime.Now.ToUniversalTime()
                };

      c.SetBody(post.description);

      foreach (var t in usedTags)
        c.AssociateWithTag(t);

      return c;
    }
  }
}