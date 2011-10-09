using FubuCore.Binding;
using FubuMVC.Core.Runtime;
using Rf.Sites.Frame;

namespace Rf.Sites.Features.Administration
{
    public class PutBehavior : AdminActionBasicBehavior
    {
        public PutBehavior(IRequestData headers, IOutputWriter writer, AdminSettings adminSettings)
            : base(headers, writer, adminSettings)
        {
            DynamicBindExceptionHandler += x =>
                                               {
                                                   if (x.Message.Equals("Content-Type"))
                                                       BadRequest();
                                                   if (x.Message.Equals("Parse"))
                                                       UnsupportedMediaType();
                                               };
            ContentAdminExceptionHandler += x =>
                                                {
                                                    if (x.Message.Equals("NotFound"))
                                                        NotFound();
                                                    else
                                                        ServerError();
                                                };
            ArgumentExceptionHandler += x =>
                                            {
                                                if (x.Message.Equals("Id"))
                                                    BadRequest();
                                                else
                                                    ServerError();
                                            };
        }
    }
}