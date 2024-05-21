using OmokShareProject;

namespace OmokHiveServer.Repository.Interface;

public interface IUserAccountRdb : IDisposable
{
    public Task<ErrorCode> CreateAccountData(string userid, string password);
    
    public Task<ErrorCode> VerifyAccountData(string userid, string password);
}