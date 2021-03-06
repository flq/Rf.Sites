using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using Rf.Sites.Entities;
using Rf.Sites.Frame.Persistence;
using Expression = System.Linq.Expressions.Expression;

namespace Rf.Sites.Test.Frame
{
    public class InMemoryRepository<T> : IRepository<T> where T : Entity
    {
        readonly List<T> list = new List<T>();

        public Expression Expression
        {
            get { return list.AsQueryable().Expression; }
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public IQueryProvider Provider
        {
            get
            {
                return list.AsQueryable().Provider;
            }
        }

        public int Add(T item)
        {
            list.Add(item);
            return list.IndexOf(item);
        }

        public T this[int id]
        {
            get { return list.Where(t => t.Id == id).SingleOrDefault(); }
            set
            {
                var idx = list.FindIndex(t => t.Id == id);
                if (idx >= 0)
                    list[idx] = value;
            }
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            return list.Remove(item);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IList<T> ByCriteria(DetachedCriteria criteria)
        {
            throw new NotImplementedException("Requires NHibernate based backend");
        }

        public void Transacted(Action<IRepository<T>> actionWithinTransaction)
        {
            // No transaction here...
            actionWithinTransaction(this);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<T>.Add(T item)
        {
            list.Add(item);
        }
    }
}