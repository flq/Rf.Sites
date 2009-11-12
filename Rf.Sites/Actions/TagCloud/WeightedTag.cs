using System;

namespace Rf.Sites.Actions.TagCloud
{
  public class WeightedTag
  {
    private readonly string text;
    private readonly int taggedAmount;

    public WeightedTag(string text, int taggedAmount)
    {
      this.text = text;
      this.taggedAmount = taggedAmount;
    }

    public int TaggedAmount
    {
      get { return taggedAmount; }
    }

    public string Text
    {
      get { return text; }
    }

    public int Segment { get; set; }
  }
}