using System;

namespace Rf.Sites.Actions.Comments
{
  public class CommentFragment
  {
    public string CommenterName { get; private set; }
    public int JumpID { get; private set; }
    public DateTime TimeOfComment { get; private set; }

    public CommentFragment(string commenterName, int jumpID, DateTime timeOfComment)
    {
      CommenterName = commenterName;
      JumpID = jumpID;
      TimeOfComment = timeOfComment;
    }
  }
}