using System;
using NHibernate;
using Rf.Sites.Entities;
using Rf.Sites.Frame;

namespace Rf.Sites.Test.Frame
{
    public class DbTestContext : IDisposable
    {
        private readonly ISessionFactory factory;
        private readonly EntityMaker maker = new EntityMaker();

        public ISession Session
        {
            get { return factory.GetCurrentSession(); }
        }

        public ISessionFactory Factory
        {
            get { return factory; }
        }

        public EntityMaker Maker
        {
            get { return maker; }
        }

        public DbTestContext()
        {
            var factoryMaker = new SessionfactoryMakerForTest();
            factory = factoryMaker.CreateFactory();
            factoryMaker.DropAndRecreateSchema(Console.Out, Session.Connection);
        }

        public void Dispose()
        {
            Session.Dispose();
        }

        /// <summary>
        /// Returns id of last entity
        /// </summary>
        protected int SaveAndCommit(params object[] entities)
        {
            int id = 0;
            using (var t = Session.BeginTransaction())
            {
                foreach (var o in entities)
                  id = (int)Session.Save(o);
                t.Commit();
            }
            return id;
        }

        protected int SaveAndCommit(object entity)
        {
            int id = 0;
            using (var t = Session.BeginTransaction())
            {
                id = (int)Session.Save(entity);
                t.Commit();
            }
            return id;
        }

        protected IRepository<T> CreateRepository<T>() where T : Entity
        {
            return new Repository<T>(Factory);
        }
    }
}