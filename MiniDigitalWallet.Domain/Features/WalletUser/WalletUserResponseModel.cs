namespace MiniDigitalWallet.Domain.Features.WalletUser;

public class WalletUserResponseModel
{
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string MobileNumber { get; set; } = null!;
    public string PinCode { get; set; } = null!;
    public decimal? Balance { get; set; }
    public string? Status { get; set; }
}