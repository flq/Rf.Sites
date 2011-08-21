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
        private readonly IPartialFactory _factory;

        public ContentContinuationBehavior(IFubuRequest request, IContainer container, IPartialFactory factory)
            : base(PartialBehavior.Executes)
        {
            _request = request;
            _container = container;
            _factory = factory;
        }



        protected override void afterInsideBehavior()
        {
            var output = _request.Get<ContentContinuation<C, VM>>();

            var destination = output.TransferRequired ?
                output.TransferInputModel :
                _container.With(output.Model).GetInstance<VM>();

            _request.Clear(output.GetType());

            _request.SetObject(destination);
            var partial = _factory.BuildPartial(destination.GetType());
            partial.InvokePartial();
        }
    }
}