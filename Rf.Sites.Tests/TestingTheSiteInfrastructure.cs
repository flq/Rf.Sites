using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rf.Sites.Frame;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class TestingTheSiteInfrastructure
  {
    [Test]
    public void OneCanTokenizeAnActionClassName()
    {
      var result = "HelloThereAction".PasCalCaseTokenization();
      Assert.That(result, Has.Length(3));
      Assert.AreEqual("Hello", result[0]);
      Assert.AreEqual("There", result[1]);
      Assert.AreEqual("Action", result[2]);
    }

    [Test]
    public void SafeCastToIntReturnsProvidedDefaultOnInvalidStrings()
    {
      string s = null;
      s.SafeCast(1).ShouldBeEqualTo(1);
      s = "";
      s.SafeCast(2).ShouldBeEqualTo(2);
      s = "bla";
      s.SafeCast(3).ShouldBeEqualTo(3);
    }

    [Test]
    public void SafeCastWorksWithIntegers()
    {
      string s = "34";
      s.SafeCast(1).ShouldBeEqualTo(34);
    }

  }
}