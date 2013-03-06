using FubuMVC.Core;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features
{
    public class ViewOutputsEndpoints
    {
        [FubuPartial]
        public ContentVM ContentVM(ContentVM vm)
        {
            return vm;
        }

        [FubuPartial]
        public InputModel404 InputModel404(InputModel404 vm)
        {
            return vm;
        }

        [FubuPartial]
        public NotYetPublishedVM InputModel404(NotYetPublishedVM vm)
        {
            return vm;
        }
    }
}