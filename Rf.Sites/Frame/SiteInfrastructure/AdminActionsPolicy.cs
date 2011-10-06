using System;
using System.Net;
using FubuCore.Binding;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class AdminActionsPolicy : IActionBehavior
    {
        private readonly IFubuRequest _request;
        private readonly RequestHeaders _headers;
        private readonly IOutputWriter _writer;
        private readonly AdminSettings _adminSettings;

        public AdminActionsPolicy(IFubuRequest request, RequestHeaders headers, IOutputWriter writer, AdminSettings adminSettings)
        {
            _request = request;
            _headers = headers;
            _writer = writer;
            _adminSettings = adminSettings;
        }

        public IActionBehavior InsideBehavior { get; set; }


        public void Invoke()
        {
            try
            {
                DoNext.Continue
                    .When(AuthorizeHeadersExist)
                    .When(AuthorizationMatches)
                    .Finally(InsideBehavior.Invoke);
            }
            catch (FubuException x)
            {
                if (x.InnerException == null || !(x.InnerException is DynamicBindException))
                    throw;
                var exceptionText = x.InnerException.Message;
                if (exceptionText.Equals("Content-Type"))
                    BadRequest();
                if (exceptionText.Equals("Parse"))
                    UnsupportedMediaType();
            }
        }

        public void InvokePartial()
        {
            throw new NotSupportedException("No partial invoke through this policy");
        }

        private bool AuthorizeHeadersExist()
        {
            var headersExist = _headers["X-RfSite-Username"] != null && _headers["X-RfSite-Password"] != null;
            if (!headersExist)
                Forbidden();
            return headersExist;
        }

        private bool AuthorizationMatches()
        {
            var match = _headers["X-RfSite-Username"] == _adminSettings.AdminUser && _headers["X-RfSite-Password"] == _adminSettings.AdminPassword;
            if (!match)
               Forbidden();
            return match;
        }

        private void Forbidden()
        {
            _writer.WriteResponseCode(HttpStatusCode.Forbidden);
        }

        private void BadRequest()
        {
            _writer.WriteResponseCode(HttpStatusCode.BadRequest);
        }

        private void UnsupportedMediaType()
        {
            _writer.WriteResponseCode(HttpStatusCode.UnsupportedMediaType);
        }
    }


    public static class DoNextExtensions
    {
        public static DoNext When(this DoNext value, Func<bool> activity)
        {
            if (value == DoNext.Stop)
                return value;
            return activity() ? DoNext.Continue : DoNext.Stop;
        }

        public static DoNext Or(this DoNext value, Func<DoNext> activity)
        {
            return value == DoNext.Stop ? value : activity();
        }

        public static void Finally(this DoNext value, Action activity)
        {
            if (value != DoNext.Stop)
                activity();
        }
    }

}