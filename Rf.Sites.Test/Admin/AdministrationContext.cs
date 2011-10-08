using System;
using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using FubuCore.Binding;
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
        protected IRequestData RequestData;
        protected AdminSettings AdminSettings;
        protected TestBehavior Inner;

        protected AdministrationContext()
        {
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

        protected abstract IDictionary<string, object> GetRequestParameters();

        protected abstract T CreateBehavior();

        protected void Invoke()
        {
            AdminBehavior.Invoke();
        }

        protected void ThisStatusWasReturned(HttpStatusCode code)
        {
            _outputWriter.Verify(ow => ow.WriteResponseCode(code));
        }

        internal class TestBehavior : IActionBehavior
        {
            public void Invoke()
            {
                WasCalled = true;
                
            }

            void IActionBehavior.InvokePartial()
            {
                throw new NotImplementedException();
            }

            public bool WasCalled { get; set; }
        }

        protected void InnerWasNotCalled()
        {
            Inner.WasCalled.Should().BeFalse();
        }

        protected void InnerWasCalled()
        {
            Inner.WasCalled.Should().BeTrue();
        }
    }
}