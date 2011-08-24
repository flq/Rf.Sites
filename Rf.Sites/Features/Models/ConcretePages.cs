using System.Linq;

namespace Rf.Sites.Features.Models
{
    public class ContentTeaserPage : Page<ContentTeaserVM>
    {
        public ContentTeaserPage(PagingArgs paging, IQueryable<ContentTeaserVM> query) : base(paging, query)
        {
        }
    }
}