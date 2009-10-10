using NUnit.Framework;
using Rf.Sites.Domain;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class WhenContentIsUsed
  {
    [Test]
    public void AddingACommentUpdatesCommentCount()
    {
      var c = new Content();
      c.AddComment(new Comment());
      Assert.AreEqual(1, c.CommentCount);
    }
  }
}