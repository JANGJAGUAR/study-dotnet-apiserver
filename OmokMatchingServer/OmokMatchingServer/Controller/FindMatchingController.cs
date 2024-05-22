using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OmokMatchingServer.Service.Interface;
using OmokShareProject;

namespace OmokMatchingServer.Controller;

[ApiController]
[Route("[controller]")]

public class FindMatching : ControllerBase
{
    IMatchWorker _matchWorker;
    
    public FindMatching(IMatchWorker matchWorker)
    {
        _matchWorker = matchWorker;
    }

    [HttpPost]
    //API 서버에서 인증받은 ID가 담긴 HTTP 요청
    public FindMatchingResponse Post(FindMatchingRequest request)
    {
        FindMatchingResponse response = new();

        _matchWorker.AddUserReqQueue(request.UserId);

        //TODO: 에러처리
        return response;
    }
    
}

public class FindMatchingRequest
{
    public string UserId { get; set; }
}

public class FindMatchingResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
}