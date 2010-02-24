using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Rf.Sites.Actions.CommentPost;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Tests.DataScenarios;
using Rf.Sites.Tests.Frame;
using Rf.Sites.Tests.Support;

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

    [Test]
    public void ApplyingMarkdownWorks()
    {
      var sw = Stopwatch.StartNew();
      var s = RunCommentThroughMarkdown.RunMarkdown(DataMother.Markdown1());
      sw.Stop();
      Console.WriteLine(sw.ElapsedMilliseconds);
      Console.WriteLine(s);
      sw.Reset(); sw.Start();
      s = RunCommentThroughMarkdown.RunMarkdown(DataMother.Markdown1());
      sw.Stop();
      Console.WriteLine(sw.ElapsedMilliseconds);
    }

    [Test]
    public void DoublePreAndCodeIsRemoved()
    {
      var model = new CommentUpdatePreparer { Comment = new Comment {Body = DataMother.Markdown2()} };
      new RunCommentThroughMarkdown().Inspect(model);
      model.Comment.Body.Contains("<pre><code>").ShouldBeFalse();
    }
  }
}