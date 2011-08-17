using System;
using System.Collections.Generic;
using System.Linq;

namespace Rf.Sites.Features
{
    public class ContentContinuation<Model,ViewModel> where Model : class where ViewModel : class
    {
        private readonly Model _model;
        private object _noContentInputModel = new InputModel404("This resource is not available");
        private readonly List<dynamic> _conditionalTransfers = new List<dynamic>();

        public ContentContinuation(Model model)
        {
            _model = model;
        }

        public ContentContinuation<Model,ViewModel> ConditionalTransferWhen(Func<Model,bool> predicate, object newInputModel)
        {
            _conditionalTransfers.Add(new { Predicate = predicate, NewModel = newInputModel });
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
                return _model == null
                           ? _noContentInputModel
                           : _conditionalTransfers.FirstOrDefault(@if => @if.Predicate(_model)).NewModel;
            }
        }

        public bool TransferRequired { get { return _model != null && _conditionalTransfers.Any(@if => @if.Predicate(_model)); } }
    }
} 