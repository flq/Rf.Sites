using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Rf.Sites.Actions;
using Rf.Sites.Actions.Args;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class TestingTheChronicsFeature
  {
    private readonly List<DateTime> dateTimes = new List<DateTime>
                                                  {
                                                    new DateTime(2005, 1, 1),
                                                    new DateTime(2005, 1, 20),
                                                    new DateTime(2005, 1, 15),
                                                    new DateTime(2005, 2, 1),
                                                    new DateTime(2005, 3, 1),
                                                    new DateTime(2006, 2, 10)
                                                  };

    private ActionEnv env;
    private Mock<ICache> cache;

    [TestFixtureSetUp]
    public void Setup()
    {
      env = new ActionEnv();

      var repository = new TestRepository<Content>();
      for (var i = 0; i < dateTimes.Count; i++)
        repository.Add(env.Maker.CreateContent(i, "A Title", dateTimes[i]));

      cache = new Mock<ICache>();
      cache.Setup(c => c.HasValue("chronics"))
        .Returns(false);
      cache.Setup(c => c.Add("chronics", It.IsAny<ChronicsTree>()));

      env.OverloadContainer(ce=>
                              {
                                ce.For<IRepository<Content>>().Use(repository);
                                ce.For<ICache>().Use(cache.Object);
                              });
    }

    
    [Test]
    public void ChronicsTreeReturnsYearNodes()
    {
      var t = new ChronicsTree(dateTimes);
      var l = t.Query("source");
      l.ShouldNotBeNull();
      l.ShouldHaveLength(2);
      l[0].ShouldBeOfType<ChronicsNode>();
      l[0].id.ShouldBeEqualTo("2006");
    }

    [Test]
    public void ChronicsReturnsMonthsDescending()
    {
      var t = new ChronicsTree(dateTimes);
      var l = t.Query("2005");
      
      l.ShouldHaveLength(3);
      l[0].ShouldBeOfType<ChronicsNode>();
      l[0].id.ShouldBeEqualTo("2005/3");
    }

    [Test]
    public void ChronicMonthsHaveNoChildren()
    {
      var t = new ChronicsTree(dateTimes);
      var l = t.Query("2005");

      l[0].hasChildren.ShouldBeFalse();
    }

    [Test]
    public void ChronicsTreeCondensesMonths()
    {
      var t = new ChronicsTree(dateTimes);
      var l = t.Query("2005");

      l.ShouldHaveLength(3);
      l[0].ShouldBeOfType<ChronicsNode>();
      l[2].count.ShouldBeEqualTo(3);
    }

    [Test]
    public void CacheWillBeUsedToStoreTheChronicsTree()
    {
      var a = env.GetAction<ChronicsIndexAction>();
      a.Execute();
      cache.VerifyAll();
    }

    [Test]
    public void ARequestWithoutQueryProducesNoData()
    {
      var a = (JsonResult)env.GetAction<ChronicsIndexAction>().Execute();
      a.Data.ShouldBeNull();
    }

    [Test]
    public void RetrievalOfYearsIsCorrectlyWired()
    {
      var a = env.GetAction<ChronicsIndexAction,ChronicsPostArgs>(new ChronicsPostArgs { RawRequestId  = "source" });
      var r = a.Execute();
      var d = (object[])((JsonResult)r).Data;
      d.ShouldNotBeNull();
      d.ShouldHaveLength(2);
      d[0].ShouldBeOfType<ChronicsNode>();
    }

    [Test]
    public void RetrievalOfMonthsIsCorrectlyWired()
    {
      var a = env.GetAction<ChronicsIndexAction, ChronicsPostArgs>(new ChronicsPostArgs { RawRequestId = "2005" });
      var r = a.Execute();
      var d = (object[])((JsonResult)r).Data;
      d.ShouldNotBeNull();
      d.ShouldHaveLength(3);
      d[0].ShouldBeOfType<ChronicsNode>();
    }

    [Test]
    public void YearTextIsCorrectlyGenerated()
    {
      var r = (JsonResult)env.GetAction<ChronicsIndexAction, ChronicsPostArgs>(new ChronicsPostArgs { RawRequestId = "source" })
        .Execute();
      r.Data.ShouldNotBeNull();
      var node = ((ChronicsNode[])r.Data)[0];
      node.text.ShouldNotBeNull();
      node.text.Contains(FrameUtilities.RelativeUrlToAction<ContentYearAction>()).ShouldBeTrue();
    }

    [Test]
    public void MonthTextIsCorrectlyGenerated()
    {
      var r = (JsonResult)env.GetAction<ChronicsIndexAction, ChronicsPostArgs>(new ChronicsPostArgs { RawRequestId = "2005" })
        .Execute();
      r.Data.ShouldNotBeNull();
      var node = ((ChronicsNode[])r.Data)[0];
      node.text.ShouldNotBeNull();
      node.text.Contains(FrameUtilities.RelativeUrlToAction<ContentMonthAction>()).ShouldBeTrue();

    }
  }
}