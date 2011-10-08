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

        public object GetContent(int id)
        {
            return null;
        }

        public string[] GetTags()
        {
            return _tagRepFactory().Select(t => t.Name).ToArray();
        }

        public void UpdateContent(int id, dynamic content)
        {
            
        }

        public int InsertContent(dynamic content)
        {
            return 12;
        }
    }
}