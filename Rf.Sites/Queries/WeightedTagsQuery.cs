using System;
using NHibernate;
using Rf.Sites.Frame;

namespace Rf.Sites.Queries
{
  public class WeightedTagsQuery : INHibernateQuery
  {
    private readonly string[] tagsToIgnore;

    public WeightedTagsQuery(string[] tagsToIgnore)
    {
      this.tagsToIgnore = tagsToIgnore;
    }

    public IQuery Query(IStatelessSession session)
    {
      var q = session.CreateQuery(
          @"select tag.Name, count(cnt.Id) from 
            Tag tag join tag.RelatedContent cnt
            where tag.Name not in (:taglist)
            group by tag.Name");
      q.SetParameterList("taglist", tagsToIgnore);
      return q;
    }
  }
}