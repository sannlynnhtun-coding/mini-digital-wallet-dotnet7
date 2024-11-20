namespace MiniDigitalWallet.Domain.Features.Register;

public abstract class RegisterRequestModel
{
    public int UserId { get; set; }

    public string MobileNumber { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string PinCode { get; set; } = null!;

    public decimal? Balance { get; set; }

    public string? Status { get; set; }
}