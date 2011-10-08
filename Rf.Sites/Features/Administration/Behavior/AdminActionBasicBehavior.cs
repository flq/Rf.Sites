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
    public class AdminActionBasicBehavior : IActionBehavior
    {
        private readonly IRequestData _headers;
        private readonly IOutputWriter _writer;
        private readonly AdminSettings _adminSettings;

        protected Action<DynamicBindException> DynamicBindExceptionHandler = _ => { };
        protected Action<ContentAdminException> ContentAdminExceptionHandler = _ => { };

        public AdminActionBasicBehavior(IRequestData headers, IOutputWriter writer, AdminSettings adminSettings)
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
                HandleAfterInsideInvoke();
            }
            catch (FubuException x)
            {
                if (x.InnerException != null)
                {
                    if (x.InnerException is DynamicBindException)
                    {
                        DynamicBindExceptionHandler((DynamicBindException)x.InnerException);
                        return;
                    }
                    if (x.InnerException is ContentAdminException)
                    {
                        ContentAdminExceptionHandler((ContentAdminException)x.InnerException);
                        return;
                    }
                }
                ServerError();
            }
        }

        protected IOutputWriter Writer
        {
            get { return _writer; }
        }

        protected virtual void HandleAfterInsideInvoke() { }

        void IActionBehavior.InvokePartial()
        {
            throw new NotSupportedException("No partial invoke through this policy");
        }

        private bool AuthorizeHeadersExist()
        {
            var headerExist = _headers.Value("X-RfSite-AdminToken") != null;
            if (!headerExist)
                Forbidden();
            return headerExist;
        }

        private bool AuthorizationMatches()
        {
            var match = _headers.Value("X-RfSite-AdminToken").Equals(_adminSettings.AdminToken);
            if (!match)
                Forbidden();
            return match;
        }

        protected void Forbidden() { Writer.WriteResponseCode(HttpStatusCode.Forbidden); }
        protected void BadRequest() { Writer.WriteResponseCode(HttpStatusCode.BadRequest); }
        protected void UnsupportedMediaType() { Writer.WriteResponseCode(HttpStatusCode.UnsupportedMediaType); }
        protected void NotFound() { Writer.WriteResponseCode(HttpStatusCode.NotFound); }
        protected void ServerError() { Writer.WriteResponseCode(HttpStatusCode.InternalServerError); }
    }
}