using System;
using NHibernate.Validator.Engine;

namespace Rf.Sites.Domain.Frame
{
  public class NHBasedValidator : IValidator
  {
    private readonly ValidatorEngine validatorEngine;

    public NHBasedValidator(ValidatorEngine validatorEngine)
    {
      this.validatorEngine = validatorEngine;
    }

    public bool Validate(object entity)
    {
      return validatorEngine.IsValid(entity);
    }
  }
}