using CloudStructures;
using CloudStructures.Structures;
using Microsoft.Extensions.Options;
using OmokAPIServer.Model;
using OmokAPIServer.Repository.Interface;
using OmokShareProject;
using ZLogger;

namespace OmokAPIServer.Repository;

public class RedisApiDb : IRedisApiDb
{
    private readonly ILogger<RedisApiDb> _logger;
    public RedisConnection _redisConn;
    
    public RedisApiDb(ILogger<RedisApiDb> logger, IOptions<DbConfig> dbConfig)
    {
        _logger = logger;

        RedisConfig config = new("default", dbConfig.Value.RedisApiDb);
        _redisConn = new RedisConnection(config);

        _logger.ZLogDebug($"userDbAddress:{dbConfig.Value.RedisApiDb}");
    }

    public void Dispose()
    {
    }
    
    public async Task<ErrorCode> RegisterToken(string id, string authToken)
    {
        string key = "UID_"+id;
        var result = ErrorCode.None;

        RedisAuthData user = new()
        {
            UserId = id,
            AuthToken = authToken
        };

        try
        {
            RedisString<RedisAuthData> redis = new(_redisConn, key, LoginTimeSpan());
            if (await redis.SetAsync(user) == false)
            {
                result = ErrorCode.LoginFailAddRedis;
                return result;
            }
        }
        catch
        {
            result = ErrorCode.LoginFailAddRedis;
            return result;
        }

        return result;
    }
    public async Task<ErrorCode> CheckExistenceToken(string id)
    {
        string key = "UID_"+id;
        var result = ErrorCode.None;

        try
        {
            RedisString<RedisAuthData> redis = new(_redisConn, key, null);
            RedisResult<RedisAuthData> user = await redis.GetAsync();

            // (에러처리)
            if (!user.HasValue)
            {
                result = ErrorCode.CheckAuthFailNotExist;
                return result;
            }
            if (user.Value.UserId != id)
            {
                result = ErrorCode.CheckAuthFailNotMatch;
                return result;
            }
        }
        catch
        {
            result = ErrorCode.CheckAuthFailException;
            return result;
        }
        
        return result;
    }
    public async Task<ErrorCode> VerifyUserToken(string id, string token)
    {
        string key = "UID_"+id;
        var result = ErrorCode.None;

        try
        {
            RedisString<RedisAuthData> redis = new(_redisConn, key, null);
            RedisResult<RedisAuthData> user = await redis.GetAsync();

            // (에러처리)
            if (!user.HasValue)
            {
                result = ErrorCode.CheckAuthFailNotExist;
                return result;
            }
            if (user.Value.UserId != id)
            {
                result = ErrorCode.CheckAuthFailNotMatch;
                return result;
            }
            if (user.Value.AuthToken != token)
            {
                result = ErrorCode.CheckAuthFailNotMatchToken;
                return result;
            }
        }
        catch
        {
            result = ErrorCode.CheckAuthFailException;
            return result;
        }
        
        return result;
    }

   
    public async Task<(ErrorCode, string)> GetUserToken(string id)
    {
        string key = "UID_"+id;

        try
        {
            RedisString<RedisAuthData> redis = new(_redisConn, key, null);
            RedisResult<RedisAuthData> user = await redis.GetAsync();
            if (!user.HasValue)
            {
                return (ErrorCode.GetTokenFailNotExist, "");
            }

            return (ErrorCode.None, user.Value.AuthToken);
        }
        catch
        {
            return (ErrorCode.GetTokenFailException, "");
        }
    }
    
    
    // 토큰 유지 시간
    public TimeSpan LoginTimeSpan()
    {
        ushort LoginKeyExpireMin = 10;
        //TODO : config로 빼기
        
        return TimeSpan.FromMinutes(LoginKeyExpireMin);
    }
}