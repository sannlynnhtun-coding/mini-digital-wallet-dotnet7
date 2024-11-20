using Microsoft.AspNetCore.Mvc;
using MiniDigitalWallet.Database.AppDbContextModels;
using MiniDigitalWallet.Domain.Features.Transfer;
using MiniDigitalWallet.Domain.Features.Wallet;
using MiniDigitalWallet.Domain.Models;

namespace MiniDigitalWallet.Api.Endpoints.Wallet;

[ApiController]
[Route("api/[controller]")]
public class WalletController : BaseController
{
    private readonly WalletService _service;

    public WalletController(WalletService service)
    {
        _service = service;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] TblWalletUser newUser)
    {
        var result = await _service.RegisterAsync(newUser);
        return Execute(result);
    }

    [HttpPatch("UpdateProfile/{id}")]
    public async Task<IActionResult> UpdateProfile(int id, [FromBody] TblWalletUser updatedUser)
    {
        var result = await _service.UpdateProfileAsync(id, updatedUser);
        return Execute(result);
    }

    [HttpPatch("ChangePin/{id}")]
    public async Task<IActionResult> ChangePin(int id, [FromBody] string newPin)
    {
        var result = await _service.ChangePinAsync(id, newPin);
        return Execute(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _service.GetUserAsync(id);
        return Execute(result);
    }

    [HttpPost("Transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferRequestModel requestModel)
    {
        var result = await _service.TransferAsync(requestModel.SenderId, requestModel.ReceiverId, requestModel.Amount);
        return Execute(result);
    }

    [HttpGet("TransactionHistory/{userId}")]
    public async Task<IActionResult> TransactionHistory(int userId, int pageNo = 1, int pageSize = 10)
    {
        var result = await _service.GetTransactionHistoryAsync(userId, pageNo, pageSize);
        return Execute(result);
    }

    [HttpPost("Withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] TransactionRequestModel requestModel)
    {
        var result = await _service.WithdrawAsync(requestModel.UserId, requestModel.Amount);
        return Execute(result);
    }

    [HttpPost("Deposit")]
    public async Task<IActionResult> Deposit([FromBody] TransactionRequestModel requestModel)
    {
        var result = await _service.DepositAsync(requestModel.UserId, requestModel.Amount);
        return Execute(result);
    }
}
