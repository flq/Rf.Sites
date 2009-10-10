using System;
using System.Collections.Generic;

namespace Rf.Sites.Frame
{
  public class PeriodOfTimeOutput
  {
    private readonly DateTime thePast;
    private readonly DateTime reference;
    private DateTime pastToPresentCursor;
    private int yearCounter;
    private int monthCounter;
    private int weekCounter;
    private int dayCounter;

    public PeriodOfTimeOutput(DateTime timeInQuestion) : this(timeInQuestion, DateTime.Now)
    {
    }

    public PeriodOfTimeOutput(DateTime thePast, DateTime reference)
    {
      this.thePast = thePast;
      pastToPresentCursor = thePast;
      this.reference = reference;
    }

    public override string ToString()
    {
      advanceTheTimeCursor(
        () => new DateTime(pastToPresentCursor.Year + 1, thePast.Month, thePast.Day),
        () => yearCounter++
      );
      advanceTheTimeCursor(
        () => pastToPresentCursor + monthTimeToAdd(),
        () => monthCounter++
      );
      advanceTheTimeCursor(
        () => pastToPresentCursor + TimeSpan.FromDays(7),
        () => weekCounter++
      );
      advanceTheTimeCursor(
        () => pastToPresentCursor + TimeSpan.FromDays(1),
        () => dayCounter++
      );
      return createTheString();
    }

    private string createTheString()
    {
      var parts = new List<string>();

      if (yearCounter > 0)
        parts.Add(string.Format("{0} {1}", yearCounter, yearInflection(yearCounter)));
      if (monthCounter > 0)
        parts.Add(string.Format("{0} {1}", monthCounter, monthInflection(monthCounter)));
      if (weekCounter > 0)
        parts.Add(string.Format("{0} {1}", weekCounter, weekInflection(weekCounter)));
      if (dayCounter > 0)
        parts.Add(string.Format("{0} {1}", dayCounter, dayInflection(dayCounter)));
      return parts.Count > 0 ? string.Join(", ", parts.ToArray()) : "today";

    }


    private void advanceTheTimeCursor(Func<DateTime> nextTime, Action uponSuccessfulAdvancement)
    {
      loop:
        var t = nextTime();
        if (t > reference) return;
        uponSuccessfulAdvancement();
        pastToPresentCursor = t;
      goto loop;
    }

    private TimeSpan monthTimeToAdd()
    {
      foreach (var daysInMonth in new int[] { 31, 30, 28, 29 })
      {
        var theSpan = TimeSpan.FromDays(daysInMonth);
        if ((pastToPresentCursor + theSpan).Day == pastToPresentCursor.Day)
          return theSpan;
      }
      // can happen when past lies on the last day of a e.g. a 31 day month
      // return a reasonable amount of days
      return TimeSpan.FromDays(30); 
      
    }

    private static string yearInflection(int years)
    {
      return years > 1 ? "years" : "year";
    }

    private static string monthInflection(int months)
    {
      return months > 1 ? "months" : "month";
    }

    private static string dayInflection(int days)
    {
      return days > 1 ? "days" : "day";
    }

    private static string weekInflection(int weeks)
    {
      return weeks > 1 ? "weeks" : "week";
    }
  }
}