using OmokMatchingServer.Model;
using OmokShareProject;

namespace OmokMatchingServer.Service.Interface;

public interface IMatchWorker : IDisposable
{
    public ErrorCode AddUserReqQueue(string userId);

    public (bool, MatchingData) GetCompleteMatchingDic(string userId);
}