using System;
using Rf.Sites.Actions;
using Rf.Sites.Frame;

namespace Rf.Sites.Models.Extender
{
  public class BuildChronicsNodeText : IVmExtender<ChronicsNode>
  {
    public void Inspect(ChronicsNode node)
    {
      var tokens = node.id.Split('/');
      switch (tokens.Length)
      {
        case 1:
          setTextToGoToYear(tokens, node);
          break;
        case 2:
          setTextToGoToMonth(tokens, node);
          break;
      }
    }

    private static void setTextToGoToMonth(string[] tokens, ChronicsNode node)
    {
      node.text = string.Format("<a href=\"{0}\">{1} ({2})</a>",
        FrameUtilities.RelativeUrlToAction<ContentMonthAction>(tokens[0], tokens[1]), monthAsWord(tokens[1]), node.count);
    }

    private static void setTextToGoToYear(string[] tokens, ChronicsNode node)
    {
      node.text = string.Format("<a href=\"{0}\">{1}</a>",
                                FrameUtilities.RelativeUrlToAction<ContentYearAction>(tokens[0]), tokens[0]);
    }

    private static string monthAsWord(string monthNum)
    {
      int month;
      int.TryParse(monthNum, out month);
      if (month == 0) return monthNum;
      var dt = new DateTime(2001, month, 1);
      return dt.ToString("MMMM");
    }
  }
}