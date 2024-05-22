using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OmokAPIServer.Repository.Interface;
using OmokShareProject;
using ZLogger;

namespace OmokAPIServer.Controllers;

[ApiController]
[Route("[controller]")]

public class FindMatching : ControllerBase
{
    private readonly ILogger<FindMatching> _logger;
    private readonly IRedisApiDb _redisApiDb;

    private readonly HttpClient _matching = new();
    string _matchingServerAddress = "";

    public FindMatching(ILogger<FindMatching> logger, IRedisApiDb redisApiDb)
    {
        _logger = logger;
        _redisApiDb = redisApiDb;
    }

    [HttpPost]
    public async Task<FindMatchingResponse> FindMatchingAndLoadData(FindMatchingRequest request)
    {
        FindMatchingResponse response = new();

        // API 레디스에 토큰 검증
        ErrorCode errorCode = await _redisApiDb.VerifyUserToken(request.UserId, request.Token);

        // (에러 처리)
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }
        //TODO: 함수로 만들어서 서비스로 빼자
        //{
        // HTTP 전송
        try
        {
            // POST 요청 보내기
            var signupResponse = await _matching.PostAsJsonAsync(_matchingServerAddress+"/FindMatching", new { UserId = request.UserId});

            // 응답 성공 여부 확인
            signupResponse.EnsureSuccessStatusCode();

            // (에러 처리)
            if (signupResponse == null)
            {
                Console.WriteLine($@"[API->Matching Error] ErrorCode:{ErrorCode.MatchingServer_Fail_InvalidResponse}, UserID = {request.UserId}");
            }
        }
        catch (HttpRequestException e)
        {
            // 오류 처리
            Console.WriteLine($"Request error: {e.Message}");
        }
        //}

        return response;

    }

}
public class FindMatchingRequest
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string Token { get; set; }
}

public class FindMatchingResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
}