using FubuCore.Binding;
using FubuMVC.Core.Runtime;
using Rf.Sites.Frame;

namespace Rf.Sites.Features.Administration
{
    public class InsertBehavior : AdminActionBasicBehavior
    {
        private readonly IFubuRequest _request;

        public InsertBehavior(IRequestData headers, IOutputWriter writer, AdminSettings adminSettings, IFubuRequest request)
            : base(headers, writer, adminSettings)
        {
            _request = request;
            DynamicBindExceptionHandler += x =>
                                               {
                                                   if (x.Message.Equals("Content-Type"))
                                                       BadRequest();
                                                   if (x.Message.Equals("Parse"))
                                                       UnsupportedMediaType();
                                               };
        }

        protected override void HandleAfterInsideInvoke()
        {
            if (!_request.Has<InsertInfo>())
                ServerError();
            else
            {
                var info = _request.Get<InsertInfo>();
                var url = info.Url;
                Writer.WriteHeader("Location", url);
                StatusCreated();
            }
        }
    }
}