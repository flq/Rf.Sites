using Rf.Sites.Entities;
using Rf.Sites.Frame.Persistence;

namespace Rf.Sites.Test.Frame
{
    internal interface IDataScenario<T> where T : Entity
    {
        void Accept(IRepository<T> repository);
    }
}