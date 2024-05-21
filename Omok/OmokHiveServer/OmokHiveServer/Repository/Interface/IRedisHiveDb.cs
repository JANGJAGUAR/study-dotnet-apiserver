using OmokShareProject;

namespace OmokHiveServer.Repository.Interface;

public interface IRedisHiveDb : IDisposable
{
    public Task<ErrorCode> RegisterHiveToken(string userid, string password);
    public Task<(ErrorCode, string)> VerifyHiveToken(string userid, string password);
}