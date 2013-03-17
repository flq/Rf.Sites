using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Rf.Sites.Entities;

namespace Rf.Sites.Frame.CloudStorageSupport
{
    public class StoreToDb
    {
        private readonly Func<ISession> _session;

        public StoreToDb(Func<ISession> session)
        {
            _session = session;
        }

        public void Store(params MarkdownFile[] files)
        {
            files.ForEach(Store);
        }

        private void Store(MarkdownFile file)
        {
            try
            {
                var s = _session();
                using (var tx = s.BeginTransaction())
                {
                    var chosenTags = ChosenTags(file, s);
                    var c = new Content
                    {
                        IsMarkdown = true,
                        Body = file.PostBody,
                        Published = true,
                        Created = file.Publish.HasValue ? file.Publish.Value : DateTime.UtcNow,
                        Title = file.Title
                    };

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
    }
}