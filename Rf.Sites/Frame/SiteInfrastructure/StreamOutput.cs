using System.Collections.Generic;
using System.IO;
using System.Linq;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Policies;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Runtime;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class StreamOutput : IChainModification
    {
        public void Modify(BehaviorChain chain)
        {
            chain.Output.AddWriter(typeof(StreamingModelMediaWriter));
        }
    }

    public class StreamingModelMediaWriter : IMediaWriter<IStreamOutput>
    {
        private readonly IOutputWriter _writer;

        public StreamingModelMediaWriter(IOutputWriter writer)
        {
            _writer = writer;
        }

        public void Write(string mimeType, IStreamOutput resource)
        {
            _writer.ContentType(MimeType.MimeTypeByValue(resource.MimeType));
            _writer.Write(resource.MimeType, stream =>
            {
                using (var tw = new StreamWriter(stream))
                    resource.Accept(tw);
            });
        }

        public IEnumerable<string> Mimetypes
        {
            get { return Enumerable.Empty<string>(); }
        }
    }

    public interface IStreamOutput
    {
        string MimeType { get; }
        void Accept(TextWriter textWriter);
    }
}