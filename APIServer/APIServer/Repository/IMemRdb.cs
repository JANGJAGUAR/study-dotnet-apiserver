namespace APIServer.Repository;

public interface IMemRdb : IDisposable
{
    public Task<ErrorCode> RegisterToken(long id, string email, string authToken);

    public Task<ErrorCode> CheckUserAuthAsync(string id, string authToken);

    public Task<(bool, RdbAuthUserData)> GetUserAsync(string id);
}