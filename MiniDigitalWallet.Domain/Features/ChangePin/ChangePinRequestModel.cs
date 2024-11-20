namespace MiniDigitalWallet.Domain.Features.ChangePin;

public abstract class ChangePinRequestModel
{
    public int UserId { get; set; }
    public string NewPin { get; set; } = null!;
}