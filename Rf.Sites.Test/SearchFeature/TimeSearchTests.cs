using System;
using System.Linq;
using NUnit.Framework;
using Rf.Sites.Test.Frame;

namespace Rf.Sites.Test.SearchFeature
{
    [TestFixture]
    public class TimeSearchTests : SearchFeatureContext
    {
        [Test]
        public void GetsOneYear()
        {
            WithDates(new DateTime(2010, 10, 1));
            Search("20");
            SearchReturnedThisLink("Content from Year 2010", "/year/2010");
        }

        [Test]
        public void GetsTwoYears()
        {
            WithDates(new DateTime(2010, 1, 1), new DateTime(2011,1,1));
            Search("20");
            SearchReturnedThisLink("Content from Year 2010", "/year/2010");
            SearchReturnedThisLink("Content from Year 2011", "/year/2011");
        }

        [Test]
        public void NoResultWithNoMatch()
        {
            WithDates(new DateTime(1980, 10, 1));
            Search("20");
            SearchReturnedNothing();
        }

        [TearDown]
        public void Reset()
        {
            using (var tx = DBContext.Session.BeginTransaction())
            {
                DBContext.Session.CreateSQLQuery("DELETE from Content WHERE 1 = 1").UniqueResult();
                tx.Commit();
            }
            ClearCache();
        }

        private void WithDates(params DateTime[] dates)
        {
            var em = new EntityMaker();
            using (var tx = DBContext.Session.BeginTransaction())
            {
                foreach (var c in dates.Select(em.CreateContent))
                    DBContext.Session.Save(c);
                tx.Commit();
            }
            DBContext.Session.Clear();
        }
    }
}