using Microsoft.AspNetCore.Mvc;
using MiniDigitalWallet.Database.AppDbContextModels;
using MiniDigitalWallet.Domain.Features.WalletUser;
using MiniDigitalWallet.Domain.Models;

namespace MiniDigitalWallet.Api.Endpoints.WalletUser;

[ApiController]
[Route("api/[controller]")]
public class WalletUsersController : ControllerBase
{
    private readonly WalletUserService _service;

    public WalletUsersController(WalletUserService service)
    {
        _service = service;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] TblWalletUser newUser)
    {
        try
        {
            var user = await _service.RegisterAsync(newUser);
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("UpdateProfile/{id}")]
    public async Task<IActionResult> UpdateProfile(int id, [FromBody] TblWalletUser updatedUser)
    {
        try
        {
            var user = await _service.UpdateProfileAsync(id, updatedUser);
            if (user == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("ChangePin/{id}")]
    public async Task<IActionResult> ChangePin(int id, [FromBody] string newPin)
    {
        try
        {
            var user = await _service.ChangePinAsync(id, newPin);
            if (user == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _service.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost("Transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferRequestModel requestModel)
    {
        try
        {
            await _service.TransferAsync(requestModel.SenderId, requestModel.ReceiverId, requestModel.Amount);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("TransactionHistory/{userId}")]
    public async Task<IActionResult> TransactionHistory(int userId, int pageNo = 1, int pageSize = 10)
    {
        var transactions = await _service.GetTransactionHistoryAsync(userId, pageNo, pageSize);
        return Ok(transactions);
    }
}