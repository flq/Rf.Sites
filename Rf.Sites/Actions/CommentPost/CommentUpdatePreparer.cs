using System;
using System.Web.Mvc;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;

namespace Rf.Sites.Actions.CommentPost
{
  public class CommentUpdatePreparer
  {
    public CommentUpdatePreparer()
    {
      IsValid = true;
    }

    public CommentUpdatePreparer(ControllerContext ctx, 
                                 IValidator validator, 
                                 IVmExtender<CommentUpdatePreparer>[] commentModelExtensions)
    {
      var r = ctx.RequestContext.HttpContext.Request;
      ContentId = r.Form["id"].SafeCast(0);
      Comment = new Comment
                  {
                    CommenterName = r.Form["name"],
                    CommenterEmail = r.Form["email"],
                    Body = r.Form["comment_text"],
                    CommenterWebsite = r.Form["website"],
                    Created = DateTime.Now.ToUniversalTime()
                  };
      IsValid = validator != null ? validator.Validate(Comment) : true;
      commentModelExtensions.Apply(this);
    }

    public Comment Comment { get; set; }
    public bool IsValid { get; set; }
    public int ContentId { get; set; }
  }
}