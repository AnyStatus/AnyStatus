using AnyStatus.API.Common;
using MediatR;
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

        public ValidationBehavior(IValidator<TRequest>[] validators) => _validators = validators;

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Length == 0)
            {
                return next();
            }

            var failures = _validators
                 .Select(v => v.Validate(request))
                 .Where(v => v != null)
                 .SelectMany(v => v)
                 .Where(v => v != null)
                 .ToList();

            if (failures.Any())
            {
                var sb = new StringBuilder();

                foreach (var failure in failures)
                {
                    sb.AppendLine(failure.ErrorMessage);
                }

                throw new ValidationException(sb.ToString());
            }

            return next();
        }
    }
}
