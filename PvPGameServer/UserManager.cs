using System;
using System.Collections.Generic;
using System.Linq;
using PvPGameServer.CS;


namespace PvPGameServer;

public class UserManager
{
    int _maxUserCount;
    UInt64 _userSequenceNumber = 0;

    Dictionary<string, User> _userMap = new Dictionary<string, User>();
    int _userCheckCycle;


    public void Init(int maxUserCount, int userCheckCycle)
    {
        _maxUserCount = maxUserCount;
        _userCheckCycle = userCheckCycle;
    }
    

    public ERROR_CODE AddUser(string userID, string sessionID, DateTime dateTime)
    {
        if(IsFullUserCount())
        {
            return ERROR_CODE.LOGIN_FULL_USER_COUNT;
        }

        if (_userMap.ContainsKey(sessionID))
        {
            return ERROR_CODE.ADD_USER_DUPLICATION;
        }


        ++_userSequenceNumber;
        
        var user = new User();
        user.Set(_userSequenceNumber, sessionID, userID, dateTime);
        _userMap.Add(sessionID, user);

        return ERROR_CODE.NONE;
    }

    public ERROR_CODE RemoveUser(string sessionID)
    {
        if(_userMap.Remove(sessionID) == false)
        {
            return ERROR_CODE.REMOVE_USER_SEARCH_FAILURE_USER_ID;
        }

        return ERROR_CODE.NONE;
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

    public void UserCheck(DateTime dateTime)
    {
        Console.WriteLine("user count: " + _userMap.Count);
        //TODO 일단 다 검사하는데 나눠서 검사 어케하지
        // var remainder =  dateTime.Second % _userCheckCycle;
        foreach (var user in _userMap)
        {
            Console.WriteLine("user: " + user.Key);
            var sessionid = user.Key;
            var diffSpan = new TimeSpan(user.Value.LastAccessTime.Ticks - dateTime.Ticks);
            if (diffSpan.TotalSeconds is > 10 or < -10)
            {
                RemoveUser(sessionid);
            }
        }
        return;
    }
}

public class User
{
    UInt64 SequenceNumber = 0;
    string SessionID;
   
    public int RoomNumber { get; private set; } = -1;
    string UserID;

    public DateTime LastAccessTime { get; set; }
            
    public void Set(UInt64 sequence, string sessionID, string userID, DateTime lastAccessTime)
    {
        SequenceNumber = sequence;
        SessionID = sessionID;
        UserID = userID;
        LastAccessTime = lastAccessTime;
    }                   
    
    public bool IsConfirm(string netSessionID)
    {
        return SessionID == netSessionID;
    }

    public string ID()
    {
        return UserID;
    }

    public void EnteredRoom(int roomNumber)
    {
        RoomNumber = roomNumber;
    }

    public void LeaveRoom()
    {
        RoomNumber = -1;
    }

    public bool IsStateLogin() { return SequenceNumber != 0; }

    // 방 번호 -1이면 true
    public bool IsStateRoom() { return RoomNumber != -1; }
}

