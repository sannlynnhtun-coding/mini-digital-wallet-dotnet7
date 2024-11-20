namespace MiniDigitalWallet.Domain.Features.Register;

public class RegisterService
{
    private readonly AppDbContext _db;

    public RegisterService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<RegisterResponseModel>> RegisterAsync(RegisterRequestModel requestModel)
    {
        Result<RegisterResponseModel> model;
        try
        {
            // var validator = new RegisterRequestModelValidator();
            // var results = await validator.ValidateAsync(requestModel);
            //
            // if (!results.IsValid)
            // {
            //     var errors = string.Join(", ", results.Errors.Select(e => e.ErrorMessage));
            //     return Result<RegisterResponseModel>.ValidationError(errors);
            // }

            #region Validation

            var result = await new RegisterRequestModelValidator()
                .ValidateModelAsync<RegisterRequestModel, RegisterResponseModel>(requestModel);
            if (result.IsValidationError)
            {
                model = result;
                goto Response;
            }

            #endregion

            #region Register Wallet User
            
            TblWalletUser item = new TblWalletUser()
            {
                Balance = requestModel.Balance,
                Status = requestModel.Status,
                MobileNumber = requestModel.MobileNumber,
                PinCode = requestModel.PinCode,
                UserId = requestModel.UserId,
                UserName = requestModel.UserName,
            };

            await _db.TblWalletUsers.AddAsync(item);
            await _db.SaveAndDetachAsync();
            
            #endregion
            
            model = Result<RegisterResponseModel>.Success("User registered successfully.");
        }
        catch (Exception ex)
        {
            model = Result<RegisterResponseModel>.SystemError(ex);
        }

        Response:
        return model;
    }
}