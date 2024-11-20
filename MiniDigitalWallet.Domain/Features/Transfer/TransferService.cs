namespace MiniDigitalWallet.Domain.Features.Transfer;

public class TransferService
{
    private readonly AppDbContext _db;

    public TransferService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<TransferResponseModel>> TransferAsync(int senderId, int receiverId, decimal amount)
    {
        Result<TransferResponseModel> model;
        try
        {
            var sender = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == senderId);
            var receiver = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == receiverId);

            if (sender == null || receiver == null)
                return Result<TransferResponseModel>.ValidationError("Sender or receiver not found.");

            if (sender.Balance < amount)
                return Result<TransferResponseModel>.ValidationError("Insufficient balance.");

            sender.Balance -= amount;
            receiver.Balance += amount;

            var transaction = new TblTransaction
            {
                SenderUserId = senderId,
                ReceiverUserId = receiverId,
                Amount = amount,
                TransactionDate = DateTime.Now,
                TransactionType = "Transfer"
            };

            _db.TblTransactions.Add(transaction);
            await _db.SaveAndDetachAsync();

            var responseModel = new TransferResponseModel
            {
                Transaction = transaction,
                Response = BaseResponseModel.Success("000", "Success.")
            };

            model = Result<TransferResponseModel>.Success(responseModel, "Transfer completed successfully.");
        }
        catch (Exception ex)
        {
            model = Result<TransferResponseModel>.SystemError(ex.Message);
        }

        return model;
    }
}