using System;
using NUnit.Framework;
using Rf.Sites.Handlers;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class AssortedHandlerTests
  {
    [Test]
    public void BehaviorOfthe404Handler()
    {
      HandlerEnv env = new HandlerEnv();
      env.ExecuteHandler<Handler404>();
      env.UsedContext.ResponseStatusCode.ShouldBeEqualTo(404);
      env.UsedContext.EndResponseWasCalled.ShouldBeTrue();
    }
  }
}