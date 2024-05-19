using System.Net.Http.Json;
using OmokGameClient.CS;

namespace OmokGameClient;

public class APIService
{
    private readonly HttpClient _client = new();
    string _matchingServerAddress;

    public void MatchingServerFind(string userid)
    {
        // HTTP 전송
        var matchingResponse = _client.PostAsJsonAsync(_matchingServerAddress+"", new { UserId = userid });
        //TODO: 뒤에 다른 이름으로 보내기
        
        // (에러 처리)
        if (matchingResponse == null)
        {
            Console.WriteLine($@"[Matching Btn Error] ErrorCode:{ErrorCode.Matching_Fail_InvalidResponse}, UserID = {userid}");
        }
    }
    public void MatchingServerCheck(string userid)
    {
        // HTTP 전송
        var matchingResponse = _client.PostAsJsonAsync(_matchingServerAddress+"", new { UserId = userid });
        //TODO: 뒤에 다른 이름으로 보내기
        
        // (에러 처리)
        if (matchingResponse == null)
        {
            Console.WriteLine($@"[Matching Btn Error] ErrorCode:{ErrorCode.Matching_Fail_InvalidResponse}, UserID = {userid}");
        }
    }
    
}