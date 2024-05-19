using MemoryPack;
using OmokSocketServer.CS;

namespace OmokSocketServer.Manager;

public class Room
{
    public static Action<MemoryPackBinaryRequestInfo> DistributeInnerPacket;
    public const int InvalidRoomNumber = -1;


    public bool IsWaiting { get; set; } = false;
    public string WaitingSID { get; set; }
    
    
    
    public int Index { get; private set; }
    public int Number { get; private set; }
    
    //[게임 관련]
    public string NowTurnSID { get; set; }
    public string NextTurnSID { get; set; }
    public bool IsGameStart { get; set; }
    //TODO 게임 시작 시 바꿔주기
    public DateTime GameStartTime { get; set; }
    //TODO 게임 시작 시 바꿔주기
    public DateTime LastPutStoneTime { get; set; }
    //TODO 돌 뒀을시 바꿔주기
    

    int _maxUserCount;
    
    List<RoomUser> _roomUserList = new List<RoomUser>();
    // TODO 얘는 풀링 안해도 되나 
    
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
        if(GetRoomUser(userID) != null)
        {
            return false;
        }

        var roomUser = new RoomUser();
        roomUser.Set(userID, netSessionID);
        _roomUserList.Add(roomUser);

        return true;
    }

    public void RemoveUser(string netSessionID)
    {
        var index = _roomUserList.FindIndex(x => x.NetSessionID == netSessionID);
        _roomUserList.RemoveAt(index);
    }

    
    public bool RemoveUser(RoomUser user)
    {
        return _roomUserList.Remove(user);
    }

    // id로 유저 찾아서 출력
    public RoomUser GetRoomUser(string userID)
    {
        return _roomUserList.Find(x => x.UserID == userID);
    }

    // 세션 id로 유저 찾아서 출력
    public RoomUser GetRoomUserByNetSessionId(string netSessionID)
    {
        return _roomUserList.Find(x => x.NetSessionID == netSessionID);
    }

    // 유저 인원 수 출력 
    public int CurrentUserCount()
    {
        return _roomUserList.Count();
    }

    public void NotifyPacketUserList(string userNetSessionID)
    {
        var packet = new PKTNtfRoomUserList();
        foreach (var user in _roomUserList)
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
        foreach(var user in _roomUserList)
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
                var internalPacket = InnerPakcetMaker.MakeNTFInnerTimeTurnChange(Number);                
                DistributeInnerPacket(internalPacket);
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
                // 그냥 방 빼버리면 안되나
                RemoveUser(NowTurnSID);
                RemoveUser(NextTurnSID);
                IsGameStart = false;
            }
        }
    }
    
}


public class RoomUser
{
    public string UserID { get; private set; }
    public string NetSessionID { get; private set; }
    
    public int TurnCount { get; set; }
    //TODO 초기화 필요

    public void Set(string userID, string netSessionID)
    {
        UserID = userID;
        NetSessionID = netSessionID;
    }
}
