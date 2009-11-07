using System;
using Rf.Sites.Domain;

namespace Rf.Sites.Tests
{
  public class EntityMaker
  {
    private int tagCount;
    private readonly Random r = new Random();

    public Content CreateContent()
    {
      var content = new Content
                      {
                        Title = "Title",
                        Published = true,
                        Created = DateTime.Now
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

    public Comment CreateComment()
    {
      return new Comment
               {
                 CommenterName = "Jones",
                 Created = DateTime.Now,
                 Body = "Hello"
               };
    }

    public Comment CreateComment(string name)
    {
      return new Comment
      {
        CommenterName = name,
        Created = DateTime.Now,
        Body = "Hello"
      };
    }
  }
}