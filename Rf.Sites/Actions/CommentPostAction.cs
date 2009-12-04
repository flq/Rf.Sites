using System;
using System.Web.Mvc;
using Rf.Sites.Actions.Args;
using Rf.Sites.Frame;

namespace Rf.Sites.Actions
{
  public class CommentPostAction : IAction
  {
    public CommentPostAction(CommentPostArgs args)
    {
      
    }

    public ActionResult Execute()
    {
      return new EmptyResult();
    }
  }
}