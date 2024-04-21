using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CloudStructures.Structures;
using APIServer;
using APIServer.Repository;
using APIServer.Service;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Execution;
using ZLogger;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]

public class Login : ControllerBase
{
    // private readonly IUserDb _userDb;
    private readonly IMemRdb _memoryDb;
    private readonly IUserDb _userDb;
    private readonly ILogger<Login> _logger;
    
    //readonly IMemoryDb _memoryDb;
    readonly IAuthService _authService;
    // readonly IGameService _gameService;
    // readonly IDataLoadService _dataLoadService;

    public Login(ILogger<Login> logger, IAuthService authService, IUserDb userDb, IMemRdb memoryDb) //  IMemoryDb memoryDb, IAuthService authService, IGameService gameService, IDataLoadService dataLoadService)
    {
        _logger = logger;
        _userDb = userDb;
        _memoryDb = memoryDb;
        
        // _memoryDb = memoryDb;
        _authService = authService;
        // _gameService = gameService;
        // _dataLoadService = dataLoadService;
    }

    [HttpPost]
    public async Task<LoginResponse> LoginAndLoadData(LoginRequest request)
    {
        LoginResponse response = new();

        //하이브 토큰 체크
        var errorCode = await _authService.VerifyTokenToHive(request.Email, request.AuthToken);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }
        
        //유저 있는지 확인
        (errorCode, var accountid) = await _authService.VerifyUser(request.Email);
        // 유저가 없다면 유저 데이터 생성
        if(errorCode == ErrorCode.LoginFailUserNotExist)
        {
            errorCode = await _userDb.InitNewUserGameData(request.Email);
        }
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }
        response.Accountid = accountid;
        
        //토큰 발급
        errorCode = await _memoryDb.RegisterToken(accountid, request.Email, request.AuthToken);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }
        response.AuthToken  = request.AuthToken;
        //
        // //로그인 시간 업데이트
        // errorCode = await _authService.UpdateLastLoginTime(uid);
        // if (errorCode != ErrorCode.None)
        // {
        //     response.Result = errorCode;
        //     return response;
        // }
        
        // 유저 데이터 로드
        (errorCode, response.userData) = await _userDb.LoadUserData(accountid);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }
        
        _logger.ZLogInformation($"[Login] Accountid : {accountid}, " +
                                $"Email : {response.userData.Email}, " +
                                $"Level : {response.userData.Level}, " +
                                $"Exp : {response.userData.Exp}, " +
                                $"Win : {response.userData.Win}, " +
                                $"Lose : {response.userData.Lose}");
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
    // [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    // [StringLength(30, ErrorMessage = "PASSWORD IS TOO LONG")]
    // [DataType(DataType.Password)]
    // 
    public string AuthToken { get; set; }
}

public class LoginResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    [Required] public string AuthToken { get; set; } = "";
    [Required] public int Accountid { get; set; } = 0;

    public UserGameData userData { get; set; }
}