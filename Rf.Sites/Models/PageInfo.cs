namespace Rf.Sites.Models
{
  public class PageInfo
  {
    public int Number { get; private set; }
    public bool IsCurrent { get; private set; }
    public string Link { get; private set; }

    public PageInfo(int i, bool b)
    {
      Number = i;
      IsCurrent = b;
    }
  }
}