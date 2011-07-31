using NHibernate;
using NHibernate.Validator.Engine;
using Rf.Sites.Frame;
using StructureMap.Configuration.DSL;
using IValidator = Rf.Sites.Frame.IValidator;

namespace Rf.Sites.Bootstrapping
{
    public class DataAccessRegistry : Registry
    {
        public DataAccessRegistry()
        {
            var maker = new SessionFactoryMaker();
            ForSingletonOf<ISessionFactory>().Use(maker.CreateFactory);

            For<ValidatorEngine>().Use(maker.GetValidationEngine);
            For<IValidator>().Use<NHBasedValidator>();

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