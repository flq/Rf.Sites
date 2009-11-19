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
      this.years = (from d in dateTimes
                   let value = new YearMonth {Year = d.Year, Month = d.Month}
                   group value by value.Year into years
                   select years).ToDictionary(g=>g.Key.ToString(), g=>g.AsEnumerable());
    }

    public object[] Query(string query)
    {
      if (query == "root")
        return years.Keys
          .Select(s => new ChronicsNode {expanded = false, hasChildren = true, id = s, text = s})
          .ToArray();

      if (years.ContainsKey(query))
        return (from s in years[query]
               let t = s.Year.ToString() + "/" + s.Month
               select new ChronicsNode
                  {
                    expanded = false,
                    hasChildren = true,
                    id = t,
                    text = t
                  }).ToArray();
      
      return null;
    }

    private class YearMonth
    {
      public int Year { get; set; }
      public int Month { get; set; }
    }
  }
}