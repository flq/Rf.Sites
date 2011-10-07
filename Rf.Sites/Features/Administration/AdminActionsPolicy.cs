using System;
using System.Net;
using FubuCore.Binding;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Rf.Sites.Frame;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features.Administration
{
    public class AdminActionsPolicy : IActionBehavior
    {
        private readonly IRequestData _headers;
        private readonly IOutputWriter _writer;
        private readonly AdminSettings _adminSettings;

        public AdminActionsPolicy(IRequestData headers, IOutputWriter writer, AdminSettings adminSettings)
        {
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
            var headersExist = _headers.Value("X-RfSite-Username") != null && _headers.Value("X-RfSite-Password") != null;
            if (!headersExist)
                Forbidden();
            return headersExist;
        }

        private bool AuthorizationMatches()
        {
            var match = _headers.Value("X-RfSite-Username").Equals(_adminSettings.AdminUser) && _headers.Value("X-RfSite-Password").Equals(_adminSettings.AdminPassword);
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
}