using System;
using System.Net;
using System.Web.Mvc;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites.Actions.CommentPost
{
  public class CommentUpdatePreparer
  {
    private readonly ILog log;

    public CommentUpdatePreparer()
    {
      IsValid = true;
      log = new NullLogger();
    }

    public CommentUpdatePreparer(ControllerContext ctx, 
                                 IValidator validator, 
                                 Environment environment,
                                 ILog log,
                                 IVmExtender<CommentUpdatePreparer>[] commentModelExtensions)
    {
      this.log = log;
      if (!environment.CommentingEnabled)
      {
        // Somebody trying to post a comment even though there is no form
        IsValid = false;
        return;
      }

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

      log.Info("Starting a Comment process with comment {0}", Comment);

      IsValid = validator != null ? validator.Validate(Comment) : true;
      var ip = r.ServerVariables["HTTP_X_FORWARDED_FOR"];
      if (string.IsNullOrEmpty(ip))
      {
        ip = r.ServerVariables["REMOTE_ADDR"];
      }
      IPAddress address;
      IPAddress.TryParse(ip, out address);
      CommenterIPAddress = address;
      commentModelExtensions.Apply(this);
    }

    public Comment Comment { get; set; }
    public IPAddress CommenterIPAddress { get; set; }
    public bool IsValid { get; set; }
    public int ContentId { get; set; }
  }
}