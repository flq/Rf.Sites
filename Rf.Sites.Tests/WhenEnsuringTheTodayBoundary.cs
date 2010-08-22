using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rf.Sites.Actions;
using Rf.Sites.Actions.Args;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Tests.DataScenarios;
using Rf.Sites.Tests.Frame;
using Environment = Rf.Sites.Frame.Environment;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class WhenEnsuringTheTodayBoundary
  {
    readonly ActionEnv actionEnv = new ActionEnv();

    [TearDown]
    public void TearDown()
    {
      actionEnv.ReleaseResources();
    }

    [Test]
    public void OnlyPastContentIsObtained()
    {
      actionEnv.UseInMemoryDb();
      actionEnv.DataScenario<PastAndFutureContent>();
      var action = new ContentIndexAction(new PageArgs(), 
        new Repository<Content>(actionEnv.InMemoryDB.Factory)) { Environment = new Environment { ItemsPerPage = 5 } };
      var model = action.Execute().GetModelFromAction<List<ContentFragmentViewModel>>();
      model.ShouldHaveCount(2);
    }

    [Test]
    public void OnlyPastContentInCurrentYear()
    {
      actionEnv.UseInMemoryDb();
      actionEnv.DataScenario<PastAndFutureContent>();
      var action = new ContentYearAction(new YearArgs { Page = 0, Year = 2010}, 
        new Repository<Content>(actionEnv.InMemoryDB.Factory)) { Environment = new Environment { ItemsPerPage = 5 } };
      var model = action.Execute().GetModelFromAction<List<ContentFragmentViewModel>>();
      model.ShouldHaveCount(2);
    }

    [Test]
    public void OnlyPastContentInCurrentMonth()
    {
      actionEnv.UseInMemoryDb();
      actionEnv.DataScenario<PastAndFutureContent>();
      var action = new ContentMonthAction(new MonthArgs { Page = 0, Year = DateTime.Now.Year, Month = DateTime.Now.Month },
        new Repository<Content>(actionEnv.InMemoryDB.Factory)) { Environment = new Environment { ItemsPerPage = 5 } };
      var model = action.Execute().GetModelFromAction<List<ContentFragmentViewModel>>();
      model.ShouldHaveCount(2);
    }
  }
}