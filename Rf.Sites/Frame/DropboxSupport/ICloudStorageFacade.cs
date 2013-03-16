using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using DropNet.Models;
using System.Linq;

namespace Rf.Sites.Frame.DropboxSupport
{
    public interface ICloudStorageFacade
    {
        IList<MarkdownFile> GetAllUnpublished();

        void UpdatePublishState(IList<MarkdownFile> files);
    }

    public class MarkdownFile
    {
        private readonly MetaData _md;

        public MarkdownFile(MetaData md, byte[] getFile)
        {
            _md = md;
            RawContents = Encoding.Unicode.GetString(getFile);
            
            using (var r = new StringReader(RawContents))
            {
                IsValid = ParseRaw(r);
            }
        }

        public string Name { get { return _md.Name; } }

        public string RawContents { get; private set; }

        public string PublishedName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Name) +
                DropboxFacade.PublishedMarker +
                Path.GetExtension(Name);
            }
        }

        public bool IsValid { get; private set; }

        public string Title { get; private set; }

        public DateTime? Publish { get; private set; }

        public IList<string> Tags { get; private set; }

        public string PostBody { get; private set; }

        private bool ParseRaw(TextReader reader)
        {
            var values = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            string current;

            while (!string.IsNullOrEmpty(current = reader.ReadLine()))
            {
                var parts = current.Split(':');
                if (parts.Length != 2)
                    continue;
                values.Add(parts[0].Trim(), parts[1].Trim());
            }

            while (string.IsNullOrEmpty(current))
            {
                current = reader.ReadLine();
            }

            PostBody = current + reader.ReadToEnd();

            if (values.ContainsKey("title"))
                Title = values["title"];
            if (values.ContainsKey("publish"))
                Publish = DateTime.ParseExact(values["publish"], "dd.MM.yyyy", DateTimeFormatInfo.InvariantInfo);
            if (values.ContainsKey("tags"))
                Tags = values["tags"].Split(',').Select(s => s.Trim().ToLowerInvariant()).ToList();
            return !string.IsNullOrEmpty(Title) && (Tags != null) && !string.IsNullOrEmpty(PostBody);
        }
    }
}