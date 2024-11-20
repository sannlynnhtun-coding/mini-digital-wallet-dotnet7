namespace MiniDigitalWallet.Domain.Features.ChangePin;

public class ChangePinService
{
    private readonly AppDbContext _db;

    public ChangePinService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<ChangePinResponseModel>> ChangePinAsync(ChangePinRequestModel requestModel)
    {
        Result<ChangePinResponseModel> model;
        try
        {
            var user = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == requestModel.UserId);
            if (user == null)
                return Result<ChangePinResponseModel>.ValidationError("User not found.");

            #region Validation

            var result = await new ChangePinRequestModelValidator()
                .ValidateModelAsync<ChangePinRequestModel, ChangePinResponseModel>(requestModel);
            if (result.IsValidationError)
            {
                model = result;
                goto Response;
            }

            #endregion

            #region Change Pin

            user.PinCode = requestModel.NewPin;
            await _db.SaveAndDetachAsync();

            model = Result<ChangePinResponseModel>.Success("Pin code changed successfully.");

            #endregion
        }
        catch (Exception ex)
        {
            model = Result<ChangePinResponseModel>.SystemError(ex.Message);
        }

        Response:
        return model;
    }
}