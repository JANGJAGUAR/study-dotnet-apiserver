using OmokShareProject;

namespace OmokAPIServer.Repository.Interface;

public interface IRedisApiDb : IDisposable
{
    public Task<ErrorCode> RegisterToken(string id, string authToken);
    public Task<ErrorCode> CheckExistenceToken(string id);
    public Task<ErrorCode> VerifyUserToken(string id, string token);
    public Task<(ErrorCode, string)> GetUserToken(string id);
}