using APIServer.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using APIServer.Repository;
using ZLogger;

namespace APIServer.Service;

public class AuthService : IAuthService
{
    readonly ILogger<AuthService> _logger;
    readonly IUserDb _userDb;
    readonly IMemRdb _memRdb;
    string _hiveServerAPIAddress;

    public AuthService(ILogger<AuthService> logger, IConfiguration configuration, IUserDb userDb, IMemRdb memRdb)
    {
        _userDb = userDb;
        _logger = logger;
        _hiveServerAPIAddress = configuration.GetSection("HiveServerAddress").Value + "/VerifyToken";
        _memRdb = memRdb;
    }
    

    public async Task<ErrorCode> VerifyTokenToHive(string playerEmail, string hiveToken)
    {
        try
        {
            HttpClient client = new();
            
            // HTTP 전송(인증 맞는지)
            var hiveResponse = await client.PostAsJsonAsync(_hiveServerAPIAddress, new { Email = playerEmail, AuthToken = hiveToken });
            _logger.ZLogDebug($"Now");
            //hive 에 이거 받아서 주는 놈 만들기 (스테이스코드쟤는 머임)
            // 이름은("HiveServerAddress").Value + "/verifytoken"; 이거

            if (hiveResponse == null|| !ValidateHiveResponse(hiveResponse)) // 
            {
                _logger.ZLogDebug($"[VerifyTokenToHive Service] ErrorCode:{ErrorCode.Hive_Fail_InvalidResponse}, PlayerEmail = {playerEmail}, Token = {hiveToken}, StatusCode = {hiveResponse?.StatusCode}");
                return ErrorCode.Hive_Fail_InvalidResponse;
            }

            // HTTP 재전송 (에러코드 받음) 
            var authResult = await hiveResponse.Content.ReadFromJsonAsync<ErrorCodeDTO>();
            if (!ValidateHiveAuthErrorCode(authResult))
            {
                return ErrorCode.Hive_No_Response;
            }

            return ErrorCode.None;
        }
        catch
        {
            _logger.ZLogDebug($"[VerifyTokenToHive Service] ErrorCode:{ErrorCode.Hive_Fail_Exception}, PlayerID = {playerEmail}, Token = {hiveToken}");
            return ErrorCode.Hive_Fail_Exception;
        }
    }
    // ////////////////여기 위까지 했음
    
    public async Task<(ErrorCode, int)> VerifyUser(string email)
    {
        try
        {
            // playerEmail로 userInfo 조회
            UserGameData userInfo = await _userDb.GetUserByPlayerEmail(email);
            
            if (userInfo is null)
            {
                return (ErrorCode.LoginFailUserNotExist, 0);
            }
    
            return (ErrorCode.None, userInfo.Accountid);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[VerifyUser] ErrorCode: {ErrorCode.LoginFailException}, PlayerId: {email}");
            return (ErrorCode.LoginFailException, 0);
        }
    }

    // public async Task<ErrorCode> UpdateLastLoginTime(int uid)
    // {
    //     try
    //     {
    //         var count = await _gameDb.UpdateRecentLogin(uid);
    //
    //         if (count != 1)
    //         {
    //             _logger.ZLogError($"[UpdateLastLoginTime] ErrorCode: {ErrorCode.LoginUpdateRecentLoginFail}, count : {count}");
    //             return ErrorCode.LoginUpdateRecentLoginFail;
    //         }
    //
    //         return ErrorCode.None;
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.ZLogError(e,
    //             $"[UpdateLastLoginTime] ErrorCode: {ErrorCode.LoginUpdateRecentLoginFailException}, Uid: {uid}");
    //         return ErrorCode.CreateUserFailException;
    //     }
    // }

    // 얘네 둘도 머하는 걸까
    public bool ValidateHiveResponse(HttpResponseMessage? response)
    {
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return false;
        }
        return true;
    }

    bool ValidateHiveAuthErrorCode(ErrorCodeDTO? authResult)
    {
        if (authResult == null || authResult.Result != ErrorCode.None)
        {
            return false;
        }
    
        return true;
    }
    //
    // public async Task<(ErrorCode, string)> RegisterToken(int uid)
    // {
    //     var token = Security.CreateAuthToken();
    //
    //     return (await _memoryDb.RegistUserAsync(token, uid), token);
    // }
}
public class ErrorCodeDTO
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
}
