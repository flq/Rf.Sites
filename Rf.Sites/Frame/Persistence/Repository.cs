using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using Rf.Sites.Entities;
using Expression = System.Linq.Expressions.Expression;

namespace Rf.Sites.Frame.Persistence
{
    /// <summary>
    /// Let's take Fabio's statement by heart and make our repository queryable and a collection
    /// </summary>
    /// <typeparam name="T">Any Entity</typeparam>
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly ISessionFactory sessionFactory;

        private ISession theSession()
        {
            return sessionFactory.GetCurrentSession();
        }

        public Repository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public T this[int id]
        {
            get
            {
                return theSession().Get<T>(id);
            }
            set
            {
                theSession().Update(value, id);
            }
        }

        public IList<T> ByCriteria(DetachedCriteria criteria)
        {
            return criteria.GetExecutableCriteria(theSession()).List<T>();
        }

        public void Transacted(Action<IRepository<T>> actionWithinTransaction)
        {
            using (var t = theSession().BeginTransaction())
            {
                try
                {
                    actionWithinTransaction(this);
                    t.Commit();
                }
                catch
                {
                    t.Rollback();
                    throw;
                }
            }
        }

        public Expression Expression
        {
            get { return theSession().Query<T>().Expression; }
        }

        public Type ElementType
        {
            get { return theSession().Query<T>().ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return theSession().Query<T>().Provider; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return theSession().Query<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Add(T item)
        {
            return (int)theSession().Save(item);
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public void Clear()
        {
            throw new InvalidOperationException("Repository does not allow truncate");
        }

        public virtual bool Contains(T item)
        {
            //LINQNH does not do contains. Implement whether the Id exists in DB
            return (from e in this
                    where e.Id == item.Id
                    select e.Id).Count() > 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new InvalidOperationException("An anachronistic remnant that is not implemented");
        }

        public bool Remove(T item)
        {
            theSession().Delete(item);
            return true;
        }

        public int Count
        {
            get { return theSession().Query<T>().Count(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}