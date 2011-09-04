using System;
using System.IO;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class StreamingOutput : OutputNode<StreamingOutputBehavior>
    {
        
    }

    public class StreamingOutputBehavior : BasicBehavior
    {

        private readonly IStreamingData _streaming;
        private readonly IFubuRequest _request;

        public StreamingOutputBehavior(IStreamingData streaming, IFubuRequest request)
            : base(PartialBehavior.Executes)
        {
            _streaming = streaming;
            _request = request;
        }

        protected override void afterInsideBehavior()
        {
            var m = _request.Get<IStreamOutput>();
            if (m == null)
                throw new InvalidOperationException("No Streamoutput instance could be retrieved.");
            _streaming.OutputContentType = m.MimeType;
            using (var tw = new StreamWriter(_streaming.Output))
              m.Accept(tw);
        }
    }

    public interface IStreamOutput
    {
        string MimeType { get; }
        void Accept(TextWriter textWriter);
    }
}