using FubuMVC.Core;
using Rf.Sites.Features.Models;

namespace Rf.Sites.Features
{
    public class Handle404Endpoint
    {
        [FubuPartial]
        public RedirectTo404 A404(RedirectTo404 vm)
        {
            return vm;
        }
     }
}