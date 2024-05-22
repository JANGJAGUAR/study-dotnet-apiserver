using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OmokHiveServer.Repository.Interface;
using OmokShareProject;
using ZLogger;

namespace OmokHiveServer.Controller;

[ApiController]
[Route("[controller]")]
public class HiveLogin : ControllerBase
{
    private readonly ILogger<HiveLogin> _logger;
    private readonly IRedisHiveDb _redisHiveDb;
    
    public HiveLogin(ILogger<HiveLogin> logger, IRedisHiveDb redisHiveDb, IConfiguration config)
    {
        _logger = logger;
        _redisHiveDb = redisHiveDb;
    }
    [HttpPost]
    public async Task<HiveLoginResponse> VerifyAndLogin(HiveLoginRequest request)
    {
        HiveLoginResponse response = new();
        
        (ErrorCode errorCode, string token) = await _redisHiveDb.VerifyHiveToken(request.Userid, request.Password);
        response.Token = token;
        
        // (에러 처리)
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        _logger.ZLogInformation($"[VerifyAndLogin] ID:{request.Userid}, Password:{request.Password}");
        return response;
    }
}
public class HiveLoginRequest
{
    public string Userid { get; set; }
    public string Password { get; set; }
}

public class HiveLoginResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    public string Token { get; set; }
}