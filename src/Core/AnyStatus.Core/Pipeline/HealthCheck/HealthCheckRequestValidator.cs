using AnyStatus.API.Common;
using AnyStatus.API.Widgets;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Core.Pipeline.HealthCheck
{
    /// <summary>
    /// Validate health check request.
    /// </summary>
    /// <typeparam name="TContext">The health-check.</typeparam>
    public class HealthCheckRequestValidator<TContext> : IValidator<StatusRequest<TContext>>
        where TContext : IStatusWidget
    {
        public IEnumerable<ValidationResult> Validate(StatusRequest<TContext> request)
        {
            if (request.Context is null)
            {
                throw new ValidationException("Request context is null.");
            }

            var context = new ValidationContext(request.Context, null, null);

            var results = new List<ValidationResult>();

            Validator.TryValidateObject(request.Context, context, results);

            return results;
        }
    }
}
