using FluentValidation;

namespace MiniDigitalWallet.Domain.Features.Profile;

public class ProfileRequestModelValidator : AbstractValidator<ProfileRequestModel>
{
    public ProfileRequestModelValidator()
    {
        RuleFor(user => user.MobileNumber)
            .NotEmpty().WithMessage("Mobile number cannot be empty.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid mobile number format.");

        RuleFor(user => user.UserName)
            .NotEmpty().WithMessage("User name cannot be empty.")
            .MaximumLength(100).WithMessage("User name cannot be more than 100 characters.");

        RuleFor(user => user.PinCode)
            .NotEmpty().WithMessage("Pin code cannot be empty.")
            .Length(6).WithMessage("Pin code must be exactly 6 characters.");

        RuleFor(user => user.Balance)
            .GreaterThanOrEqualTo(0).WithMessage("Balance cannot be negative.")
            .When(user => user.Balance.HasValue);
    }
}