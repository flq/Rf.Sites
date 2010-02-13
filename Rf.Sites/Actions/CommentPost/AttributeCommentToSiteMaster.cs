using System;
using Rf.Sites.Frame;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites.Actions.CommentPost
{
  [Order(1)]
  public class AttributeCommentToSiteMaster : IVmExtender<CommentUpdatePreparer>
  {
    private readonly Environment env;

    public AttributeCommentToSiteMaster(Environment env)
    {
      this.env = env;
    }

    public void Inspect(CommentUpdatePreparer commentInfo)
    {
      var c = commentInfo.Comment.Body;
      var passWordTag = "/" + env.SiteMasterPassword;

      if (string.IsNullOrEmpty(c) || !c.EndsWith(passWordTag))
        return;
      
      var comment = commentInfo.Comment;
      comment.IsFromSiteMaster = true;
      comment.CommenterEmail = env.SiteMasterEmail;
      comment.CommenterName = env.SiteMasterName;
      comment.CommenterWebsite = env.SiteMasterWebPage;
      comment.Body = c.Replace(passWordTag, "");
    }
  }
}