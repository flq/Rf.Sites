using System;
using System.Collections.Generic;
using System.Linq;

namespace Rf.Sites.Models
{
  public class ChronicsTree
  {

    private readonly Dictionary<string, IEnumerable<YearMonth>> years; 
      

    public ChronicsTree(IEnumerable<DateTime> dateTimes)
    {
      var r = from d in dateTimes
                   let value = new {d.Year, d.Month}
                   group value by value.Year into years
                   select new { 
                     Year = years.Key, 
                     Months = 
                     (from y in years 
                      group y by y.Month into months 
                      select new { months.Key, Count = months.Count()})
                   };
      this.years = r.ToDictionary(a => a.Year.ToString(),
                             a => a.Months
                               .Select(b => 
                                 new YearMonth {Year = a.Year, Month = b.Key, Count = b.Count})
                                 .AsEnumerable());
    }

    public ChronicsNode[] Query(string query)
    {
      if (string.IsNullOrEmpty(query))
        return null;

      if (query == "source")
        return years.Keys
          .Select(s => new ChronicsNode {expanded = false, hasChildren = true, id = s})
          .ToArray();

      if (years.ContainsKey(query))
        return (from s in years[query]
               let t = s.Year.ToString() + "/" + s.Month
               select new ChronicsNode
                  {
                    expanded = false,
                    hasChildren = false,
                    id = t,
                    count = s.Count
                  }).ToArray();
      
      return null;
    }

    private class YearMonth
    {
      public int Year { get; set; }
      public int Month { get; set; }
      public int Count { get; set; }
    }
  }
}