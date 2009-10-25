using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rf.Sites.Actions;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Tests.Frame;
using StructureMap;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class TestingTheSiteInfrastructure
  {
    [Test]
    public void OneCanTokenizeAnActionClassName()
    {
      var result = "HelloThereBAction".PasCalCaseTokenization();
      Assert.That(result, Has.Length(4));
      Assert.AreEqual("Hello", result[0]);
      Assert.AreEqual("There", result[1]);
      Assert.AreEqual("B", result[2]);
      Assert.AreEqual("Action", result[3]);
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
      "34".SafeCast(1).ShouldBeEqualTo(34);
    }

    [Test]
    public void CurrentConfigResolvesActions()
    {
      ActionEnv env = new ActionEnv();
      var a1 = env.GetAction("contentindex");
      a1.ShouldBeOfType<ContentIndexAction>();
      var a2 = env.GetAction("Blablerg");
      a2.ShouldBeOfType<UnknownAction>();
    }

    [Test]
    public void ConfigEnsuresProvisionOfContainer()
    {
      ActionEnv env = new ActionEnv();
      var a = env.GetAction<ContentIndexAction>();
      ((AbstractAction)a).Container.ShouldNotBeNull();
    }

    [Test]
    public void ConfigEnsuresProvisionOfEnvironment()
    {
      ActionEnv env = new ActionEnv();
      var a = env.GetAction<ContentIndexAction>();
      ((AbstractAction)a).Environment.ShouldNotBeNull();
    }

    [Test]
    public void EnvEnsuresExistenceOfVmExtender()
    {
      ActionEnv env = new ActionEnv();
      var cnt = new Content { Body = "<code>jo</code>" };
      var vm = env.Container.With(cnt).GetInstance<ContentViewModel>();
      vm.Body.Contains("<pre ").ShouldBeTrue();
    }

    [Test]
    public void EnvObtainsTheCommentToVMConverter()
    {
      ActionEnv env = new ActionEnv();
      var oC = env.Container.GetInstance<IObjectConverter<Comment, CommentVM>>();
      oC.ShouldNotBeNull();
    }

    [Test]
    public void KnownCommentExtenderIsApplied()
    {
      ActionEnv env = new ActionEnv();
      var oC = env.Container.GetInstance<IObjectConverter<Comment, CommentVM>>();
      var vm = oC.Convert(new Comment {Body = "<code>jo</code>"});
      vm.Body.Contains("<pre ").ShouldBeTrue();
    }
  }
}