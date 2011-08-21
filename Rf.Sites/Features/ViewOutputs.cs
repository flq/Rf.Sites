using Rf.Sites.Features.DisplayContent;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features
{
    [HasActions]
    public class ViewOutputs
    {
        public ContentVM ContentVM(ContentVM vm)
        {
            return vm;
        }

        public InputModel404 InputModel404(InputModel404 vm)
        {
            return vm;
        }
    }
}