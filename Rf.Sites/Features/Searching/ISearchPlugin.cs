using System.Collections.Generic;
using Rf.Sites.Features.Models;

namespace Rf.Sites.Features.Searching
{
    public interface ISearchPlugin
    {
        IEnumerable<Link> LinksFor(string searchTerm);
    }
}