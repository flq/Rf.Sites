using NUnit.Framework;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class UsingTheTeaserGenerator
  {
    [Test]
    public void ShortTextComesOutTheSame()
    {
      TeaserGenerator gen = new TeaserGenerator(5);
      gen.Process("abc").ShouldBeEqualTo("abc");
    }

    [Test]
    public void LongerTextIsConcatenated()
    {
      TeaserGenerator gen = new TeaserGenerator(5);
      gen.Process("abcdefg").ShouldBeEqualTo("abcde");
    }

    [Test]
    public void TagIsStripped()
    {
      TeaserGenerator gen = new TeaserGenerator(8);
      gen.Process("<p>abcdefg").ShouldBeEqualTo(" abcdefg");
    }

    [Test]
    public void TagAndCloseIsStripped()
    {
      TeaserGenerator gen = new TeaserGenerator(19);
      gen.Process("<p>blablabla</p>blabla").ShouldBeEqualTo(" blablabla blabla");
    }

    [Test]
    public void TagWithAttribsIsStripped()
    {
      TeaserGenerator gen = new TeaserGenerator(31);
      gen.Process("<p class=\"joe\">blablabla</p>blabla").ShouldBeEqualTo(" blablabla blabla");
    }

    [Test]
    public void FunnyHtmlConstructsAreRemoved()
    {
      TeaserGenerator gen = new TeaserGenerator(16);
      gen.Process("blablabla<br>blabla").ShouldBeEqualTo("blablabla blabla");
    }

    [Test]
    public void EmptyAttributesDontConfuse()
    {
      TeaserGenerator gen = new TeaserGenerator(28);
      gen.Process("<p class=\"\">blablabla</p>blabla").ShouldBeEqualTo(" blablabla blabla");
    }
  }
}