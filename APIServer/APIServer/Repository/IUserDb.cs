namespace APIServer.Repository;

public interface IUserDb : IDisposable
{
    public Task<ErrorCode> InitNewUserGameData(String id);
    public Task<UserGameData> GetUserByPlayerEmail(string email);
    public Task<Tuple<ErrorCode, UserGameData>> LoadUserData(int accountid);
}