using MemoryPack;
using System;
using System.Collections.Generic;
using System.Linq;
using PvPGameServer.CS;


namespace PvPGameServer;

public class Room
{
    public const int InvalidRoomNumber = -1;


    public bool IsWaiting { get; set; } = false;
    public string WaitingSID { get; set; }
    
    public string NowTurnSID { get; set; }
    public string NextTurnSID { get; set; }
    public int Index { get; private set; }
    public int Number { get; private set; }

    public bool IsGameStart { get; set; }
    //TODO 게임 시작 시 바꿔주기
    public DateTime GameStartTime { get; set; }
    //TODO 게임 시작 시 바꿔주기
    public DateTime LastPutStoneTime { get; set; }
    //TODO 돌 뒀을시 바꿔주기
    

    int _maxUserCount;
    
    List<RoomUser> _userList = new List<RoomUser>();

    public static Func<string, byte[], bool> NetSendFunc;


    public void Init(int index, int number, int maxUserCount)
    {
        Index = index;
        Number = number;
        _maxUserCount = maxUserCount;
    }

    public bool AddUser(string userID, string netSessionID)
    {
        // 이미 이id가 있으면
        if(GetUser(userID) != null)
        {
            return false;
        }

        var roomUser = new RoomUser();
        roomUser.Set(userID, netSessionID);
        _userList.Add(roomUser);

        return true;
    }

    public void RemoveUser(string netSessionID)
    {
        var index = _userList.FindIndex(x => x.NetSessionID == netSessionID);
        _userList.RemoveAt(index);
    }

    
    public bool RemoveUser(RoomUser user)
    {
        return _userList.Remove(user);
    }

    // id로 유저 찾아서 출력
    public RoomUser GetUser(string userID)
    {
        return _userList.Find(x => x.UserID == userID);
    }

    // 세션 id로 유저 찾아서 출력
    public RoomUser GetUserByNetSessionId(string netSessionID)
    {
        return _userList.Find(x => x.NetSessionID == netSessionID);
    }

    // 유저 인원 수 출력 
    public int CurrentUserCount()
    {
        return _userList.Count();
    }

    public void NotifyPacketUserList(string userNetSessionID)
    {
        var packet = new PKTNtfRoomUserList();
        foreach (var user in _userList)
        {
            packet.UserIDList.Add(user.UserID);
        }

        var sendPacket = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NTF_ROOM_USER_LIST);
        
        NetSendFunc(userNetSessionID, sendPacket);
    }

    public void NofifyPacketNewUser(string newUserNetSessionID, string newUserID)
    {
        var packet = new PKTNtfRoomNewUser();
        packet.UserID = newUserID;
        
        var sendPacket = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NTF_ROOM_NEW_USER);
        
        Broadcast(newUserNetSessionID, sendPacket);
    }

    public void NotifyPacketLeaveUser(string userID)
    {
        if(CurrentUserCount() == 0)
        {
            return;
        }

        var packet = new PKTNtfRoomLeaveUser();
        packet.UserID = userID;
        
        var sendPacket = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NTF_ROOM_LEAVE_USER);
      
        Broadcast("", sendPacket);
    }

    public void Broadcast(string excludeNetSessionID, byte[] sendPacket)
    {
        foreach(var user in _userList)
        {
            if(user.NetSessionID == excludeNetSessionID)
            {
                continue;
            }

            NetSendFunc(user.NetSessionID, sendPacket);
        }
    }

    // 주기적으로 돌 뒀는지 체크
    public void RoomPutStoneCheck(DateTime dateTime)
    {
        if (IsGameStart)
        {
            // 착수 30초 체크
            var diffStoneSpan = new TimeSpan(dateTime.Ticks - LastPutStoneTime.Ticks);
            if (diffStoneSpan.TotalSeconds > 30)
            {
                //TODO *** 턴 넘기기
                //해당 유저의 turnCount++;(이거 게임 입장 시 초기화)
                
                //TODO ** 이건 딴데서 처리해도 될듯 
                //if (그사람.턴 카운트>6) => 게임 종료 하면서 패배처리
            }
        }
    }
    
    // 주기적으로 게임 지속 되었는지 체크
    public void RoomGameCheck(DateTime dateTime)
    {
        if (IsGameStart)
        {
            // 오랜 시간 게임 지속되었는지 체크
            var diffGameSpan = new TimeSpan(dateTime.Ticks - GameStartTime.Ticks);
            if (diffGameSpan.TotalMinutes > 60)
            {
                //TODO *** 게임 무효화
                // 게임 무효화라고 룸정보에 표시하기
                
                //TODO ** 이건 딴데서 처리해도 될듯 
                //if(무효화) => 무효화 패킷 만들자 (클라 돌둔거 초기화, 클라 턴 초기화, 서버돌둔거 초기화, 게임 시작 및 기다린 사람등 초기화, 승패 건들x)
            }
        }
    }
    
}


public class RoomUser
{
    public string UserID { get; private set; }
    public string NetSessionID { get; private set; }

    public void Set(string userID, string netSessionID)
    {
        UserID = userID;
        NetSessionID = netSessionID;
    }
}
