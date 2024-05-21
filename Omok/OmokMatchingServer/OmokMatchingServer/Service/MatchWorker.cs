using System.Collections.Concurrent;
using CloudStructures;
using Microsoft.Extensions.Options;
using OmokMatchingServer.Model;
using OmokMatchingServer.Service.Interface;
using OmokShareProject;

namespace OmokMatchingServer.Service;

public class MatchWorker : IMatchWorker
{
    List<string> _pvpServerAddressList = new();

    System.Threading.Thread _matchWorker = null;
    ConcurrentQueue<string> _reqQueue = new();
    
    Queue<int> _roomNoQueue = new();

    // key는 유저ID
    ConcurrentDictionary<string, string> _completeMatchingDic = new();

    //TODO: 2개의 Pub/Sub을 사용하므로 Redis 객체가 2개 있어야 한다.
    // 매칭서버에서 -> 게임서버, 게임서버 -> 매칭서버로

    string _redisAddress = "";
    string _matchingRedisPubKey = "MatchingReq";
    string _matchingRedisSubKey = "MatchingReq";

    private string matchingId1;
    private string matchingId2;
    
    
    public RedisConnection _redisConn;
    string key = "userDataList-test-key";
    TimeSpan defaultExpiry = TimeSpan.FromDays(1);
    
    public MatchWorker(IOptions<DbConfig> dbConfig)
    {
        Console.WriteLine("MatchWoker 생성자 호출");
        _redisAddress = dbConfig.Value.RedisAddress;

        // Redis 연결 및 초기화
        RedisConfig config = new("default", _redisAddress);
        _redisConn = new RedisConnection(config);
        
        var redis = new CloudStructures.Structures.RedisList<string>(_redisConn, key, defaultExpiry);
        
        redis.RightPushAsync("34.64.235.108:32452"); //TODO:소켓 서버 정보, 이것들은 따로 레디스에 넣어주기
        
        // 방 번호 1개씩 갖다쓰기 위해 초기화
        var RoomMaxNumber = 100; //TODO: Config로 받기 
        for (int idx = 1; idx <= RoomMaxNumber; idx++)
        {
            _roomNoQueue.Enqueue(idx);
        }
        
        
        _matchWorker = new System.Threading.Thread(this.RunMatching);
        _matchWorker.Start();

    }
    
    public ErrorCode AddUserReqQueue(string userId)
    {
        ErrorCode errorCode = ErrorCode.None;
        try
        {
            _reqQueue.Enqueue(userId);
            return errorCode;
        }
        catch (Exception ex)
        {
            errorCode = ErrorCode.Matching_Fail_Notqueued;
            return errorCode;
        }
        
    }

    public (bool, MatchingData) GetCompleteMatchingDic(string userId)
    {
        // _completeMatchingDic에 있으면 소켓 서버 연결 가능
        if (_completeMatchingDic.TryGetValue(userId, out var outValue)) 
        {
            MatchingData matchingData = new();
            matchingData.ServerAddress = outValue;
            matchingData.RoomNumber = _roomNoQueue.Dequeue();

            return (true, matchingData);
        }
        Console.WriteLine($@"[Matching Btn Error] ErrorCode:{ErrorCode.MatchingFailNotExistCompleteMatchingDic}, UserID = {userId}");
        return (false, null);
        
    }

    void RunMatching()
    {
        while (true)
        {
            try
            {

                if (_reqQueue.Count < 2)
                {
                    System.Threading.Thread.Sleep(1);
                    continue;
                }
                else
                {
                    // 큐에서 2명을 뽑기
                    if (!_reqQueue.TryDequeue(out matchingId1))
                    {
                        Console.WriteLine($"[Matching Error] ErrorCode:{ErrorCode.MatchingFailReqQueueNotExist}");
                    }
                    if (!_reqQueue.TryDequeue(out matchingId2))
                    {
                        Console.WriteLine($"[Matching Error] ErrorCode:{ErrorCode.MatchingFailReqQueueNotExist}");
                    }
                    
                    Console.WriteLine($"ID_1: {matchingId1}, ID_2: {matchingId2}");
                    
                    
                    
                    // 레디스에서 서버 정보 받기
                    // var roomSocketServerAddress = "";
                    var redis = new CloudStructures.Structures.RedisList<string>(_redisConn, key, defaultExpiry);
                    var roomSocketServerAddress = redis.LeftPopAsync().Result;
                    if (roomSocketServerAddress.HasValue == false)
                    {
                        Console.WriteLine($"[Matching Error] ErrorCode:{ErrorCode.MatchingFailRedisNotExist}");
                    }
                    
                    // 게임 서버 정보를 두 친구에게 전달하기 위해 _completeMatchingDic에다가 (userid, 서버주소)형태로 넣기
                    _completeMatchingDic.TryAdd(matchingId1, roomSocketServerAddress.Value);
                    _completeMatchingDic.TryAdd(matchingId2, roomSocketServerAddress.Value);
                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Matching Error] ErrorCode:{ErrorCode.MatchingFailException}");
            }
        }
    }



    public void Dispose()
    {
        Console.WriteLine("MatchWoker 소멸자 호출");
    }
}