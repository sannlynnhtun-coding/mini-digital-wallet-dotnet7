namespace MiniDigitalWallet.Domain.Models;

public class TransferResponseModel
{
    public BaseResponseModel Response { get; set; }
    public TblTransaction Transaction { get; set; } 
}