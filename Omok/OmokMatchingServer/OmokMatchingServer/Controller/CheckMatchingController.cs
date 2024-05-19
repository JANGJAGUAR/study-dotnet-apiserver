using Microsoft.AspNetCore.Mvc;
using OmokMatchingServer.CS;
using OmokMatchingServer.Service.Interface;

namespace OmokMatchingServer.Controller;

[ApiController]
[Route("[controller]")]
public class CheckMatching : ControllerBase
{
    IMatchWorker _matchWorker;


    public CheckMatching(IMatchWorker matchWorker)
    {
        _matchWorker = matchWorker;
    }

    [HttpPost]
    public CheckMatchingResponse Post(CheckMatchingRequest request)
    {
        CheckMatchingResponse response = new();

        (var result, var matchingData) = _matchWorker.GetCompleteMatchingDic(request.UserId);
        
        //TODO: matchingData의 내용이 있으면, 소켓 서버로 연결을 시켜주고, 결과를 담아서 보낸다
        return response;
    }
}
public class CheckMatchingRequest
{
    public string UserId { get; set; }
}


public class CheckMatchingResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
    public string ServerAddress { get; set; } = "";
    public int RoomNumber { get; set; } = 0;    
}