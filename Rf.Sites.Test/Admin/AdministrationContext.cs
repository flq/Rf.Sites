using System;
using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using FubuCore.Binding;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Moq;
using Rf.Sites.Features.Administration;
using Rf.Sites.Frame;
using InMemoryRequestData = FubuCore.Binding.InMemoryRequestData;

namespace Rf.Sites.Test.Admin
{
    internal abstract class AdministrationContext<T> where T : AdminActionBasicBehavior
    {
        private readonly Mock<IOutputWriter> _outputWriter;
        protected T AdminBehavior;
        protected InMemoryRequestData RequestData;
        protected AdminSettings AdminSettings;
        protected TestBehavior Inner;
        protected InMemoryFubuRequest Request;

        protected AdministrationContext()
        {
            Request = new InMemoryFubuRequest();
            AdminSettings = new AdminSettings();
            _outputWriter = new Mock<IOutputWriter>();
            RequestData = new InMemoryRequestData(GetRequestParameters());
            AdminBehavior = CreateBehavior();
            Inner = new TestBehavior();
            AdminBehavior.InsideBehavior = Inner;
        }

        protected IOutputWriter OutputWriter
        {
            get { return _outputWriter.Object; }
        }

        protected virtual IDictionary<string, object> GetRequestParameters()
        {
            return new Dictionary<string, object>();
        }

        protected abstract T CreateBehavior();

        protected void Invoke()
        {
            AdminBehavior.Invoke();
        }

        protected void ThisStatusWasReturned(HttpStatusCode code)
        {
            _outputWriter.Verify(ow => ow.WriteResponseCode(code));
        }

        protected void InnerWasNotCalled()
        {
            Inner.WasCalled.Should().BeFalse();
        }

        protected void InnerWasCalled()
        {
            Inner.WasCalled.Should().BeTrue();
        }

        protected void EnsureSuccessfulAuthentication()
        {
            RequestData["X-RfSite-AdminToken"] = "bar";
            AdminSettings.AdminToken = "bar";
        }

        internal class TestBehavior : IActionBehavior
        {
            private Action _action = () => { };

            public void Invoke()
            {
                WasCalled = true;
                // Impl Detail! Fubu wraps exceptions into its own !!
                try
                {
                    _action();
                }
                catch (Exception x)
                {
                    throw new FubuException(666, x, "Yo! Bang!");
                }
            }

            void IActionBehavior.InvokePartial()
            {
                throw new NotImplementedException();
            }

            public bool WasCalled { get; set; }

            public void SetAction(Action action)
            {
                _action += action;
            }
        }

        protected void WhenInnerIsCalledDoThis(Action action)
        {
            Inner.SetAction(action);
        }
    }
}