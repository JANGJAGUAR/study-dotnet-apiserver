namespace HiveServer.Repository;

public interface IMemoryRdb : IDisposable
{
    public Task<ErrorCode> RegistUserAsync(string id, string authToken, long accountId);

    public Task<ErrorCode> CheckUserAuthAsync(string email, string authToken);

    public Task<(bool, RdbAuthUserData)> GetUserAsync(string id);
}