using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OmokMatchingServer.CS;
using OmokMatchingServer.Service.Interface;

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
    public FindMatchingResponse Post(FindMatchingRequest request)
    {
        FindMatchingResponse response = new();

        _matchWorker.AddUserReqQueue(request.UserId);

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