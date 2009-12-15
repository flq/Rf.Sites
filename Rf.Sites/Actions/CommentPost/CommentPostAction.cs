using System.Web.Mvc;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;

namespace Rf.Sites.Actions.CommentPost
{
  public class CommentPostAction : IAction
  {
    private readonly IRepository<Content> repository;
    private readonly int contentId;
    private readonly Comment comment;

    public CommentPostAction(CommentUpdatePreparer args, 
                             IRepository<Content> content)
    {
      repository = content;
      if (args.IsValid)
      {
        contentId = args.ContentId;
        comment = args.Comment;
      }
    }

    public ActionResult Execute()
    {
      if (comment == null) goto end;
      
      var c = repository[contentId];
      if (c == null) goto end;

      repository.Transacted(r=>
                              {
                                c.AddComment(comment);
                                r[c.Id] = c;
                              });
      
      end: return new EmptyResult();
    }
  }
}