using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.API.Common
{
    public interface IValidator<in TRequest>
    {
        IEnumerable<ValidationResult> Validate(TRequest request);
    }
}
