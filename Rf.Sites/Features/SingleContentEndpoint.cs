﻿using System.Diagnostics;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Querying;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame;
using Rf.Sites.Frame.Persistence;
using Rf.Sites.Frame.SiteInfrastructure;

namespace Rf.Sites.Features
{
    public class SingleContentEndpoint
    {
        private readonly IRepository<Content> _repository;
        private readonly SiteSettings _settings;
        private readonly ServerVariables _vars;
        private readonly IUrlRegistry _reg;

        public SingleContentEndpoint(IRepository<Content> repository, SiteSettings settings, ServerVariables vars, IUrlRegistry reg)
        {
            _repository = repository;
            _settings = settings;
            _vars = vars;
            _reg = reg;
        }

        [UrlPattern("go/{Id}")]
        public ContentVM GetContent(ContentId contentId)
        {
            var c = _repository[contentId.Id];
            return c == null ? null : new ContentVM(c, _settings, _vars, _reg);
        }

        public class NullTo404Behavior : BasicBehavior
        {
            private readonly IFubuRequest _request;
            private readonly IPartialFactory _factory;
            private readonly IChainResolver _resolver;

            public NullTo404Behavior(IFubuRequest request, IPartialFactory factory, IChainResolver resolver)
                : base(PartialBehavior.Executes)
            {
                _request = request;
                _factory = factory;
                _resolver = resolver;
            }

            protected override DoNext performInvoke()
            {
                var bla = _request.Get<ContentVM>();
                
                if (bla == null)
                {
                    var inputModel404 = new InputModel404("Content not found");
                    _request.Set(inputModel404);
                    var p = _factory.BuildPartial(_resolver.FindUnique(inputModel404));
                    p.InvokePartial();
                    return DoNext.Stop;
                }
                
                return DoNext.Continue;
            }
        }
    }
}