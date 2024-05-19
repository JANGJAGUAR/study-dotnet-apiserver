using OmokAPIServer.CS;
using OmokAPIServer.Model;

namespace OmokAPIServer.Repository.Interface;

public interface IUserInfoRdb : IDisposable
{
    public Task<ErrorCode> CreateNewUser(string id);
    public Task<(ErrorCode, UserGameData)> GetUserDataById(string id);
    public Task<ErrorCode> UpdateLastLoginTime(string id);
}