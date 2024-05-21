using System.Net.Http.Json;
using OmokShareProject;

namespace OmokGameClient;

public class APIService
{
    private readonly HttpClient _client = new();
    string _apiServerAddress = "https://localhost:44384";
    string _matchingServerAddress = "34.64.235.108:32884";

    
    public void LoginApiServer(string userid, string password)
    {
        // HTTP 전송
        var matchingResponse = _client.PostAsJsonAsync(_apiServerAddress+"/Login", new { UserId = userid, Password = password });
        //TODO: 이 이름 맞는지
        
        // (에러 처리)
        if (matchingResponse == null)
        {
            Console.WriteLine($@"[Login Btn Error] ErrorCode:{ErrorCode.Login_Fail_InvalidResponse}, UserID = {userid}");
        }
    }
    public void SignupApiServer(string userid, string password)
    {
        // HTTP 전송
        var matchingResponse = _client.PostAsJsonAsync(_apiServerAddress+"/Signup", new { UserId = userid, Password = password });
        //TODO: 이 이름 맞는지
        
        // (에러 처리)
        if (matchingResponse == null)
        {
            // Console.WriteLine($@"[Login Btn Error] ErrorCode:{ErrorCode.Signup_Fail_InvalidResponse}, UserID = {userid}");
        }
    }
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