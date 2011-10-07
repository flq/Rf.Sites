using System;
using System.Linq;
using Rf.Sites.Entities;
using Rf.Sites.Features.Administration;
using Rf.Sites.Frame.Persistence;

namespace Rf.Sites.Frame
{
    public class ContentAdminImpl : IContentAdministration
    {
        private readonly Func<IRepository<Tag>> _tagRepFactory;

        public ContentAdminImpl(Func<IRepository<Tag>> tagRepFactory)
        {
            _tagRepFactory = tagRepFactory;
        }

        public string[] GetTags()
        {
            return _tagRepFactory().Select(t => t.Name).ToArray();
        }

        public void UpdateContent(dynamic content)
        {
            
        }

        public int InsertContent(dynamic content)
        {
            return 0;
        }
    }
}