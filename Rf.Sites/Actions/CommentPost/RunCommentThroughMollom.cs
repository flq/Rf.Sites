using System;
using NMollom;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites.Actions.CommentPost
{
  [Order(2)]
  public class RunCommentThroughMollom : IVmExtender<CommentUpdatePreparer>
  {
    private readonly ILog log;
    private readonly Environment env;

    public RunCommentThroughMollom(Environment env, ILog log)
    {
      this.log = log;
      if (env.IsMollomInfoAvailable())
        this.env = env;
    }

    public void Inspect(CommentUpdatePreparer viewModel)
    {
      if (env == null)
        return;
      var c = viewModel.Comment;
      var m = new Mollom(env.MollomPublicKey, env.MollomPrivateKey);
      log.Info("About to call mollom with comment from IP {0} with name {1}", viewModel.CommenterIPAddress, c.CommenterName);
      var assessment = m.CheckContent("", c.Body, c.CommenterName, c.CommenterEmail, c.CommenterWebsite, "", "", viewModel.CommenterIPAddress);
      log.Info("Got result: {0}, Ham:{1}, Spam:{2}, Unsure:{3}", assessment.Quality, assessment.IsHam(), assessment.IsSpam(), assessment.IsUnsure());
      if (assessment.IsSpam())
        viewModel.IsValid = false;
      if (assessment.IsUnsure())
        c.AwaitsModeration = true;
    }
  }
}