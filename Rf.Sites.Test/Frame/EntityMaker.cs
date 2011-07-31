using System;
using Rf.Sites.Entities;

namespace Rf.Sites.Test.Frame
{
  public class EntityMaker
  {
    private int tagCount;
    private readonly Random r = new Random();

    public Content CreateContent()
    {
      return CreateContent(0, "Title", DateTime.Now.AddDays(-1).ToUniversalTime());
    }

    public Content CreateContent(DateTime created)
    {
      return CreateContent(0, "Title", created);
    }

    public Content CreateContent(string title)
    {
      return CreateContent(0, title, DateTime.Now);
    }

    public Content CreateContent(int id, string title, DateTime created)
    {
      var content = new Content(id)
                      {
                        Title = title,
                        Published = true,
                        Created = created
                      };

      content.SetBody("Body");
      return content;
    }

    public Content CreateRandomContent()
    {
      return new Content
               {
                 Title = Guid.NewGuid().ToString(),
                 Body = Guid.NewGuid().ToString(),
                 Published = true,
                 Created = DateTime.Now - TimeSpan.FromDays(r.Next(1, 100))
               };
    }

    public Tag CreateTag()
    {
      return new Tag
               {
                 Created = DateTime.Now, 
                 Name = string.Format("Tag{0}", tagCount++)
               };
    }

    public void ResetTagCount()
    {
      tagCount = 0;
    }

  }
}