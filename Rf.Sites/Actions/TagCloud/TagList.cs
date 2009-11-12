using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rf.Sites.Actions.TagCloud
{
  public class TagList : IEnumerable<WeightedTag>
  {
    readonly List<WeightedTag> tags = new List<WeightedTag>();
    private double segmentSize;

    public void Add(WeightedTag tag)
    {
      tags.Add(tag);
    }

    public IEnumerator<WeightedTag> GetEnumerator()
    {
      return tags.OrderBy(wt => wt.Text).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Segment(int numberOfSegments)
    {
      if (tags.Count == 0) return;
      int min, max;
      findMinAndMax(out min, out max);
      // +1 due to having the tagged amounts numbers inclusive
      segmentSize = (max - min + 1)/(double)numberOfSegments;

      // Subtracting int renormalizes the list to 0 being the smallest.
      // that way a simple division should sort out in which segment they fall
      tags.ForEach(wt =>
                     {
                       var proposedSegment = (int) Math.Round((wt.TaggedAmount - (min - 1))/segmentSize);
                       if (proposedSegment < 1)
                         proposedSegment = 1;
                       if (proposedSegment > numberOfSegments)
                         proposedSegment = numberOfSegments;
                       wt.Segment = proposedSegment;
                     });
    }

    private void findMinAndMax(out int min, out int max)
    {

      min = int.MaxValue;
      max = 0;

      for (int i = 0; i < tags.Count; i++)
      {
        min = tags[i].TaggedAmount < min ? tags[i].TaggedAmount : min;
        max = tags[i].TaggedAmount > max ? tags[i].TaggedAmount : max;
      }
    }
  }
}