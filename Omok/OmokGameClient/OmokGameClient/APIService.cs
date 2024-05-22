using System.Net.Http.Json;
using OmokShareProject;

namespace OmokGameClient;

public class APIService
{
    private readonly HttpClient _client = new();
    string _apiServerAddress = "https://localhost:44368";
    string _matchingServerAddress = "";
    //TODO: 주소 적어야함


    public async void LoginApiServer(string userid, string password)
    {
        // HTTP 전송
        try
        {
            // POST 요청 보내기
            var loginResponse = await _client.PostAsJsonAsync(_apiServerAddress+"/Login", new { UserId = userid, Password = password });

            // 응답 성공 여부 확인
            loginResponse.EnsureSuccessStatusCode();

            // (에러 처리)
            if (loginResponse == null)
            {
                Console.WriteLine($@"[Login Btn Error] ErrorCode:{ErrorCode.Login_Fail_InvalidResponse}, UserID = {userid}");
            }
        }
        catch (HttpRequestException e)
        {
            // 오류 처리
            Console.WriteLine($"Request error: {e.Message}");
        }
        
        
        
    }
    public async void SignupApiServer(string userid, string password)
    {
        // HTTP 전송
        try
        {
            // POST 요청 보내기
            var signupResponse = await _client.PostAsJsonAsync(_apiServerAddress+"/Signup", new { UserId = userid, Password = password });

            // 응답 성공 여부 확인
            signupResponse.EnsureSuccessStatusCode();

            // (에러 처리)
            if (signupResponse == null)
            {
                Console.WriteLine($@"[Signup Btn Error] ErrorCode:{ErrorCode.Signup_Fail_InvalidResponse}, UserID = {userid}");
            }
        }
        catch (HttpRequestException e)
        {
            // 오류 처리
            Console.WriteLine($"Request error: {e.Message}");
        }
    }
    public async void MatchingServerFind(string userid, string token)
    {
        // HTTP 전송
        try
        {
            // POST 요청 보내기
            var matchingFindResponse = await _client.PostAsJsonAsync(_apiServerAddress+"/FindMatching", new { UserId = userid, Token = token });

            // 응답 성공 여부 확인
            matchingFindResponse.EnsureSuccessStatusCode();

            // (에러 처리)
            if (matchingFindResponse == null)
            {
                Console.WriteLine($@"[Matching Btn Error] ErrorCode:{ErrorCode.MatchingFind_Fail_InvalidResponse}, UserID = {userid}");
            }
        }
        catch (HttpRequestException e)
        {
            // 오류 처리
            Console.WriteLine($"Request error: {e.Message}");
        }
    }
    public async void MatchingServerCheck(string userid)
    {
        // HTTP 전송
        try
        {
            // POST 요청 보내기
            var matchingCheckResponse = await _client.PostAsJsonAsync(_matchingServerAddress+"/CheckMatching", new { UserId = userid });

            // 응답 성공 여부 확인
            matchingCheckResponse.EnsureSuccessStatusCode();

            // (에러 처리)
            if (matchingCheckResponse == null)
            {
                Console.WriteLine($@"[Matching Btn Error] ErrorCode:{ErrorCode.MatchingCheck_Fail_InvalidResponse}, UserID = {userid}");
            }
        }
        catch (HttpRequestException e)
        {
            // 오류 처리
            Console.WriteLine($"Request error: {e.Message}");
        }
    }
    
}