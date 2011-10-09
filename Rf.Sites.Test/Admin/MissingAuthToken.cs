using System;
using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using Rf.Sites.Features.Administration;

namespace Rf.Sites.Test.Admin
{
    [TestFixture]
    internal class MissingAuthToken : AdministrationContext<AdminActionBasicBehavior>
    {
        protected override AdminActionBasicBehavior CreateBehavior()
        {
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

    [TestFixture]
    internal class InInsertionArgumentExceptionCaught : AdministrationContext<InsertBehavior>
    {
        protected override InsertBehavior CreateBehavior()
        {
            return new InsertBehavior(RequestData, OutputWriter, AdminSettings, Request);
        }

        [TestFixtureSetUp]
        public void Given()
        {
            EnsureSuccessfulAuthentication();
            WhenInnerIsCalledDoThis(() =>
                                        {
                                            throw new ArgumentException("Id");
                                        });
            Invoke();
        }

        [Test]
        public void Inner_behavior_was_called()
        {
            InnerWasCalled();
        }

        public void Bad_request_is_sent_back()
        {
            ThisStatusWasReturned(HttpStatusCode.BadRequest);
        }
    }
}