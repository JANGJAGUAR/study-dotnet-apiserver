using OmokAPIServer.CS;

namespace OmokAPIServer.Repository.Interface;

public interface IRedisApiDb : IDisposable
{
    public Task<ErrorCode> RegisterToken(string id, string authToken);
    public Task<ErrorCode> VerifyUserToken(string id);
    public Task<(ErrorCode, string)> GetUserToken(string id);
}