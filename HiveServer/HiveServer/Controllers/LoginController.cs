using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CloudStructures.Structures;
using HiveServer;
using HiveServer.Repository;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Execution;
using ZLogger;

namespace HiveServer.Controllers;

[ApiController]
[Route("[controller]")]

public class Login : ControllerBase
{
    private readonly IAccountDb _accountDb;
    private readonly IMemoryRdb _memoryDb;
    private readonly ILogger<Login> _logger;

    public Login(ILogger<Login> logger, IAccountDb accountDb, IMemoryRdb memoryDb)
    {
        _logger = logger;
        _accountDb = accountDb;
        _memoryDb = memoryDb;
    }
    
    [HttpPost]
    public async Task<LoginResponse> Post(LoginRequest request)
    {
        LoginResponse response = new();

        // ID, PW 검증
        (ErrorCode errorCode, long accountId) = await _accountDb.VerifyUser(request.Email, request.Password);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }


        string authToken = Security.CreateAuthToken();
        errorCode = await _memoryDb.RegistUserAsync(request.Email, authToken, accountId);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        _logger.ZLogInformation($"[Login] email:{request.Email}, AuthToken:{authToken}, AccountId:{accountId}");

        response.AuthToken = authToken;
        return response;
    }
}


public class LoginRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "EMAIL CANNOT BE EMPTY")]
    [StringLength(50, ErrorMessage = "EMAIL IS TOO LONG")]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
    public string Email { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(30, ErrorMessage = "PASSWORD IS TOO LONG")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class LoginResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    [Required] public string AuthToken { get; set; } = "";
}