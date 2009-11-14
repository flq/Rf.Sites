using NUnit.Framework;
using Rf.Sites.Actions.TagCloud;
using System.Linq;
using Rf.Sites.Tests.DataScenarios;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class UsingTheTagCloudFeature
  {
    [Test]
    public void ATagListOutputsAlphabetically()
    {
      TagList tl = new TagList { new WeightedTag("C", 1), new WeightedTag("A", 2), new WeightedTag("B", 3) };

      var t2 = tl.ToList();
      t2[0].Text.ShouldBeEqualTo("A");
      t2[1].Text.ShouldBeEqualTo("B");
    }

    [Test]
    public void ASimpleTagListSegmentation()
    {
      TagList tl = new TagList
                     {
                       new WeightedTag("A", 9), new WeightedTag("B", 1), new WeightedTag("C", 6)
                     };
      tl.Segment(3);

      var l = tl.ToList();
      l[0].Segment.ShouldBeEqualTo(3);
      l[1].Segment.ShouldBeEqualTo(1);
      l[2].Segment.ShouldBeEqualTo(2);
    }

    [Test]
    public void AMoreComplexSegmentation()
    {
      TagList tl = new TagList
                     {
                       new WeightedTag("A", 17), new WeightedTag("B", 3), new WeightedTag("C", 12)
                     };
      tl.Segment(4);

      var l = tl.ToList();
      l[0].Segment.ShouldBeEqualTo(4);
      l[1].Segment.ShouldBeEqualTo(1);
      l[2].Segment.ShouldBeEqualTo(3);
    }

    [Test]
    public void TagcloudActionWorksAsIntended()
    {
      ActionEnv env = new ActionEnv();
      env.UseInMemoryDb();
      env.DataScenario<AFewTagsAndNumerousContent>();

      var a = new TagcloudIndexAction(env.FactoryForStatelessSession());
      env.GetAction(a);

      var tl = a.Execute().GetModelFromAction<TagList>();
      tl.ShouldNotBeNull();
      var wTags = tl.ToList();
      wTags.ShouldHaveCount(3);
      wTags[0].Segment.ShouldBeEqualTo(1);
      wTags[2].Segment.ShouldBeEqualTo(5);
    }
  }
}