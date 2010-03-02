using System;
using NMollom;
using Rf.Sites.Frame;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites.Actions.CommentPost
{
  [Order(2)]
  public class RunCommentThroughMollom : IVmExtender<CommentUpdatePreparer>
  {
    private readonly Environment env;

    public RunCommentThroughMollom(Environment env)
    {
      if (env.IsMollomInfoAvailable())
        this.env = env;
    }

    public void Inspect(CommentUpdatePreparer viewModel)
    {
      if (env == null)
        return;
      var c = viewModel.Comment;
      var m = new Mollom(env.MollomPublicKey, env.MollomPrivateKey);
      var assessment = m.CheckContent("", c.Body, c.CommenterName, c.CommenterEmail, c.CommenterWebsite, "", "", viewModel.CommenterIPAddress);
      if (assessment.IsSpam())
        viewModel.IsValid = false;
      if (assessment.IsUnsure())
        c.AwaitsModeration = true;
    }
  }
}