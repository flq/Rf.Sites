using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rf.Sites.Actions;
using Rf.Sites.Models;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class TestingTheChronicsFeature
  {
    [Test]
    public void ChronicsTreeReturnsYearNodes()
    {
      var lT = new List<DateTime>
                 {
                   new DateTime(2005,1,1),
                   new DateTime(2006,1,1)
                 };
      var t = new ChronicsTree(lT);
      var l = t.Query("root");
      l.ShouldNotBeNull();
      l.ShouldHaveLength(2);
      l[0].ShouldBeOfType<ChronicsNode>();
      ((ChronicsNode)l[0]).id.ShouldBeEqualTo("2005");
    }

    [Test]
    public void ChronicsReturnsMonths()
    {
      var lT = new List<DateTime>
                 {
                   new DateTime(2005,1,1),
                   new DateTime(2005,2,1),
                   new DateTime(2005,3,1),
                   new DateTime(2006,2,10)
                 };
      var t = new ChronicsTree(lT);
      var l = t.Query("2005");
      
      l.ShouldHaveLength(3);
      l[0].ShouldBeOfType<ChronicsNode>();
      ((ChronicsNode)l[0]).id.ShouldBeEqualTo("2005/1");
    }
    
  }
}