using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using Rf.Sites.Entities;

namespace Rf.Sites.Frame.Persistence
{
  public interface IRepository<T> : ICollection<T>, IQueryable<T>
    where T : Entity
  {
    new int Add(T item);
    T this[int id] { get; set; }
    IList<T> ByCriteria(DetachedCriteria criteria);
    void Transacted(Action<IRepository<T>> actionWithinTransaction);

  }
}