using Microsoft.AspNetCore.Mvc;
using HiveServer.Repository;
using HiveServer.Models;
using ZLogger;


namespace HiveServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateAccount: ControllerBase
{
    private readonly IAccountDb _accountDb;
    private readonly ILogger<CreateAccount> _logger;

    public CreateAccount(ILogger<CreateAccount> logger, IAccountDb accountDb)
    {
        _logger = logger;
        _accountDb = accountDb;
    }

    // [HttpPost("test")]
    // [Route("test")]
    // public IActionResult test()
    // {
    //     return Ok(101);
    // }
    [HttpPost]
    public async Task<CreateAccountRes> Post(CreateAccountReq request)
    {
        var response = new CreateAccountRes();
    
        var errorCode = await _accountDb.CreateAccountAsync(request.Email, request.Password);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        _logger.ZLogInformation($"[CreateAccount] Email:{request.Email}");
        _logger.ZLogInformation($"[CreateAccount] {request:json}");
        _logger.ZLogInformation($"[CreateAccount] {new { request.Email }:json}");
        return response;
    }
}