using FubuCore.Binding;
using FubuMVC.Core.Runtime;
using Rf.Sites.Frame;

namespace Rf.Sites.Features.Administration
{
    public class GetBehavior : AdminActionBasicBehavior
    {
        public GetBehavior(IRequestData headers, IOutputWriter writer, AdminSettings adminSettings)
            : base(headers, writer, adminSettings)
        {
            ContentAdminExceptionHandler += x =>
                                                {
                                                    if (x.Message.Equals("NotFound"))
                                                        NotFound();
                                                    else
                                                        ServerError();
                                                };
        }
    }
}