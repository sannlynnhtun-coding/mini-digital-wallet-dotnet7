using System;
using System.Collections.Generic;

namespace MiniDigitalWallet.Database.AppDbContextModels;

public partial class TblWalletUser
{
    public int UserId { get; set; }

    public string MobileNumber { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string PinCode { get; set; } = null!;

    public decimal? Balance { get; set; }

    public string? Status { get; set; }
}
