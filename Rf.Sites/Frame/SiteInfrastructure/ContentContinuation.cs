using System;
using System.Collections.Generic;
using System.Linq;
using Rf.Sites.Features.Models;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class ContentContinuation<M,ViewModel> where M : class where ViewModel : class
    {
        private M _model;
        private bool _pipelineHasRun;
        private readonly object _noContentInputModel = new InputModel404("This resource is not available");
        private readonly List<dynamic> _transfersDueToModelCondition = new List<dynamic>();

        private readonly List<Func<M,M>> _pipeline = new List<Func<M, M>>();

        public ContentContinuation(M model)
        {
            _model = model;
        }

        public ContentContinuation<M,ViewModel> AddPipeline(Func<M,M> pipeline)
        {
            _pipeline.Add(pipeline);
            return this;
        }

        public ContentContinuation<M,ViewModel> ConditionalTransfer(Func<M,bool> predicate, Func<M,object> newInputModel)
        {
            _transfersDueToModelCondition.Add(new { Predicate = predicate, NewModel = newInputModel });
            return this;
        }


        public object TransferInputModel
        {
            get
            {
                return _transfersDueToModelCondition.FirstOrDefault(@if => @if.Predicate(Model)).NewModel(Model) ?? _noContentInputModel; 
            }
        }

        public bool TransferRequired { get { return Model == null || _transfersDueToModelCondition.Any(@if => @if.Predicate(Model)); } }

        public M Model
        {
            get
            {
                // Any pipeline defs will run only once
                if (_pipelineHasRun || _pipeline.Count == 0)
                    return _model;
                _model = _pipeline.Aggregate(_model, (m, pipeline) => pipeline(m));
                _pipelineHasRun = true;
                return _model;
            }
        }
    }
} 