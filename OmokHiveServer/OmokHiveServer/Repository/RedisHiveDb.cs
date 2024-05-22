using CloudStructures;
using Microsoft.Extensions.Options;
using OmokHiveServer.Model;
using OmokHiveServer.Repository.Interface;
using OmokShareProject;
using ZLogger;

namespace OmokHiveServer.Repository;

public class RedisHiveDb : IRedisHiveDb
{
    
    private readonly ILogger<RedisHiveDb> _logger;
    public RedisConnection _redisConn;
  
    public RedisHiveDb(ILogger<RedisHiveDb> logger, IOptions<DbConfig> dbConfig)
    {
        _logger = logger;

        RedisConfig config = new("default", dbConfig.Value.RedisHiveDb);
        _redisConn = new RedisConnection(config);

        _logger.ZLogDebug($"userDbAddress:{dbConfig.Value.RedisHiveDb}");
    }
    
    public void Dispose()
    {
    }
    public async Task<ErrorCode> RegisterHiveToken(string userid, string password)
    {
        ErrorCode result = ErrorCode.None;
        //TODO: 계정이 mysql에서 확인 있으면 토큰 생성
        
        // string key = MemoryRdbKeyMaker.MakeUIDKey(email);
        //
        // RdbAuthUserData user = new()
        // {
        //     Email = email,
        //     AuthToken = authToken,
        //     AccountId = accountId,
        //     State = UserState.Default.ToString()
        // };
        //
        // try
        // {
        //     RedisString<RdbAuthUserData> redis = new(_redisConn, key, LoginTimeSpan());
        //     if (await redis.SetAsync(user, LoginTimeSpan()) == false)
        //     {
        //         result = ErrorCode.LoginFailAddRedis;
        //         return result;
        //     }
        // }
        // catch
        // {
        //     result = ErrorCode.LoginFailAddRedis;
        //     return result;
        // }

        return result;
    }
    
    public async Task<(ErrorCode, string)> VerifyHiveToken(string userid, string password)
    {
        
        ErrorCode result = ErrorCode.None;
        string token = "";
        
        //TODO: 레디스 확인, 있으면 바로 토큰 반환, 없으면 MYSQL 가서 등록 정보 확인 (있으면 토큰 생성 후 토큰 반환, 없으면 에러 메시지와 토큰"" 반환)
        
        
        // string key = MemoryRdbKeyMaker.MakeUIDKey(email);
        //
        // try
        // {
        //     RedisString<RdbAuthUserData> redis = new(_redisConn, key, null);
        //     RedisResult<RdbAuthUserData> user = await redis.GetAsync();
        //
        //     if (!user.HasValue)
        //     {
        //         result = ErrorCode.CheckAuthFailNotExist;
        //         return result;
        //     }
        //
        //     if (user.Value.Email != email || user.Value.AuthToken != authToken)
        //     {
        //         result = ErrorCode.CheckAuthFailNotMatch;
        //         return result;
        //     }
        // }
        // catch
        // {
        //     result = ErrorCode.CheckAuthFailException;
        //     return result;
        // }


        return (result, token);
    }
    
    public TimeSpan LoginTimeSpan()
    {
        ushort LoginKeyExpireMin = 20;
        //TODO : config로 빼기
        
        return TimeSpan.FromMinutes(LoginKeyExpireMin);
    }
}