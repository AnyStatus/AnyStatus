using AnyStatus.API.Common;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Core.Pipeline.Validation
{
    public class DefaultRequestValidator : IValidator<IRequest>
    {
        /// <summary>
        /// The default request validator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Returns a collection of validation results.</returns>
        public IEnumerable<ValidationResult> Validate(IRequest request)
        {
            if (request is null)
            {
                throw new ValidationException("Request cannot be null.");
            }

            var context = new ValidationContext(request, null, null);

            var failures = new List<ValidationResult>();

            Validator.TryValidateObject(request, context, failures);

            return failures;
        }
    }
}
