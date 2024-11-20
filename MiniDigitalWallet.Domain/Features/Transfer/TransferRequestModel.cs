namespace MiniDigitalWallet.Domain.Features.Transfer;

public class TransferRequestModel
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public decimal Amount { get; set; }
}
