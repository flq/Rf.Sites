using System;
using System.Collections;
using System.Collections.Generic;
using FubuMVC.Core.Urls;
using NHibernate;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.SiteInfrastructure;
using System.Linq;

namespace Rf.Sites.Features.Searching
{
    public class SearchOnTime : ISearchPlugin
    {
        private readonly ICache _cache;
        private readonly IUrlRegistry _urls;
        private readonly Func<ISession> _session;

        public SearchOnTime(ICache cache, IUrlRegistry urls, Func<ISession> session)
        {
            _cache = cache;
            _urls = urls;
            _session = session;
        }

        public IEnumerable<Link> LinksFor(string searchTerm)
        {
            if (TimesAreNotCached)
                CacheTimes();

            var results = 
                Nodes.Where(y => y.Year.StartsWith(searchTerm))
                .Select(n => new Link
                                 {
                                     link = _urls.UrlFor(new YearPaging { Year = int.Parse(n.Year) }),
                                     linktext = "Content from Year " + n.Year
                                 });

            return results;
        }

        private void CacheTimes()
        {
            const string query = @"select distinct year(c.Created) as year, 
                                                   month(c.Created) as month from Content as c 
                                                   order by year(c.Created) desc, month(c.Created) desc";
            var q = _session().CreateQuery(query);
            var l = (ArrayList)q.List();

            var nodes = l.OfType<object[]>()
                .Select(row => Tuple.Create(row[0].ToString(), row[1].ToString()))
                .GroupBy(ym => ym.Item1)
                .Select(group => new YearNode(group.Key, group))
                .ToList();

            _cache.Add("timetree", nodes);
        }

        private bool TimesAreNotCached
        {
            get { return !_cache.HasValue("timetree"); }
        }

        private IEnumerable<YearNode> Nodes { get { return _cache.Get<List<YearNode>>("timetree"); } }

        private class YearNode
        {
            public YearNode(string year, IEnumerable<Tuple<string,string>> months)
            {
                Year = year;
                Months = new HashSet<string>(months.Select(m => m.Item2));
            }

            public string Year { get; private set; }
            public HashSet<string> Months { get; private set; }
        }
    }
}