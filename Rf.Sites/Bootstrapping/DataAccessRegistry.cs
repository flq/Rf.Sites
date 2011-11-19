using NHibernate;
using Rf.Sites.Frame.Persistence;
using StructureMap.Configuration.DSL;

namespace Rf.Sites.Bootstrapping
{
    public class DataAccessRegistry : Registry
    {
        public DataAccessRegistry()
        {
            var maker = new SessionFactoryMaker();
            ForSingletonOf<ISessionFactory>().Use(maker.CreateFactory);

            For<ISession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());

            For<IStatelessSession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(ctx => ctx.GetInstance<ISessionFactory>().OpenStatelessSession());

            For(typeof(IRepository<>)).Use(typeof(Repository<>));
        }
    }
}