using System;
using System.Collections.Generic;
using System.IO;
using DropNet;
using System.Linq;
using DropNet.Models;

namespace Rf.Sites.Frame.DropboxSupport
{
    public class DropboxFacade : IDropboxFacade
    {
        public const string PublishedMarker = "-published";

        private readonly Func<DropNetClient> _clientProvider;

        public DropboxFacade(Func<DropNetClient> clientProvider)
        {
            _clientProvider = clientProvider;
        }

        public IList<MarkdownFile> GetAllUnpublished()
        {
            var dropNetClient = _clientProvider();
            var metaData = dropNetClient.GetMetaData();
            var unpublishedItems = metaData.Contents
                .Where(UnpublishedMarkdownFiles).ToList();

            return unpublishedItems
                .Select(md => new MarkdownFile(md, dropNetClient.GetFile("/" + md.Name)))
                .ToList();
        }

        public void UpdatePublishState(IList<MarkdownFile> files)
        {
            var dropNetClient = _clientProvider();
            foreach (var f in files)
            {
                dropNetClient.Move("/" + f.Name, "/" + f.PublishedName);
            }
        }

        private static bool UnpublishedMarkdownFiles(MetaData arg)
        {
            var withoutExtension = Path.GetFileNameWithoutExtension(arg.Name);
            var extension = Path.GetExtension(arg.Name);
            return 
                withoutExtension != null && 
                extension != null && 
                withoutExtension.IndexOf(PublishedMarker, StringComparison.Ordinal) == -1 && 
                extension == ".md";
        }
    }
}