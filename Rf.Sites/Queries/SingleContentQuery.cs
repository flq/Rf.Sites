using System;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.Frame;

namespace Rf.Sites.Queries
{
  public class SingleContentQuery : IDbResolve<Content,Content>
  {
    private readonly int id;

    public SingleContentQuery(int id)
    {
      this.id = id;
    }

    public Content Query(IRepository<Content> repository)
    {
      return repository[id];
    }
  }
}