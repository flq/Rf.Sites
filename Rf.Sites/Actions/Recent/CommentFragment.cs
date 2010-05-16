using System;

namespace Rf.Sites.Actions.Recent
{
  public class CommentFragment
  {
    private const int titleCutOff = 40;

    public string CommenterName { get; private set; }
    public int JumpID { get; private set; }
    public int CommentID { get; private set; }
    public string ContentTitle { get; private set; }
    public DateTime TimeOfComment { get; private set; }

    public CommentFragment(string commenterName, int jumpID, int commentID, DateTime timeOfComment, string contentTitle)
    {
      CommenterName = commenterName;
      JumpID = jumpID;
      CommentID = commentID;
      TimeOfComment = timeOfComment;
      ContentTitle = contentTitle.Length > titleCutOff ? contentTitle.Substring(0, titleCutOff) + "..." : contentTitle;
    }
  }
}