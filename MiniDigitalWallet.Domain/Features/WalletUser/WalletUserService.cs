using Microsoft.EntityFrameworkCore;
using MiniDigitalWallet.Database.AppDbContextModels;
using MiniDigitalWallet.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniDigitalWallet.Domain.Features.WalletUser;

public class WalletUserService
{
    private readonly AppDbContext _db;

    public WalletUserService(AppDbContext context)
    {
        _db = context;
    }

    public async Task<Result<TblWalletUser>> RegisterAsync(TblWalletUser newUser)
    {
        try
        {
            var validationResult = ValidateUser(newUser);
            if (!validationResult.IsSuccess)
                return Result<TblWalletUser>.ValidationError(validationResult.Message);

            _db.TblWalletUsers.Add(newUser);
            await _db.SaveChangesAsync();
            return Result<TblWalletUser>.Success(newUser, "User registered successfully.");
        }
        catch (Exception ex)
        {
            return Result<TblWalletUser>.SystemError(ex.Message);
        }
    }

    public async Task<Result<TblWalletUser>> UpdateProfileAsync(int id, TblWalletUser updatedUser)
    {
        try
        {
            var user = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                return Result<TblWalletUser>.ValidationError("User not found.");

            var validationResult = ValidateUser(updatedUser);
            if (!validationResult.IsSuccess)
                return Result<TblWalletUser>.ValidationError(validationResult.Message);

            user.UserName = updatedUser.UserName;
            user.MobileNumber = updatedUser.MobileNumber;
            user.Balance = updatedUser.Balance;
            user.Status = updatedUser.Status;

            await _db.SaveChangesAsync();
            return Result<TblWalletUser>.Success(user, "Profile updated successfully.");
        }
        catch (Exception ex)
        {
            return Result<TblWalletUser>.SystemError(ex.Message);
        }
    }

    public async Task<Result<TblWalletUser>> ChangePinAsync(int id, string newPin)
    {
        try
        {
            var user = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                return Result<TblWalletUser>.ValidationError("User not found.");

            if (string.IsNullOrWhiteSpace(newPin))
                return Result<TblWalletUser>.ValidationError("Pin code cannot be empty.");

            if (newPin.Length != 6)
                return Result<TblWalletUser>.ValidationError("Pin code must be exactly 6 characters.");

            user.PinCode = newPin;
            await _db.SaveChangesAsync();
            return Result<TblWalletUser>.Success(user, "Pin code changed successfully.");
        }
        catch (Exception ex)
        {
            return Result<TblWalletUser>.SystemError(ex.Message);
        }
    }

    public async Task<Result<TblWalletUser>> GetUserAsync(int id)
    {
        try
        {
            var user = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                return Result<TblWalletUser>.ValidationError("User not found.");

            return Result<TblWalletUser>.Success(user, "User retrieved successfully.");
        }
        catch (Exception ex)
        {
            return Result<TblWalletUser>.SystemError(ex.Message);
        }
    }

    public async Task<Result<TransferResponseModel>> TransferAsync(int senderId, int receiverId, decimal amount)
    {
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
            await _db.SaveChangesAsync();

            var responseModel = new TransferResponseModel
            {
                Transaction = transaction,
                Response = BaseResponseModel.Success("000", "Success.")
            };

            return Result<TransferResponseModel>.Success(responseModel, "Transfer completed successfully.");
        }
        catch (Exception ex)
        {
            return Result<TransferResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<ResultTransferResponseModel>> TransferAsync2(int senderId, int receiverId, decimal amount)
    {
        try
        {
            var sender = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == senderId);
            var receiver = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == receiverId);

            if (sender == null || receiver == null)
                return Result<ResultTransferResponseModel>.ValidationError("Sender or receiver not found.");

            if (sender.Balance < amount)
                return Result<ResultTransferResponseModel>.ValidationError("Insufficient balance.");

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

            await _db.TblTransactions.AddAsync(transaction);
            await _db.SaveChangesAsync();

            var item = new ResultTransferResponseModel
            {
                Transaction = transaction
            };

            return Result<ResultTransferResponseModel>.Success(item, "Transfer completed successfully.");
        }
        catch (Exception ex)
        {
            return Result<ResultTransferResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<PaginationModel<TblTransaction>>> GetTransactionHistoryAsync(int userId, int pageNo, int pageSize)
    {
        try
        {
            var transactions = _db.TblTransactions
                .Where(t => t.SenderUserId == userId || t.ReceiverUserId == userId)
                .OrderByDescending(t => t.TransactionDate);

            var pagedTransactions = await PaginationModel<TblTransaction>.CreateAsync(transactions.AsNoTracking(), pageNo, pageSize);

            return Result<PaginationModel<TblTransaction>>.Success(pagedTransactions, "Transaction history retrieved successfully.");
        }
        catch (Exception ex)
        {
            return Result<PaginationModel<TblTransaction>>.SystemError(ex.Message);
        }
    }

    public async Task<Result<WithdrawResponseModel>> WithdrawAsync(int userId, decimal amount)
    {
        try
        {
            var user = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return Result<WithdrawResponseModel>.ValidationError("User not found.");

            if (user.Balance < amount)
                return Result<WithdrawResponseModel>.ValidationError("Insufficient balance.");

            user.Balance -= amount;

            var transaction = new TblTransaction
            {
                SenderUserId = userId,
                Amount = amount,
                TransactionDate = DateTime.Now,
                TransactionType = "Withdraw"
            };

            _db.TblTransactions.Add(transaction);
            await _db.SaveChangesAsync();

            return Result<WithdrawResponseModel>.Success("Withdraw completed successfully.");
        }
        catch (Exception ex)
        {
            return Result<WithdrawResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<DepositResponseModel>> DepositAsync(int userId, decimal amount)
    {
        try
        {
            var user = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return Result<DepositResponseModel>.ValidationError("User not found.");

            user.Balance += amount;

            var transaction = new TblTransaction
            {
                ReceiverUserId = userId,
                Amount = amount,
                TransactionDate = DateTime.Now,
                TransactionType = "Deposit"
            };

            _db.TblTransactions.Add(transaction);
            await _db.SaveChangesAsync();

            return Result<DepositResponseModel>.Success("Deposit completed successfully.");
        }
        catch (Exception ex)
        {
            return Result<DepositResponseModel>.SystemError(ex.Message);
        }
    }

    private Result<ValidateUserResponseModel> ValidateUser(TblWalletUser user)
    {
        if (string.IsNullOrWhiteSpace(user.MobileNumber))
            return Result<ValidateUserResponseModel>.ValidationError("Mobile number cannot be empty.");

        if (!System.Text.RegularExpressions.Regex.IsMatch(user.MobileNumber, @"^\+?[1-9]\d{1,14}$"))
            return Result<ValidateUserResponseModel>.ValidationError("Invalid mobile number format.");

        if (string.IsNullOrWhiteSpace(user.UserName))
            return Result<ValidateUserResponseModel>.ValidationError("User name cannot be empty.");

        if (user.UserName.Length > 100)
            return Result<ValidateUserResponseModel>.ValidationError("User name cannot be more than 100 characters.");

        if (string.IsNullOrWhiteSpace(user.PinCode))
            return Result<ValidateUserResponseModel>.ValidationError("Pin code cannot be empty.");

        if (user.PinCode.Length != 6)
            return Result<ValidateUserResponseModel>.ValidationError("Pin code must be exactly 6 characters.");

        if (user.Balance.HasValue && user.Balance.Value < 0)
            return Result<ValidateUserResponseModel>.ValidationError("Balance cannot be negative.");

        return Result<ValidateUserResponseModel>.Success();
    }
}
