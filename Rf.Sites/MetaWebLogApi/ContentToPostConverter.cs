using System;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using System.Linq;

namespace Rf.Sites.MetaWeblogApi
{
  public class ContentToPostConverter : IObjectConverter<Content,Post>
  {
    public Post Convert(Content content)
    {

      var categories = content.Tags != null ?
        content.Tags.Select(t => t.Name).ToArray() :
        new string[0];

      var post = new Post
                   {
                     dateCreated = content.Created,
                     description = content.Body,
                     title = content.Title,
                     postid = content.Id,
                     categories = categories
                   };
      return post;
    }
  }
}