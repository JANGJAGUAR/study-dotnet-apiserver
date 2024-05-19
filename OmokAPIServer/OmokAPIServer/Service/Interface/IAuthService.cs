using OmokAPIServer.CS;

namespace OmokAPIServer.Service.Interface;

public interface IAuthService
{
    public Task<(ErrorCode, string)> VerifyTokenToRedisApiDb(string id, string password);
    public Task<ErrorCode> VerifyUserToRdb(string id);
}