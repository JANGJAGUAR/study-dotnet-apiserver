using OmokMatchingServer.Model;

namespace OmokMatchingServer.Service.Interface;

public interface IMatchWorker : IDisposable
{
    public void AddUserReqQueue(string userId);

    public (bool, MatchingData) GetCompleteMatchingDic(string userId);
}