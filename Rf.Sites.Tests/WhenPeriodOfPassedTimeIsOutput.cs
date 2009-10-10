using System;
using NUnit.Framework;
using Rf.Sites.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class WhenPeriodOfPassedTimeIsOutput
  {
    private readonly DateTime basePast = new DateTime(2009, 9, 1);

    [Test]
    public void CorrectlyOutputsASmallPeriodWithinWeek()
    {
      var t2 = new DateTime(2009, 9, 6);
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(basePast, t2);
      Assert.AreEqual("5 days", o.ToString());
    }

    [Test]
    public void SmallestUnitIsToday()
    {
      var t2 = basePast;
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(basePast, t2);
      Assert.AreEqual("today", o.ToString());
    }

    [Test]
    public void WeeksAppearWhenMoreThanAWeek()
    {
      var t2 = new DateTime(2009, 9, 17);
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(basePast, t2);
      Assert.AreEqual("2 weeks, 2 days", o.ToString());
    }

    [Test]
    public void DaysAreCorrectlyInflected()
    {
      var t2 = new DateTime(2009, 9, 2);
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(basePast, t2);
      Assert.AreEqual("1 day", o.ToString());
    }

    [Test]
    public void WeeksAreCorrectlyInflected()
    {
      var t2 = new DateTime(2009, 9, 9);
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(basePast, t2);
      Assert.AreEqual("1 week, 1 day", o.ToString()); 
    }

    [Test]
    public void MonthsAreAlsoOutput()
    {
      var t2 = new DateTime(2009, 10, 1);
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(basePast, t2);
      Assert.AreEqual("1 month", o.ToString()); 
    }

    [Test]
    public void CombinationsOfMonthAndWeekAndDayArePossible()
    {
      var t2 = new DateTime(2009, 10, 10);
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(basePast, t2);
      Assert.AreEqual("1 month, 1 week, 2 days", o.ToString());
    }

    [Test]
    public void YearsAreAlsoOutput()
    {
      var t2 = new DateTime(2010, 9, 1);
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(basePast, t2);
      Assert.AreEqual("1 year", o.ToString());
    }

    [Test]
    public void CombinationOfYearMonthWeekAndDayIsPossible()
    {
      var t2 = new DateTime(2010, 11, 10);
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(basePast, t2);
      Assert.AreEqual("1 year, 2 months, 1 week, 2 days", o.ToString());
    }

    [Test]
    public void NextYearButLessThanAYear()
    {
      var t2 = new DateTime(2010, 7, 10);
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(basePast, t2);
      Assert.AreEqual("10 months, 1 week, 2 days", o.ToString());
    }

    [Test]
    public void NextMonthButLessThanAMonth() // Worked without changing code
    {
      var thePast = new DateTime(2009, 9, 10);
      var t2 = new DateTime(2009, 10, 2);
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(thePast, t2);
      Assert.AreEqual("3 weeks, 1 day", o.ToString());
    }

    [Test]
    public void NextYearButLessThanAYearVariant2()
    {
      var otherBase = new DateTime(2009, 9, 10);
      var t2 = new DateTime(2010, 6, 2);
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(otherBase, t2);
      Assert.AreEqual("8 months, 3 weeks, 2 days", o.ToString());
    }

    [Test]
    public void AVariantOfALongInterval()
    {
      var t2 = new DateTime(2012, 8, 30);
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(basePast, t2);
      Assert.AreEqual("2 years, 11 months, 4 weeks, 1 day", o.ToString());
    }

    [Test]
    public void IssueInFindingAMonthWhenPastDateIs31st()
    {
      var past = new DateTime(2009, 8, 31);
      var now = new DateTime(2009, 10, 1);
      PeriodOfTimeOutput o = new PeriodOfTimeOutput(past, now);
      Assert.AreEqual("1 month, 1 day", o.ToString());
    }
  }
}