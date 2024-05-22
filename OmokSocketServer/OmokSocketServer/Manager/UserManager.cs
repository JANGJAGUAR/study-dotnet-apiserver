using OmokShareProject;

namespace OmokSocketServer.Manager;

public class UserManager
{
    int _maxUserCount;
    UInt64 _userSequenceNumber = 0;

    Dictionary<string, User> _userMap = new Dictionary<string, User>();
    private List<User> _userList = new List<User>();

    

    private Queue<int> unregisteredUserNumber = new Queue<int>();
    private int _userCheckMaxCount = 0;
    
    private int _start = 0;
    private int _end = 0;
    
    
    public void Init(int maxUserCount, int userCheckMaxCount)
    {
        // 최대 인원 수 설정
        _maxUserCount = maxUserCount;
        // 한번에 체크할 인원 수 설정
        _userCheckMaxCount = userCheckMaxCount;
        // 유저 리스트 초기화 + 미등록 번호 넣어놓기
        for (int i = 0; i < _maxUserCount; i++)
        {
            var user = new User();
            // user.Set(_userSequenceNumber, sessionID, userID, dateTime);
            _userList.Add(user);
            unregisteredUserNumber.Enqueue(i);
        }
    }

    public ErrorCode AddUser(string userID, string sessionID, DateTime dateTime)
    {
        if(IsFullUserCount())
        {
            return ErrorCode.LOGIN_FULL_USER_COUNT;
        }

        if (_userMap.ContainsKey(sessionID))
        {
            return ErrorCode.ADD_USER_DUPLICATION;
        }


        ++_userSequenceNumber;
        
        var user = new User();
        user.Set(_userSequenceNumber, sessionID, userID, dateTime);
        _userMap.Add(sessionID, user);
        
        // 미등록 번호 중에 하나에 할당
        var userListIdx = unregisteredUserNumber.Dequeue();
        user.UserListIdx = userListIdx;
        _userList[userListIdx] = user;

        return ErrorCode.NONE;
    }

    public ErrorCode RemoveUser(string sessionID)
    {
        if(_userMap.Remove(sessionID) == false)
        {
            return ErrorCode.REMOVE_USER_SEARCH_FAILURE_USER_ID;
        }


        var removeIdx = GetUser(sessionID).UserListIdx;
        var user = new User();
        _userList[removeIdx] = user;
        unregisteredUserNumber.Enqueue(removeIdx);

        return ErrorCode.NONE;
    }

    public User GetUser(string sessionID)
    {
        User user = null;
        _userMap.TryGetValue(sessionID, out user);
        return user;
    }

    bool IsFullUserCount()
    {
        return _maxUserCount <= _userMap.Count();
    }

    public void CheckUserList()
    {
        //끝 설정
        _end = _start + _userCheckMaxCount;
        if (_end >= _maxUserCount)
        {
            _end = _maxUserCount;
        }
        
        for (int idx = _start; idx < _end; idx++)
        {
            //TODO 여기서 시간을 받아도 되는걸까? 적어도 for문 바깥?
            var nowTime = DateTime.Now;
            
            // HeartBeat 패킷 왔는지 체크
            var checkUser = _userList[idx];
            var diffStoneSpan = new TimeSpan(nowTime.Ticks - checkUser.LastAccessTime.Ticks);
            if (diffStoneSpan.TotalSeconds > 10)
            {
                //TODO 세션 ID를 public 에 get set 으로 둬도 괜찮은가
                //TODO ***** 코드 변경
                RemoveUser(checkUser.SessionID);
            }
        }
        
        //시작 설정
        _start += _userCheckMaxCount;
        if (_start >= _maxUserCount)
        {
            _start = 0;
        }
    }
}



