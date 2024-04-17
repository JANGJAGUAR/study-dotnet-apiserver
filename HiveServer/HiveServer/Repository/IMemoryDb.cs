using HiveServer.Models;

namespace HiveServer.Repository;
// Redis DB 관련
public interface IMemoryDb
{
    public void Init(string address);

    public Task<ErrorCode> RegistUserAsync(string id, string authToken, long accountId);

    // public Task<ErrorCode> CheckUserAuthAsync(string id, string authToken);
    // public Task<(bool, RedisDB)> GetUserAsync(string id);
    //
    // public Task<bool> SetUserStateAsync(RedisDB user, UserState userState);
    //
    // public Task<bool> SetUserReqLockAsync(string key);
    //
    // public Task<bool> DelUserReqLockAsync(string key);
}