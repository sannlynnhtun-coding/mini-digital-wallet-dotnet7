namespace MiniDigitalWallet.Domain.Models;

public class TransactionRequestModel
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
}
