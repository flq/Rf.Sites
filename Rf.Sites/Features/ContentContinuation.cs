using System;
using System.Collections.Generic;
using System.Linq;

namespace Rf.Sites.Features
{
    public class ContentContinuation<M,ViewModel> where M : class where ViewModel : class
    {
        private readonly M _model;
        private object _noContentInputModel = new InputModel404("This resource is not available");
        private readonly List<dynamic> _transfersDueToModelCondition = new List<dynamic>();

        public ContentContinuation(M model)
        {
            _model = model;
        }

        public ContentContinuation<M,ViewModel> ConditionalTransferWhen(Func<M,bool> predicate, object newInputModel)
        {
            _transfersDueToModelCondition.Add(new { Predicate = predicate, NewModel = newInputModel });
            return this;
        }

        public void OverrideDefaultNoContentBehavior(object inputModel)
        {
            _noContentInputModel = inputModel;
        }

        public object TransferInputModel
        {
            get
            {
                return Model == null
                           ? _noContentInputModel
                           : _transfersDueToModelCondition.FirstOrDefault(@if => @if.Predicate(Model)).NewModel;
            }
        }

        public bool TransferRequired { get { return Model != null && _transfersDueToModelCondition.Any(@if => @if.Predicate(Model)); } }

        public M Model
        {
            get { return _model; }
        }
    }
} 