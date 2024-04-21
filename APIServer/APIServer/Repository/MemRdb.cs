using CloudStructures;
using CloudStructures.Structures;
using Microsoft.Extensions.Options;
using ZLogger;

namespace APIServer.Repository;

public class MemRdb : IMemRdb
{
    private readonly ILogger<MemRdb> _logger; // accountDB?
    public RedisConnection _redisConn;
  
    public MemRdb(ILogger<MemRdb> logger, IOptions<DbConfig> dbConfig) // accountDB?
    {
        _logger = logger;

        RedisConfig config = new("default", dbConfig.Value.RedisDb);
        _redisConn = new RedisConnection(config);

        _logger.ZLogDebug($"userDbAddress:{dbConfig.Value.RedisDb}");
    }

    public void Dispose()
    {
    }
    public async Task<ErrorCode> RegisterToken(long id, string email, string authToken)
    {
        string key = MemoryRdbKeyMaker.MakeUIDKey(email);
        ErrorCode result = ErrorCode.None;

        RdbAuthUserData user = new()
        {
            Email = ""+email,
            AuthToken = authToken,
            AccountId = id,
            State = UserState.Default.ToString()
        };

        try
        {
            RedisString<RdbAuthUserData> redis = new(_redisConn, key, LoginTimeSpan());
            if (await redis.SetAsync(user, LoginTimeSpan()) == false)
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

    public async Task<ErrorCode> CheckUserAuthAsync(string id, string authToken)
    {
        string key = MemoryRdbKeyMaker.MakeUIDKey(id);
        ErrorCode result = ErrorCode.None;

        try
        {
            RedisString<RdbAuthUserData> redis = new(_redisConn, key, null);
            RedisResult<RdbAuthUserData> user = await redis.GetAsync();

            if (!user.HasValue)
            {
                result = ErrorCode.CheckAuthFailNotExist;
                return result;
            }

            if (user.Value.Email != id || user.Value.AuthToken != authToken)
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

   
    public async Task<(bool, RdbAuthUserData)> GetUserAsync(string id)
    {
        string uid = MemoryRdbKeyMaker.MakeUIDKey(id);

        try
        {
            RedisString<RdbAuthUserData> redis = new(_redisConn, uid, null);
            RedisResult<RdbAuthUserData> user = await redis.GetAsync();
            if (!user.HasValue)
            {
                return (false, null);
            }

            return (true, user.Value);
        }
        catch
        {
            return (false, null);
        }
    }
    
    

    public TimeSpan LoginTimeSpan()
    {
        return TimeSpan.FromMinutes(RediskeyExpireTime.LoginKeyExpireMin);
    }

    public TimeSpan TicketKeyTimeSpan()
    {
        return TimeSpan.FromSeconds(RediskeyExpireTime.TicketKeyExpireSecond);
    }

    public TimeSpan NxKeyTimeSpan()
    {
        return TimeSpan.FromSeconds(RediskeyExpireTime.NxKeyExpireSecond);
    }
}