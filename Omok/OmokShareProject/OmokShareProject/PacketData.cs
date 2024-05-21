using MemoryPack;

namespace OmokShareProject;
public struct MemoryPackPacketHeadInfo
{
    const int PacketHeaderMemoryPackStartPos = 1;
    public const int HeadSize = 6;

    public UInt16 TotalSize;
    public UInt16 Id;
    public byte Type;
    
    // 전체크기 정보 = (시작 위치) + 패킷 헤더 위치
    public static UInt16 GetTotalSize(byte[] data, int startPos)
    {
        return FastBinaryRead.UInt16(data, startPos + PacketHeaderMemoryPackStartPos);
    }

    // id 위치(패킷 헤더 위치+2)에 packetId 값 넣기
    public static void WritePacketId(byte[] data, PACKETID packetId)
    {
        FastBinaryWrite.UInt16(data, PacketHeaderMemoryPackStartPos + 2, (UInt16)packetId);
    }
    
    // 읽어오기 : 패킷 헤더 위치 부터 [사이즈, ID, 타입]
    public void Read(byte[] headerData)
    {
        var pos = PacketHeaderMemoryPackStartPos;

        TotalSize = FastBinaryRead.UInt16(headerData, pos);
        pos += 2;

        Id = FastBinaryRead.UInt16(headerData, pos);
        pos += 2;

        Type = headerData[pos];
        pos += 1;
    }
    
    // 쓰기 : 패킷 헤더 위치에 [사이즈, ID, 타입]
    public void WriteHeader(byte[] binary)
    {
        var pos = PacketHeaderMemoryPackStartPos;
    
        FastBinaryWrite.UInt16(binary, pos, TotalSize);
        pos += 2;
    
        FastBinaryWrite.UInt16(binary, pos, Id);
        pos += 2;
    
        binary[pos] = Type;
    }
    public static void Write(byte[] packetData, PACKETID packetId, byte type = 0)
    {
        var pos = PacketHeaderMemoryPackStartPos;

        FastBinaryWrite.UInt16(packetData, pos, (UInt16)packetData.Length);
        pos += 2;

        FastBinaryWrite.UInt16(packetData, pos, (UInt16)packetId);
        pos += 2;

        packetData[pos] = type;
    }
}

// 메모리팩 헤더 정보 [사이즈, ID, 타입]
[MemoryPackable]
public partial class PkHeader
{
    public UInt16 TotalSize { get; set; } = 0;
    public UInt16 Id { get; set; } = 0;
    public byte Type { get; set; } = 0;
}


// 로그인 요청 패킷 (ID, 토큰 전달)
[MemoryPackable]
public partial class PKTReqLogin : PkHeader
{
    public string UserID { get; set; }
    public string AuthToken { get; set; }
    
    public DateTime Datetime { get; set; }
}

// 로그인 응답 (로그인 결과 반환)
[MemoryPackable]
public partial class PKTResLogin : PkHeader
{
    public short Result { get; set; }
}

// 입장 불가 알림 (로그인 결과 반환??)
[MemoryPackable]
public partial class PKNtfMustClose : PkHeader
{
    public short Result { get; set; }
}

// 방 입장 요청 (방 번호 전달)
[MemoryPackable]
public partial class PKTReqRoomEnter : PkHeader
{
    public int RoomNumber { get; set; }
}

// 방 입장 응답 (입장 결과 반환)
[MemoryPackable]
public partial class PKTResRoomEnter : PkHeader
{
    public short Result { get; set; }
}

// 방의 유저 목록 알림 (유저 목록 공지)
[MemoryPackable]
public partial class PKTNtfRoomUserList : PkHeader
{
    public List<string> UserIDList { get; set; } = new List<string>();
}

// 방의 새 유저 알림 (유저 id 공지)
[MemoryPackable]
public partial class PKTNtfRoomNewUser : PkHeader
{
    public string UserID { get; set; }
}

// 방 떠나기 응답 (떠나기 결과 반환)     // (요청은 불필요 (무조건 나감))
[MemoryPackable]
public partial class PKTResRoomLeave : PkHeader
{
    public short Result { get; set; }
}

// 방 떠난 유저 알림 (유저 id 공지)
[MemoryPackable]
public partial class PKTNtfRoomLeaveUser : PkHeader
{
    public string UserID { get; set; }
}

// 방 채팅 메시지 요청 (메시지 전달)       // (응답은 불필요 (전달됐다고 알릴 필요 없음))
[MemoryPackable]
public partial class PKTReqRoomChat : PkHeader
{
    public string ChatMessage { get; set; }
}

// 방 채팅 메시지 알림 (메시지 공지)
[MemoryPackable]
public partial class PKTNtfRoomChat : PkHeader
{
    public string UserID { get; set; }

    public string ChatMessage { get; set; }
}

// 방 채팅 메시지 요청 (메시지 전달)       // (응답은 불필요 (전달됐다고 알릴 필요 없음))
[MemoryPackable]
public partial class PKTReqLobbyChat : PkHeader
{
    public string ChatMessage { get; set; }
}

// 로비 채팅 메시지 알림 (메시지 공지)
[MemoryPackable]
public partial class PKTNtfLobbyChat : PkHeader
{
    public string UserID { get; set; }

    public string ChatMessage { get; set; }
}


// // 매칭 시작 요청 (id 전달)          // 6주차 매칭서버
// [MemoryPackable]
// public partial class PKTMatchStart : PkHeader
// {
//     public string UserID { get; set; }
// }

// 게임 시작 요청 (id 전달)
[MemoryPackable]
public partial class PKTReqGameStart : PkHeader
{
    public string UserID { get; set; }
}

// 게임 시작 응답 (id(시작 사람) 반환)
[MemoryPackable]
public partial class PKTResGameStart : PkHeader
{
    public string StartID { get; set; }
}

// 돌 두기 요청 (좌표 전달)
[MemoryPackable]
public partial class PKTReqPutStone : PkHeader
{
    public int xPos { get; set; }
    public int yPos { get; set; }
}

// 돌 두기 후 응답 (둔 사람에게 둘수 있는지 반환)
[MemoryPackable]
public partial class PKTResPutStone : PkHeader
{
    public ErrorCode isAble { get; set; }
    public int xPos { get; set; }
    public int yPos { get; set; }
    public bool isWin { get; set; }
}

// 턴 넘기기 응답 (다음 사람에게 할 수 있다고 반환)
[MemoryPackable]
public partial class PKTResTurnChange : PkHeader
{
    public int xPos { get; set; }
    public int yPos { get; set; }
    public bool isLose { get; set; }
}


// // 돌 두기 후 좌표 알림 (둔 사람과 좌표를 둘다에게 공지)
// [MemoryPackable]
// public partial class PKTResPutStoneInfo : PkHeader
// {
//     public string userID { get; set; }
//     public int xPos { get; set; }
//     public int yPos { get; set; }
// }

// // 게임 종료 후 응답 (id(다음 사람) 반환)
// [MemoryPackable]
// public partial class PKTResGameEnd : PkHeader
// {
//     public string WinID { get; set; }
// }

// 에러 알림 응답 (에러코드 반환)
[MemoryPackable]
public partial class PKTNtfError : PkHeader
{
    public ErrorCode Error { get; set; }
}

// 클라이언트 HeartBeat (에러코드 반환)
[MemoryPackable]
public partial class PKTClientHeartBeat : PkHeader
{
    public DateTime dateTime { get; set; }
}

// 시간 지속 시 턴 넘기기 (턴 넘긴 횟수 반환)
[MemoryPackable]
public partial class PKTResTimeTurnChange : PkHeader
{
    public int turnCount { get; set; }
}

// 시간 초과로 게임 종료 (이겼는지 졌는지 반환)
[MemoryPackable]
public partial class PKTResTimeEndGame : PkHeader
{
    public bool IsWin { get; set; }
}


// // 게임 시작 알림 (id 공지)
// [MemoryPackable]
// public partial class PKTNtfGameStart : PkHeader
// {
//     public string UserID { get; set; }
// }