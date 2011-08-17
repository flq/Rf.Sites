using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using Rf.Sites.Features;
using StructureMap;

namespace Rf.Sites.Bootstrapping
{
    public class ContentContinuationBehavior<C,VM> : BasicBehavior where C : class where VM : class
    {
        private readonly IFubuRequest _request;
        private readonly IContinuationDirector _continuation;
        private readonly IContainer _container;

        public ContentContinuationBehavior(IFubuRequest request, IContinuationDirector continuation, IContainer container)
            : base(PartialBehavior.Executes)
        {
            _request = request;
            _continuation = continuation;
            _container = container;
        }

        protected override void afterInsideBehavior()
        {
            var output = _request.Get<ContentContinuation<C, VM>>();
            if (output.TransferRequired)
                _continuation.TransferTo(output.TransferInputModel);
            else
            {
                _request.Clear(output.GetType());
                _request.Set(_container.With(output.Model).GetInstance<VM>());
            }
        }
    }
}