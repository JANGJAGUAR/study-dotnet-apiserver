using OmokAPIServer.Model;
using OmokAPIServer.Repository.Interface;
using OmokAPIServer.Service.Interface;
using OmokShareProject;
using ZLogger;

namespace OmokAPIServer.Service;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IRedisApiDb _redisApiDb;
    private readonly IUserInfoRdb _userInfoRdb;
    readonly string _hiveServerApiAddress;

    public AuthService(ILogger<AuthService> logger, IConfiguration configuration, IRedisApiDb redisApiDb, IUserInfoRdb userInfoRdb)
    {
        _logger = logger;
        _redisApiDb = redisApiDb;
        _userInfoRdb = userInfoRdb;
        _hiveServerApiAddress = configuration.GetSection("HiveServerAddress").Value + "/VerifyToken";
        //TODO: hive 이 이름 맞는지 확인
    }
    public async Task<(ErrorCode, string)> VerifyTokenToRedisApiDb(string id, string password)
    {
        
        // 없으면 만들어야함
        if (await _redisApiDb.VerifyUserToken(id) != ErrorCode.None)
        {
            try
            {
                HttpClient client = new();
            
                // HTTP 전송
                var hiveResponse = await client.PostAsJsonAsync(_hiveServerApiAddress, new { UserId = id, Password = password });
                
                //TODO: hive에 이거 받아서 주는 놈 만들기
                
                // (에러 처리)
                if (hiveResponse == null)
                {
                    _logger.ZLogDebug($"[VerifyTokenToHive Service] ErrorCode:{ErrorCode.Hive_Fail_InvalidResponse}, UserID = {id}, Password = {password}");
                    return (ErrorCode.Hive_Fail_InvalidResponse, "");
                }

                // HTTP 응답 (에러코드, 토큰 받음) 
                var authResult = await hiveResponse.Content.ReadFromJsonAsync<HiveResponse>();
                
                // (에러 처리)
                if (authResult == null)
                {
                    return (ErrorCode.Hive_No_Response, "");
                }
                
                // 바로 등록하고 출력
                await _redisApiDb.RegisterToken(id, authResult.AuthToken);
                return (ErrorCode.None, authResult.AuthToken);
            }
            catch
            {
                _logger.ZLogDebug($"[VerifyTokenToHive Service] ErrorCode:{ErrorCode.Hive_Fail_Exception}, UserID = {id}, Password = {password}");
                return (ErrorCode.Hive_Fail_Exception, "");
            }
            
        }
        // 있으면 바로 꺼내서 출력
        else
        {
            return await _redisApiDb.GetUserToken(id);
        }
    }

    public async Task<ErrorCode> VerifyUserToRdb(string id)
    {
        try
        {
            // playerEmail로 userInfo 조회
            (ErrorCode errorCode, UserGameData userInfo) = await _userInfoRdb.GetUserDataById(id);
            
            if (userInfo is null)
            {
                return ErrorCode.LoginFailUserNotExist;
            }
    
            return errorCode;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[VerifyUser] ErrorCode: {ErrorCode.LoginFailException}, PlayerId: {id}");
            return ErrorCode.LoginFailException;
        }
    }
    
}
public class HiveResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
    public string AuthToken { get; set; } = "";
}