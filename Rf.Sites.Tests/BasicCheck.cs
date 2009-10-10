using System.Diagnostics;
using NUnit.Framework;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class BasicCheck
  {
    [Test]
    public void Do()
    {
      var b = new DbTestBase();
      using (var s = b.Session)
      Debug.Write("OK");
    }
  }
}