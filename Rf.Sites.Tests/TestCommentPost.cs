using NUnit.Framework;
using Rf.Sites.Actions.CommentPost;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Tests.DataScenarios;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class TestCommentPost
  {
    private ContentWithComments scenario;
    private ActionEnv actionEnv;
    private IRepository<Content> repository;

    [TestFixtureSetUp]
    public void Setup()
    {
      actionEnv = new ActionEnv();
      actionEnv.UseInMemoryDb();
      scenario = actionEnv.DataScenario<ContentWithComments>();
      repository = actionEnv.Container.GetInstance<IRepository<Content>>();
    }

    [Test]
    public void InvalidCommentWillNotBeStored()
    {
      var oldCount = scenario.Contents[0].CommentCount;
      var a = actionEnv.GetAction<CommentPostAction,CommentUpdatePreparer>(new CommentUpdatePreparer() { IsValid = false });
      a.Execute();
      repository[scenario.Contents[0].Id].CommentCount.ShouldBeEqualTo(oldCount);
    }

    [Test]
    public void BrokenContentIdDoesNotThrow()
    {
      var a = actionEnv.GetAction<CommentPostAction, CommentUpdatePreparer>(
        new CommentUpdatePreparer
          {
            IsValid = true,
            Comment = actionEnv.Maker.CreateComment()
          });
      a.Execute();
    }

    [Test]
    public void ValidCommentWillBeStored()
    {
      var oldCount = scenario.Contents[0].CommentCount;
      var a = actionEnv.GetAction<CommentPostAction, CommentUpdatePreparer>(
        new CommentUpdatePreparer
        {
          ContentId = scenario.Contents[0].Id,
          IsValid = true,
          Comment = actionEnv.Maker.CreateComment()
        });
      a.Execute();
      repository[scenario.Contents[0].Id].CommentCount.ShouldBeEqualTo(oldCount + 1);
    }
  }
}