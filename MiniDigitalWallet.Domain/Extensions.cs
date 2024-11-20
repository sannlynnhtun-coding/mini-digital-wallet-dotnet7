using FluentValidation;
using FluentValidation.Results;

namespace MiniDigitalWallet.Domain;

public static class Extensions
{
    public static async Task<Result<TResponse>> ValidateModelAsync<TRequest, TResponse>(this AbstractValidator<TRequest> validator, TRequest requestModel)
    {
        ValidationResult results = await validator.ValidateAsync(requestModel);

        if (!results.IsValid)
        {
            var errors = string.Join(", ", results.Errors.Select(e => e.ErrorMessage));
            return Result<TResponse>.ValidationError(errors);
        }

        return Result<TResponse>.Success();
    }
}