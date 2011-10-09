using System;
using System.Collections.Generic;
using FubuCore.Binding;
using NUnit.Framework;
using Rf.Sites.Features.Administration;

namespace Rf.Sites.Test.Admin
{
    [TestFixture]
    internal class AuthSuccess : AdministrationContext<AdminActionBasicBehavior>
    {

        protected override AdminActionBasicBehavior CreateBehavior()
        {
            return new AdminActionBasicBehavior(RequestData, OutputWriter, AdminSettings);
        }

        [TestFixtureSetUp]
        public void Given()
        {
            EnsureSuccessfulAuthentication();
            Invoke();
        }

        [Test]
        public void Inner_behavior_was_called()
        {
            InnerWasCalled();
        }
    }
}