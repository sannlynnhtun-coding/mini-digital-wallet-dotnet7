using FluentValidation;

namespace MiniDigitalWallet.Domain.Features.ChangePin;

public class ChangePinRequestModelValidator : AbstractValidator<ChangePinRequestModel>
{
    public ChangePinRequestModelValidator()
    {
        RuleFor(x => x.NewPin)
            .NotEmpty().WithMessage("Pin code cannot be empty.")
            .Length(6).WithMessage("Pin code must be exactly 6 characters.");
    }
}