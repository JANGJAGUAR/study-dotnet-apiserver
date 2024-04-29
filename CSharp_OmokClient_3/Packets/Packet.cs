using System;
using System.Collections.Generic;
using CSCommon;
using MemoryPack;

namespace OmokClient.Packets;

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

    // public static void WritePacketId(byte[] data, UInt16 packetId)
    // {
    //     FastBinaryWrite.UInt16(data, PacketHeaderMemoryPackStartPos + 2, packetId);
    // }
    
    // 패킷 헤더 위치 부터 [사이즈, ID, 타입] 읽어오기
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
    
    // 패킷 헤더 위치에 [사이즈, ID, 타입] 쓰기
    public void Write(byte[] binary)
    {
        var pos = PacketHeaderMemoryPackStartPos;

        FastBinaryWrite.UInt16(binary, pos, TotalSize);
        pos += 2;

        FastBinaryWrite.UInt16(binary, pos, Id);
        pos += 2;

        binary[pos] = Type;
        pos += 1;
    }


    // public void DebugConsolOutHeaderInfo()
    // {
    //     Console.WriteLine("DebugConsolOutHeaderInfo");
    //     Console.WriteLine("TotalSize : " + TotalSize);
    //     Console.WriteLine("Id : " + Id);
    //     Console.WriteLine("Type : " + Type);
    // }
}

// 메모리팩 헤더 정보 [사이즈, ID, 타입]
[MemoryPackable]
public partial class PkHeader
{
    public UInt16 TotalSize { get; set; } = 0;
    public UInt16 Id { get; set; } = 0;
    public byte Type { get; set; } = 0;
}


// 로그인 요청
[MemoryPackable]
public partial class PKTReqLogin : PkHeader
{
    public string UserID { get; set; }
    public string AuthToken { get; set; }
}

// 로그인 응답
[MemoryPackable]
public partial class PKTResLogin : PkHeader
{
    public short Result { get; set; }
}

// 방 입장 요청
[MemoryPackable]
public partial class PKTReqRoomEnter : PkHeader
{
    public int RoomNumber { get; set; }
}

// 방 입장 응답
[MemoryPackable]
public partial class PKTResRoomEnter : PkHeader
{
    public short Result { get; set; }
}

// 방의 유저 목록 알림
[MemoryPackable]
public partial class PKTNtfRoomUserList : PkHeader
{
    public List<string> UserIDList { get; set; } = new List<string>();
}

// 방의 새 유저 알림
[MemoryPackable]
public partial class PKTNtfRoomNewUser : PkHeader
{
    public string UserID { get; set; }
}

// 방 떠나기 응답 (요청은 불필요 (무조건 나감))
[MemoryPackable]
public partial class PKTResRoomLeave : PkHeader
{
    public short Result { get; set; }
}

// 방 떠난 유저 알림
[MemoryPackable]
public partial class PKTNtfRoomLeaveUser : PkHeader
{
    public string UserID { get; set; }
}

// 채팅 메시지 요청 (응답은 불필요 (전달됐다고 알릴 필요 없음))
[MemoryPackable]
public partial class PKTReqRoomChat : PkHeader
{
    public string ChatMessage { get; set; }
}

// 채팅 메시지 알림
[MemoryPackable]
public partial class PKTNtfRoomChat : PkHeader
{
    public string UserID { get; set; }

    public string ChatMessage { get; set; }
}

// 게임 시작 요청
[MemoryPackable]
public partial class PKTGameStart : PkHeader
{
    public string UserID { get; set; }
}