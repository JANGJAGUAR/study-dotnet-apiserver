using Microsoft.AspNetCore.Mvc;
using OmokHiveServer.Repository.Interface;
using OmokShareProject;
using ZLogger;

namespace OmokHiveServer.Controller;

[ApiController]
[Route("[controller]")]
public class HiveSignup : ControllerBase
{
    private readonly ILogger<HiveSignup> _logger;
    private readonly IUserAccountRdb _userAccountRdb;

    public HiveSignup(ILogger<HiveSignup> logger, IUserAccountRdb userAccountRdb)
    {
        _logger = logger;
        _userAccountRdb = userAccountRdb;
    }
    [HttpPost]
    public async Task<HiveSignupResponse> Signup(HiveSignupRequest request)
    {
        var response = new HiveSignupResponse();
        var errorCode = await _userAccountRdb.CreateAccountData(request.UserId, request.Password);
        
        // (에러 처리)
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        _logger.ZLogInformation($"[Signup] ID:{request.UserId}");
        return response;
    }
    
}

public class HiveSignupRequest
{
    public string UserId { get; set; }
    public string Password { get; set; }
}

public class HiveSignupResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
}