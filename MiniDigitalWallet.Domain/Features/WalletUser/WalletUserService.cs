using Microsoft.EntityFrameworkCore;
using MiniDigitalWallet.Database.AppDbContextModels;
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

    public async Task<TblWalletUser> RegisterAsync(TblWalletUser newUser)
    {
        ValidateUser(newUser);

        _db.TblWalletUsers.Add(newUser);
        await _db.SaveChangesAsync();
        return newUser;
    }

    public async Task<TblWalletUser?> UpdateProfileAsync(int id, TblWalletUser updatedUser)
    {
        var user = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null)
        {
            return null;
        }

        ValidateUser(updatedUser);

        user.UserName = updatedUser.UserName;
        user.MobileNumber = updatedUser.MobileNumber;
        user.Balance = updatedUser.Balance;
        user.Status = updatedUser.Status;

        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<TblWalletUser?> ChangePinAsync(int id, string newPin)
    {
        var user = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(newPin))
        {
            throw new ArgumentException("Pin code cannot be empty.");
        }

        if (newPin.Length != 6)
        {
            throw new ArgumentException("Pin code must be exactly 6 characters.");
        }

        user.PinCode = newPin;
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<TblWalletUser?> GetUserAsync(int id)
    {
        return await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task TransferAsync(int senderId, int receiverId, decimal amount)
    {
        var sender = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == senderId);
        var receiver = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == receiverId);

        if (sender == null || receiver == null)
        {
            throw new ArgumentException("Sender or receiver not found.");
        }

        if (sender.Balance < amount)
        {
            throw new ArgumentException("Insufficient balance.");
        }

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
    }

    public async Task<PaginationModel<TblTransaction>> GetTransactionHistoryAsync(int userId, int pageNo, int pageSize)
    {
        var transactions = _db.TblTransactions
            .Where(t => t.SenderUserId == userId || t.ReceiverUserId == userId)
            .OrderByDescending(t => t.TransactionDate);

        return await PaginationModel<TblTransaction>.CreateAsync(transactions.AsNoTracking(), pageNo, pageSize);
    }

    public async Task WithdrawAsync(int userId, decimal amount)
    {
        var user = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }

        if (user.Balance < amount)
        {
            throw new ArgumentException("Insufficient balance.");
        }

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
    }

    public async Task DepositAsync(int userId, decimal amount)
    {
        var user = await _db.TblWalletUsers.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }

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
    }

    private void ValidateUser(TblWalletUser user)
    {
        if (string.IsNullOrWhiteSpace(user.MobileNumber))
        {
            throw new ArgumentException("Mobile number cannot be empty.");
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(user.MobileNumber, @"^\+?[1-9]\d{1,14}$"))
        {
            throw new ArgumentException("Invalid mobile number format.");
        }

        if (string.IsNullOrWhiteSpace(user.UserName))
        {
            throw new ArgumentException("User name cannot be empty.");
        }

        if (user.UserName.Length > 100)
        {
            throw new ArgumentException("User name cannot be more than 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(user.PinCode))
        {
            throw new ArgumentException("Pin code cannot be empty.");
        }

        if (user.PinCode.Length != 6)
        {
            throw new ArgumentException("Pin code must be exactly 6 characters.");
        }

        if (user.Balance.HasValue && user.Balance.Value < 0)
        {
            throw new ArgumentException("Balance cannot be negative.");
        }
    }
}
