using System;
using System.Collections.Generic;

namespace MiniDigitalWallet.Database.AppDbContextModels;

public partial class TblTransaction
{
    public int TransactionId { get; set; }

    public int? SenderUserId { get; set; }

    public int ReceiverUserId { get; set; }

    public string? TransactionType { get; set; }

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }
}
