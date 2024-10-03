using BankingSystem.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace BankingSystem.Application.Behaviours
{
    public interface IValidationBehaviour : IBaseRequest
    {

    }

    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IValidationBehaviour, IRequest<TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationBehaviour(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _validator.ValidateRequest(request);

            return await next();
        }
    }

    public static class ValidatorExtensions
    {
        public static void ValidateRequest<TRequest>(this IValidator<TRequest> validator, TRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                throw new BankingException(BuildErrorMesage(validationResult.Errors), ErrorCode.Validation);
        }

        private static string BuildErrorMesage(IEnumerable<ValidationFailure> errors)
        {
            return errors.First().ToString();
        }
    }
}
