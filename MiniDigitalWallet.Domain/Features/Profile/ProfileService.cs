namespace MiniDigitalWallet.Domain.Features.Profile;

public class ProfileService
{
    private readonly AppDbContext _db;

    public ProfileService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<ProfileResponseModel>> UpdateProfileAsync(ProfileRequestModel requestModel)
    {
        Result<ProfileResponseModel> model;
        try
        {
            var item = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == requestModel.UserId);
            if (item == null)
                return Result<ProfileResponseModel>.ValidationError("User not found.");

            #region Validation

            var result = await new ProfileRequestModelValidator()
                .ValidateModelAsync<ProfileRequestModel, ProfileResponseModel>(requestModel);
            if (result.IsValidationError)
            {
                model = result;
                goto Response;
            }

            #endregion

            #region Update Profile

            item.UserName = requestModel.UserName;
            item.MobileNumber = requestModel.MobileNumber;
            item.Balance = requestModel.Balance;
            item.Status = requestModel.Status;

            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();
            
            var responseModel = new ProfileResponseModel
            {
                UserId = item.UserId,
                UserName = item.UserName,
                MobileNumber = item.MobileNumber,
                Balance = item.Balance,
                Status = item.Status
            };

            model = Result<ProfileResponseModel>.Success(responseModel, "Profile updated successfully.");

            #endregion
        }
        catch (Exception ex)
        {
            model = Result<ProfileResponseModel>.SystemError(ex.Message);
        }

        Response:
        return model;
    }
}