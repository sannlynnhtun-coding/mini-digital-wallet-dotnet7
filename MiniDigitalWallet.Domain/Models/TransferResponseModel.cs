using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniDigitalWallet.Database.AppDbContextModels;

namespace MiniDigitalWallet.Domain.Models;

public class TransferResponseModel
{
    public BaseResponseModel Response { get; set; }
    public TblTransaction Transaction { get; set; } 
}

public class ResultTransferResponseModel
{
    public TblTransaction Transaction { get; set; }
}
