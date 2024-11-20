namespace MiniDigitalWallet.Domain.Models;

public class TransferRequestModel
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public decimal Amount { get; set; }
}
