using AnyStatus.API.Common;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Pipeline.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest>[] _validators;

        public ValidationBehavior(IValidator<TRequest>[] validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var failures = _validators
                .Select(v => v.Validate(request))
                .Where(v => v != null)
                .SelectMany(v => v)
                .Where(v => v != null)
                .ToList();

            if (failures.Any())
            {
                var sb = new StringBuilder();

                sb.AppendLine("Validation Error:");

                foreach (var failure in failures)
                {
                    sb.AppendLine(failure.ErrorMessage);
                }

                throw new ValidationException(sb.ToString());
            }
            else
            {
                return await next().ConfigureAwait(false);
            }
        }
    }
}
