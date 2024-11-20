namespace MiniDigitalWallet.Domain.Features.Transfer;

public class TransferResponseModel
{
    public BaseResponseModel Response { get; set; }
    public TblTransaction Transaction { get; set; } 
}