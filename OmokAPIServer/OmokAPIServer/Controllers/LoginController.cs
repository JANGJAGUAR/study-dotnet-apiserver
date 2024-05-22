using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OmokAPIServer.Model;
using OmokAPIServer.Repository.Interface;
using OmokAPIServer.Service.Interface;
using OmokShareProject;
using ZLogger;

namespace OmokAPIServer.Controllers;

[ApiController]
[Route("[controller]")]

public class Login : ControllerBase
{
    private readonly ILogger<Login> _logger;
    private readonly IAuthService _authService;
    private readonly IUserInfoRdb _userInfoRdb;
    
    public Login(ILogger<Login> logger, IAuthService authService, IUserInfoRdb userInfoRdb)
    {
        _logger = logger;
        _authService = authService;
        _userInfoRdb = userInfoRdb;
    }

    [HttpPost]
    public async Task<LoginResponse> LoginAndLoadData(LoginRequest request)
    {
        LoginResponse response = new();

        //TODO: 로그로 어디까지 됐는지 구분
        Console.Write("Test");


        // API 레디스에 토큰 체크
        (ErrorCode errorCode, response.AuthToken) = await _authService.VerifyTokenToRedisApiDb(request.UserId, request.Password);

        // (에러 처리)
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }
        
        
        //유저 있는지 확인
        errorCode = await _authService.VerifyUserToRdb(request.UserId);
        
        // 유저가 없다면 유저 데이터 생성
        if(errorCode == ErrorCode.LoginFailUserNotExist)
        {
            errorCode = await _userInfoRdb.CreateNewUser(request.UserId);
        }
        
        // (에러 처리)
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }
        
        
        //로그인 시간 업데이트
        errorCode = await _userInfoRdb.UpdateLastLoginTime(request.UserId);
        
        // (에러 처리)
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }
        
        
        // 유저 데이터 로드
        (errorCode, response.UserData) = await _userInfoRdb.GetUserDataById(request.UserId);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }
        
        _logger.ZLogInformation($"[Login] ID : {response.UserId}, " +
                                $"LastAccessAt : {response.UserData.CreatedAt}, " +
                                $"LastAccessAt : {response.UserData.LastAccessAt}, " +
                                $"Win : {response.UserData.Win}, " +
                                $"Lose : {response.UserData.Lose}, " +
                                $"BackItem : {response.UserData.BackItem}");
        return response;
    
    }
    
}
public class LoginRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "ID CANNOT BE EMPTY")]
    [StringLength(30, ErrorMessage = "ID IS TOO LONG")]
    public string UserId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(30, ErrorMessage = "PASSWORD IS TOO LONG")]
    public string Password { get; set; }
}

public class LoginResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    
    [Required] public string UserId { get; set; } = "";
    [Required] public string AuthToken { get; set; } = "";
    public UserGameData UserData { get; set; }
}