using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using Rf.Sites.Features;
using StructureMap;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class ContentContinuationBehavior<C,VM> : BasicBehavior where C : class where VM : class
    {
        private readonly IFubuRequest _request;
        
        private readonly IContainer _container;

        public ContentContinuationBehavior(IFubuRequest request, IContainer container)
            : base(PartialBehavior.Executes)
        {
            _request = request;
            _container = container;
        }

        protected override void afterInsideBehavior()
        {
            var output = _request.Get<ContentContinuation<C, VM>>();

            var destination = output.TransferRequired ?
                output.TransferInputModel :
                _container.With(output.Model).GetInstance<VM>();

            _request.Clear(output.GetType());
            _request.Set(FubuContinuation.TransferTo(destination));
        }
    }
}