using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;

namespace Rf.Sites.Domain.Frame
{
  public interface IRepository<T> : ICollection<T>, IQueryable<T>
    where T : Entity
  {
    new int Add(T item);
    T this[int id] { get; }
    IList<T> ByCriteria(DetachedCriteria criteria);
    void Transacted(Action<IRepository<T>> actionWithinTransaction);

  }
}