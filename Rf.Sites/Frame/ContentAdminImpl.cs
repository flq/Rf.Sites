using System;
using System.Collections.Generic;
using System.Linq;
using Rf.Sites.Entities;
using Rf.Sites.Features.Administration;
using Rf.Sites.Frame.Persistence;

namespace Rf.Sites.Frame
{
    public class ContentAdminImpl : IContentAdministration
    {
        private readonly Func<IRepository<Tag>> _tagRepFactory;
        private readonly Func<IRepository<Content>> _contentRepFactory;

        public ContentAdminImpl(Func<IRepository<Tag>> tagRepFactory, Func<IRepository<Content>> contentRepFactory)
        {
            _tagRepFactory = tagRepFactory;
            _contentRepFactory = contentRepFactory;
        }

        public dynamic GetContent(string id)
        {
            int i;
            if (!int.TryParse(id, out i))
                throw new ArgumentException("Id");
            var c = _contentRepFactory()[i];
            return new {
                         title = c.Title,
                         body = c.Body,
                         publishdate = c.Created,
                         isMarkdown = c.IsMarkdown.HasValue ? (bool)c.IsMarkdown : false,
                         tags = c.Tags.Select(t => t.Name).ToArray()
                       };
        }

        public string[] GetTags()
        {
            return _tagRepFactory().Select(t => t.Name).ToArray();
        }

        public void UpdateContent(string id, dynamic content)
        {
            
        }

        public string InsertContent(dynamic content)
        {
            var id = -1;
            _contentRepFactory().Transacted(r => id = r.Add(CreateContent(content)));
            return id.ToString();
        }

        private Content CreateContent(dynamic content)
        {
            var c = new Content
            {
                Title = content.title,
                Created = content.publishdate() ? content.publishdate : DateTime.UtcNow,
                IsMarkdown = content.isMarkdown() ? content.isMarkdown : false,
                MetaKeyWords = content.metaKeywords() ? content.metaKeywords : null,
            };
            c.SetBody(content.body);
            if (content.tags())
            {
                var tags = FindAllKnownTags((string[])content.tags);
                foreach (var t in tags)
                    c.AssociateWithTag(t);
            }
            return c;
        }

        private IEnumerable<Tag> FindAllKnownTags(IEnumerable<string> tags)
        {
            return _tagRepFactory().Where(dbTag => tags.Contains(dbTag.Name)).ToList();
        }
    }
}