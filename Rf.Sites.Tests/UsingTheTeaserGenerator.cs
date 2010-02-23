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
      TagRemover gen = new TagRemover(5);
      gen.Process("abc").ShouldBeEqualTo("abc");
    }

    [Test]
    public void LongerTextIsConcatenated()
    {
      TagRemover gen = new TagRemover(5);
      gen.Process("abcdefg").ShouldBeEqualTo("abcde");
    }

    [Test]
    public void TagIsStripped()
    {
      TagRemover gen = new TagRemover(8);
      gen.Process("<p>abcdefg").ShouldBeEqualTo(" abcdefg");
    }

    [Test]
    public void TagAndCloseIsStripped()
    {
      TagRemover gen = new TagRemover(19);
      gen.Process("<p>blablabla</p>blabla").ShouldBeEqualTo(" blablabla blabla");
    }

    [Test]
    public void TagWithAttribsIsStripped()
    {
      TagRemover gen = new TagRemover(31);
      gen.Process("<p class=\"joe\">blablabla</p>blabla").ShouldBeEqualTo(" blablabla blabla");
    }

    [Test]
    public void FunnyHtmlConstructsAreRemoved()
    {
      TagRemover gen = new TagRemover(16);
      gen.Process("blablabla<br>blabla").ShouldBeEqualTo("blablabla blabla");
    }

    [Test]
    public void EmptyAttributesDontConfuse()
    {
      TagRemover gen = new TagRemover(28);
      gen.Process("<p class=\"\">blablabla</p>blabla").ShouldBeEqualTo(" blablabla blabla");
    }
  }
}