using System;
using System.Collections.Generic;
using System.Linq;
using Rf.Sites.Features.Models;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class ContentContinuation<M,ViewModel> where M : class where ViewModel : class
    {
        private readonly M _model;
        private readonly object _noContentInputModel = new InputModel404("This resource is not available");
        private readonly List<dynamic> _transfersDueToModelCondition = new List<dynamic>();

        public ContentContinuation(M model)
        {
            _model = model;
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
            get { return _model; }
        }
    }
} 