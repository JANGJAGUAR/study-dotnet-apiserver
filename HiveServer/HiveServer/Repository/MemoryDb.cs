using CloudStructures;
using CloudStructures.Structures;
using HiveServer.Models;
using System;
using System.Threading.Tasks;


namespace HiveServer.Repository;
// Redis DB 관련
public class MemoryDb: IMemoryDb
{
    public RedisConnection _redisConn;
    // private static readonly ILogger<MemoryDb> s_logger = GetLogger<MemoryDb>();

    public void Init(string address)
    {
        RedisConfig config = new("default", address);
        _redisConn = new RedisConnection(config);

        // s_logger.ZLogDebug($"userDbAddress:{address}");
    }


    public async Task<ErrorCode> RegistUserAsync(string email, string authToken, long accountId)
    {
        string key = email; //MemoryDbKeyMaker.MakeUIDKey()
        ErrorCode result = ErrorCode.None;

        RedisDB user = new()
        {
            AuthToken = authToken,
            AccountId = accountId,
        };

        try
        {
            RedisString<RedisDB> redis = new(_redisConn, key, LoginTimeSpan());
            if (await redis.SetAsync(user, LoginTimeSpan()) == false)
            {
                // s_logger.ZLogError(EventIdDic[EventType.LoginAddRedis],
                //     $"Email:{email}, AuthToken:{authToken},ErrorMessage:UserBasicAuth, RedisString set Error");
                result = ErrorCode.LoginFailAddRedis;
                return result;
            }
        }
        catch
        {
            // s_logger.ZLogError(EventIdDic[EventType.LoginAddRedis],
            //     $"Email:{email},AuthToken:{authToken},ErrorMessage:Redis Connection Error");
            result = ErrorCode.LoginFailAddRedis;
            return result;
        }

        return result;
    }

    // public async Task<ErrorCode> CheckUserAuthAsync(string id, string authToken)
    // {
    //     string key = MemoryDbKeyMaker.MakeUIDKey(id);
    //     ErrorCode result = ErrorCode.None;
    //     //
    //     // try
    //     // {
    //     //     RedisString<RedisDB> redis = new(_redisConn, key, null);
    //     //     RedisResult<RedisDB> user = await redis.GetAsync();
    //     //
    //     //     if (!user.HasValue)
    //     //     {
    //     //         s_logger.ZLogError(EventIdDic[EventType.Login],
    //     //             $"RedisDb.CheckUserAuthAsync: Email = {id}, AuthToken = {authToken}, ErrorMessage:ID does Not Exist");
    //     //         result = ErrorCode.CheckAuthFailNotExist;
    //     //         return result;
    //     //     }
    //     //
    //     //     if (user.Value.Email != id || user.Value.AuthToken != authToken)
    //     //     {
    //     //         s_logger.ZLogError(EventIdDic[EventType.Login],
    //     //             $"RedisDb.CheckUserAuthAsync: Email = {id}, AuthToken = {authToken}, ErrorMessage = Wrong ID or Auth Token");
    //     //         result = ErrorCode.CheckAuthFailNotMatch;
    //     //         return result;
    //     //     }
    //     // }
    //     // catch
    //     // {
    //     //     s_logger.ZLogError(EventIdDic[EventType.Login],
    //     //         $"RedisDb.CheckUserAuthAsync: Email = {id}, AuthToken = {authToken}, ErrorMessage:Redis Connection Error");
    //     //     result = ErrorCode.CheckAuthFailException;
    //     //     return result;
    //     // }
    //
    //
    //     return result;
    // }
    //
    // public async Task<bool> SetUserStateAsync(RedisDB user, UserState userState)
    // {
    //     // string uid = MemoryDbKeyMaker.MakeUIDKey(user.Email);
    //     // try
    //     // {
    //     //     RedisString<RedisDB> redis = new(_redisConn, uid, null);
    //     //
    //     //     user.State = userState.ToString();
    //     //
    //     //     return await redis.SetAsync(user) != false;
    //     // }
    //     // catch
    //     // {
    //     return false;
    //     // }
    // }
    //
    // public async Task<(bool, RedisDB)> GetUserAsync(string id)
    // {
    //     // string uid = MemoryDbKeyMaker.MakeUIDKey(id);
    //     //
    //     // try
    //     // {
    //     //     RedisString<RedisDB> redis = new(_redisConn, uid, null);
    //     //     RedisResult<RedisDB> user = await redis.GetAsync();
    //     //     if (!user.HasValue)
    //     //     {
    //     //         s_logger.ZLogError(
    //     //             $"RedisDb.UserStartCheckAsync: UID = {uid}, ErrorMessage = Not Assigned User, RedisString get Error");
    //     //         return (false, null);
    //     //     }
    //     //
    //     //     return (true, user.Value);
    //     // }
    //     // catch
    //     // {
    //     //     s_logger.ZLogError($"UID:{uid},ErrorMessage:ID does Not Exist");
    //     //     return (false, null);
    //     // }
    // }
    //
    // public async Task<bool> SetUserReqLockAsync(string key)
    // {
    //     try
    //     {
    //         RedisString<RedisDB> redis = new(_redisConn, key, NxKeyTimeSpan());
    //         if (await redis.SetAsync(new RedisDB
    //         {
    //             // emtpy value
    //         }, NxKeyTimeSpan(), StackExchange.Redis.When.NotExists) == false)
    //         {
    //             return false;
    //         }
    //     }
    //     catch
    //     {
    //         return false;
    //     }
    //
    //     return true;
    // }
    //
    // public async Task<bool> DelUserReqLockAsync(string key)
    // {
    //     if (string.IsNullOrEmpty(key))
    //     {
    //         return false;
    //     }
    //
    //     try
    //     {
    //         RedisString<RedisDB> redis = new(_redisConn, key, null);
    //         bool redisResult = await redis.DeleteAsync();
    //         return redisResult;
    //     }
    //     catch
    //     {
    //         return false;
    //     }
    // }
    //
    //
    public TimeSpan LoginTimeSpan()
    {
        return TimeSpan.FromMinutes(RediskeyExpireTime.LoginKeyExpireMin);
    }
    //
    // public TimeSpan TicketKeyTimeSpan()
    // {
    //     return TimeSpan.FromSeconds(RediskeyExpireTime.TicketKeyExpireSecond);
    // }
    //
    // public TimeSpan NxKeyTimeSpan()
    // {
    //     return TimeSpan.FromSeconds(RediskeyExpireTime.NxKeyExpireSecond);
    // }
}