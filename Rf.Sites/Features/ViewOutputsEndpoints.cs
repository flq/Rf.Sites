using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using Rf.Sites.Features.Models;

namespace Rf.Sites.Features
{
    public class ViewOutputsEndpoints
    {
        [FubuPartial]
        public FubuContinuation InputModel404(InputModel404 vm)
        {
            return FubuContinuation.RedirectTo("~/404.htm");
        }

     }
}