namespace MiniDigitalWallet.Domain.Features.WalletUser;

public class WalletUserService
{
    private readonly AppDbContext _db;

    public WalletUserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<WalletUserResponseModel>> GetUserAsync(WalletUserRequestModel requestModel)
    {
        Result<WalletUserResponseModel> model;
        try
        {
            var user = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == requestModel.UserId);
            if (user == null)
                return Result<WalletUserResponseModel>.ValidationError("User not found.");

            var responseModel = new WalletUserResponseModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                MobileNumber = user.MobileNumber,
                Balance = user.Balance,
                Status = user.Status,
                PinCode = user.PinCode
            };

            model = Result<WalletUserResponseModel>.Success(responseModel, "User retrieved successfully.");
        }
        catch (Exception ex)
        {
            model = Result<WalletUserResponseModel>.SystemError(ex.Message);
        }

        return model;
    }
}