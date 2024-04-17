using HiveServer.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HiveServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController: ControllerBase
{
    private readonly IAccountDb _accountDb;
    private readonly IMemoryDb _memoryDb;
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger, IAccountDb accountDb, IMemoryDb memoryDb)
    {
        _logger = logger;
        _accountDb = accountDb;
        _memoryDb = memoryDb;
    }
    
    // [HttpPost]
    // public async Task<LoginResponse> Post(LoginRequest request)
    // {
    //     LoginResponse response = new();
    //
    //     // ID, PW 검증
    //     (ErrorCode errorCode, long accountId) = await _accountDb.VerifyUser(request.Email, request.Password);
    //     if (errorCode != ErrorCode.None)
    //     {
    //         response.Result = errorCode;
    //         return response;
    //     }
    //
    //
    //     string authToken = Security.CreateAuthToken();
    //     errorCode = await _memoryDb.RegistUserAsync(request.Email, authToken, accountId);
    //     if (errorCode != ErrorCode.None)
    //     {
    //         response.Result = errorCode;
    //         return response;
    //     }
    //
    //     _logger.ZLogInformation($"[Login] email:{request.Email}, AuthToken:{authToken}, AccountId:{accountId}");
    //
    //     response.AuthToken = authToken;
    //     return response;
    // }
}