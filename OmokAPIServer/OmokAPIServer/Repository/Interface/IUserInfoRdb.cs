using OmokAPIServer.Model;
using OmokShareProject;

namespace OmokAPIServer.Repository.Interface;

public interface IUserInfoRdb : IDisposable
{
    public Task<ErrorCode> CreateNewUser(string id);
    public Task<(ErrorCode, UserGameData)> GetUserDataById(string id);
    public Task<ErrorCode> UpdateLastLoginTime(string id);
}