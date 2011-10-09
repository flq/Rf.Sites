using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using Rf.Sites.Features.Administration;

namespace Rf.Sites.Test.Admin
{
    [TestFixture]
    internal class WrongAuthToken : AdministrationContext<AdminActionBasicBehavior>
    {
        protected override IDictionary<string, object> GetRequestParameters()
        {
            return new Dictionary<string, object> { {"X-RfSite-AdminToken", "foo"} } ;
        }

        protected override AdminActionBasicBehavior CreateBehavior()
        {
            AdminSettings.AdminToken = "bar";
            return new AdminActionBasicBehavior(RequestData, OutputWriter, AdminSettings);
        }

        [TestFixtureSetUp]
        public void Given()
        {
            Invoke();
        }

        [Test]
        public void Access_was_disallowed()
        {
            ThisStatusWasReturned(HttpStatusCode.Forbidden);
        }

        [Test]
        public void Inner_behavior_was_not_called()
        {
            InnerWasNotCalled();
        }
    }
}