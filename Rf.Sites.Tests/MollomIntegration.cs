using System.IO;
using System.Xml;
using NMollom;
using NUnit.Framework;
using Rf.Sites.Actions.CommentPost;
using Rf.Sites.Domain;
using Rf.Sites.Frame;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class MollomIntegration
  {
    private string mPrivK;
    private string mPubK;

    [TestFixtureSetUp]
    public void Setup()
    {
      XmlDocument d = new XmlDocument();
      using (var fs = File.OpenRead(@"..\..\..\Rf.Sites\web.config"))
        d.Load(fs);

      mPrivK = d.SelectSingleNode("/configuration/Environment/MollomPrivateKey").InnerText;
      mPubK = d.SelectSingleNode("/configuration/Environment/MollomPublicKey").InnerText;

    }

    [Test,Ignore]
    public void TheProvidedKeysAreValid()
    {
      var m = new Mollom(mPubK, mPrivK);
      m.VerifyKey().ShouldBeTrue();
    }

    // The following tests make sense when your mollom site is in dev mode to ensure proper responses.
    // Place an ignore on them when done.

    [Test,Ignore]
    public void SpamCommentMarksCommentPreparationInvalid()
    {
      var p = getCmtPreparer("Blablabla Hello spam");
      var e = new RunCommentThroughMollom(new Environment { MollomPublicKey = mPubK, MollomPrivateKey = mPrivK});

      e.Inspect(p);

      p.IsValid.ShouldBeFalse();

    }

    [Test,Ignore]
    public void HamCommentLeavesCommentPreparationValid()
    {
      var p = getCmtPreparer("Blablabla Hello ham");
      var e = new RunCommentThroughMollom(new Environment { MollomPublicKey = mPubK, MollomPrivateKey = mPrivK });

      e.Inspect(p);

      p.IsValid.ShouldBeTrue();

    }

    private CommentUpdatePreparer getCmtPreparer(string body)
    {
      return new CommentUpdatePreparer
               {
                 Comment =
                   new Comment
                     {
                       CommenterEmail = "a@b.com",
                       CommenterName = "Frank",
                       CommenterWebsite = "http://www.ab.com",
                       Body = body
                     }
               };
    }
  }
}