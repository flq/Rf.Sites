using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Rf.Sites.Entities;

namespace Rf.Sites.Frame.CloudStorageSupport
{
    public class StoreMarkdownFileToDb : IDisposable
    {
        private readonly Lazy<ISession> _session;

        public StoreMarkdownFileToDb(Func<ISession> session)
        {
            _session = new Lazy<ISession>(session);
        }

        public void Store(IList<MarkdownFile> files)
        {
            files.ForEach(Store);
        }

        private void Store(MarkdownFile file)
        {
            try
            {
                var s = _session.Value;
                using (var tx = s.BeginTransaction())
                {
                    var chosenTags = ChosenTags(file, s);
                    var c = new Content
                    {
                        IsMarkdown = true,
                        Published = true,
                        Created = file.Publish.HasValue ? file.Publish.Value : DateTime.UtcNow,
                        Title = file.Title
                    };

                    c.SetBody(file.PostBody);

                    chosenTags.ForEach(c.AssociateWithTag);

                    s.Save(c);
                    tx.Commit();
                    file.HasBeenStoredLocally = true;
                }
            }
            catch (Exception)
            {
                // What could we sensibly do?
            }
        }

        private static List<Tag> ChosenTags(MarkdownFile file, ISession s)
        {
            return (from t in s.Query<Tag>()
                    where file.Tags.Contains(t.Name.ToLower())
                    select t).ToList();
        }

        public void Dispose()
        {
            if (_session.IsValueCreated)
                _session.Value.Dispose();
        }
    }
}