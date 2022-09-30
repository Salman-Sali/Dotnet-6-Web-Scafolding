using FluentValidation;
using MediatR;
using Shared.Exceptions;

namespace WebApp.Extensions
{
    public class MediatRValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator> _validators;
        public MediatRValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validationFailures = _validators
                .Select(validator => validator.Validate(new ValidationContext<TRequest>(request)))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure != null)
                .ToList();

            if (validationFailures.Any())
            {
                var errorMessage = string.Join(" ", validationFailures);
                throw new HhcmException(errorMessage, System.Net.HttpStatusCode.BadRequest);
            }
            return next();
        }
    }
}
