using System;

namespace Rf.Sites.Actions.Recent
{
  public class CommentFragment
  {
    public string CommenterName { get; private set; }
    public int JumpID { get; private set; }
    public string ContentTitle { get; private set; }
    public DateTime TimeOfComment { get; private set; }

    public CommentFragment(
      string commenterName, 
      int jumpID, 
      DateTime timeOfComment,
      string contentTitle)
    {
      CommenterName = commenterName;
      JumpID = jumpID;
      TimeOfComment = timeOfComment;
      ContentTitle = contentTitle.Length > 20 ? contentTitle.Substring(0, 20) + "..." : contentTitle;
    }
  }
}