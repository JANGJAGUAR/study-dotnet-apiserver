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

    System.Threading.Thread _reqWorker = null;
    System.Threading.Thread _completeWorker = null;
    ConcurrentQueue<string> _reqQueue = new();
    
    Queue<int> _roomNoQueue = new();

    // key는 유저ID
    ConcurrentDictionary<string, MatchingData> _completeMatchingDic = new();

    
    private string matchingId1;
    private string matchingId2;
    
    string _redisAddress = "";
    public RedisConnection _redisConn;
    TimeSpan defaultExpiry = TimeSpan.FromDays(1);
    
    string _matchingToSocketKey = "MatchingToSocketData";
    string _socketToMatchingKey = "SocketToMatchingData";
    
    CloudStructures.Structures.RedisList<RedisReqIdData> matchingToSocketRedis;
    CloudStructures.Structures.RedisList<RedisMatchingData> socketToMatchingRedis;
    
    MatchingData _matchingData = new MatchingData();
    MatchingData _newMatchingData = new MatchingData();
    
    public MatchWorker(IOptions<DbConfig> dbConfig)
    {
        Console.WriteLine("MatchWoker 생성자 호출");
        _redisAddress = dbConfig.Value.RedisAddress;
        _matchingToSocketKey = dbConfig.Value.MatchingToSocketKey;
        _socketToMatchingKey = dbConfig.Value.SocketToMatchingKey;

        // Redis 연결 및 초기화
        RedisConfig config = new("default", _redisAddress);
        _redisConn = new RedisConnection(config);
        matchingToSocketRedis = new CloudStructures.Structures.RedisList<RedisReqIdData>(_redisConn, _matchingToSocketKey, defaultExpiry);
        socketToMatchingRedis = new CloudStructures.Structures.RedisList<RedisMatchingData>(_redisConn, _socketToMatchingKey, defaultExpiry);
        
        // // 방 번호 1개씩 갖다쓰기 위해 초기화
        // var RoomMaxNumber = 100; //TODO: Config로 받기 
        // for (int idx = 1; idx <= RoomMaxNumber; idx++)
        // {
        //     _roomNoQueue.Enqueue(idx);
        // }
        //TODO: 이거 소켓에서 하는거
        
        
        _reqWorker = new System.Threading.Thread(this.RunMatching);
        _reqWorker.Start();

        _completeWorker = new System.Threading.Thread(this.RunMatchingComplete);
        _completeWorker.Start();

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
            return (true, outValue);
        }
        Console.WriteLine($@"[Matching Btn Error] ErrorCode:{ErrorCode.MatchingFailNotExistCompleteMatchingDic}, UserID = {userId}");
        
        _newMatchingData.ServerAddress = "";
        _newMatchingData.RoomNumber = 0;
        //TODO: 풀링때문에 이렇게 했는데 밑에거랑 상호참조 안되는지 체크
        return (false, _newMatchingData);
        
    }

    // Queue의 ID 2개를 레디스로 보내주기 위한 Update 함수
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
                    
                    Console.WriteLine($"[TryDequeue] ID_1: {matchingId1}, ID_2: {matchingId2}");
                    
                    // 레디스(매칭->소켓)에 2명 정보를 넣기
                    RedisReqIdData reqIdData = new RedisReqIdData();
                    reqIdData.UserId1 = matchingId1;
                    reqIdData.UserId2 = matchingId2;
                    matchingToSocketRedis.RightPushAsync(reqIdData);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Matching Error] ErrorCode:{ErrorCode.MatchingFailException}");
            }
        }
    }
    
    // 레디스에서 매칭이 끝난 ID 2개를 Dic에 넣어주는 함수 
    void RunMatchingComplete()
    {
        while (true)
        {
            try
            {
                // 레디스(소켓->매칭)에 있는 정보를 받아오기
                var redisMatchingData = socketToMatchingRedis.LeftPopAsync().Result;
                if (redisMatchingData.HasValue == false)
                {
                    Console.WriteLine($"[Matching Error] ErrorCode:{ErrorCode.MatchingFailRedisNotExist}");
                    //TODO: 두 함수 에러 코드 다 다시 쓰기
                }
                
                _matchingData.ServerAddress = redisMatchingData.Value.ServerAddress;
                _matchingData.RoomNumber = redisMatchingData.Value.RoomNumber;
                    
                // 게임 서버 정보를 두 ID에게 전달하기 위해 _completeMatchingDic에다가 (userid, 매칭 데이터)형태로 넣기
                _completeMatchingDic.TryAdd(redisMatchingData.Value.UserId1, _matchingData);
                _completeMatchingDic.TryAdd(redisMatchingData.Value.UserId2, _matchingData);
            }
            catch (Exception ex)
            {

            }
        }        
    }



    public void Dispose()
    {
        Console.WriteLine("MatchWoker 소멸자 호출");
    }
}