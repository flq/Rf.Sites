using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Rf.Sites.Frame.CloudStorageSupport
{
    public class MarkdownFile
    {
        public MarkdownFile(string name, byte[] getFile)
        {
            Name = name;
            RawContents = Encoding.UTF8.GetString(getFile);
            
            using (var r = new StringReader(RawContents))
            {
                IsValid = ParseRaw(r);
            }
        }

        public string Name { get; private set; }

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

        public bool HasBeenStoredLocally { get; set; }

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
            {
                var dto = DateTimeOffset.ParseExact(values["publish"], "yyyy.MM.dd HH.mm 'UTC' zzz", CultureInfo.InvariantCulture);
                Publish = dto.UtcDateTime;
            }
                
            if (values.ContainsKey("tags"))
                Tags = values["tags"].Split(',').Select(s => s.Trim().ToLowerInvariant()).ToList();
            return !string.IsNullOrEmpty(Title) && (Tags != null) && !string.IsNullOrEmpty(PostBody);
        }
    }
}