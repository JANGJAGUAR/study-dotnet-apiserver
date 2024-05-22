using Microsoft.AspNetCore.Mvc;
using OmokMatchingServer.Service.Interface;
using OmokShareProject;

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
        
        //TODO: 에러처리
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