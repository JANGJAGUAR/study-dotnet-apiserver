using System;
using System.ComponentModel.DataAnnotations;
// using HiveServer.Model.DTO;
using HiveServer.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
// using HiveServer.Services;
using Microsoft.Extensions.Configuration;
using ZLogger;


// namespace APIServer.Controllers; 이게 맞을까..?
namespace HiveServer.Controllers;

[ApiController]
[Route("[controller]")]
public class VerifyToken : ControllerBase
{
    // readonly string _saltValue;
    readonly ILogger<VerifyToken> _logger;
    readonly IMemoryRdb _memoryDb;
    // readonly IHiveDb _hiveDb;

    public VerifyToken(ILogger<VerifyToken> logger, IMemoryRdb memoryDb, IConfiguration config)
    {
        
        _logger = logger;
        _memoryDb = memoryDb;
    }

    [HttpPost]
    // public VerifyTokenResponse Verify( VerifyTokenBody request) {
    //     VerifyTokenResponse response = new(); 
    //     
    //     string saltValue = Security.SaltString();
    //     if (Security.MakeHashingToken(saltValue, request.PlayerId)!=request.HiveToken)
    //     {
    //         _logger.ZLogDebug(
    //             $"[AccoutDb.CreateAccount] ErrorCode: {ErrorCode.VerifyTokenFail}");
    //         response.Result =  ErrorCode.VerifyTokenFail;
    //     }
    //     _logger.ZLogDebug($"Success");
    //     return response;
    // }
    public async Task<VerifyResponse> Verify(VerifyRequest request)
    {
        VerifyResponse response = new();
        
        ErrorCode errorCode = await _memoryDb.CheckUserAuthAsync(request.Email, request.AuthToken);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        _logger.ZLogInformation($"[Verify] email:{request.Email}, AuthToken:{request.AuthToken}");
        return response;
    }
}


public class VerifyRequest
{
    public string Email { get; set; }
    public string AuthToken { get; set; }
}

public class VerifyResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
}

public class VerifyTokenResponse
{
    [Required]
    public ErrorCode Result { get; set; } = ErrorCode.None;
}

public class VerifyTokenBody
{
    [Required]
    public string HiveToken { get; set; }
    [Required]
    public Int64 PlayerId { get; set; }
}
