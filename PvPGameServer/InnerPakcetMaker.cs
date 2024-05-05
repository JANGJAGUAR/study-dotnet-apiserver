using MemoryPack;
using System;
using PvPGameServer.CS;


namespace PvPGameServer;

public class InnerPakcetMaker
{
    // 세션id, 방번호, 유저id 적으면 (방 떠나는 패킷id, 세션id, 방번호, 유저id)담긴 패킷 만들어줌
    public static MemoryPackBinaryRequestInfo MakeNTFInnerRoomLeavePacket(string sessionID, int roomNumber, string userID)
    {            
        var packet = new PKTInternalNtfRoomLeave()
        {
            RoomNumber = roomNumber,
            UserID = userID,
        };

        var sendData = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendData, PACKETID.NTF_IN_ROOM_LEAVE);
        
        var memoryPakcPacket = new MemoryPackBinaryRequestInfo(null);
        memoryPakcPacket.Data = sendData;
        memoryPakcPacket.SessionID = sessionID;
        return memoryPakcPacket;
    }

    // 연결됐는지, 세션 id 적으면 (연결됐는지 알려주는 패킷id, 세션id, 헤더사이즈?) 담긴 패킷 만들어줌
    public static MemoryPackBinaryRequestInfo MakeNTFInConnectOrDisConnectClientPacket(bool isConnect, string sessionID)
    {
        var memoryPackPacket = new MemoryPackBinaryRequestInfo(null);
        memoryPackPacket.Data = new byte[MemoryPackPacketHeadInfo.HeadSize];
        
        if (isConnect)
        {
            MemoryPackPacketHeadInfo.WritePacketId(memoryPackPacket.Data, PACKETID.NTF_IN_CONNECT_CLIENT);
        }
        else
        {
            MemoryPackPacketHeadInfo.WritePacketId(memoryPackPacket.Data, PACKETID.NTF_IN_DISCONNECT_CLIENT);
        }

        memoryPackPacket.SessionID = sessionID;
        return memoryPackPacket;
    }
    
    public static MemoryPackBinaryRequestInfo MakeNTFInner(bool isConnect, string sessionID)
    {
        var memoryPackPacket = new MemoryPackBinaryRequestInfo(null);
        memoryPackPacket.Data = new byte[MemoryPackPacketHeadInfo.HeadSize];
        
        if (isConnect)
        {
            MemoryPackPacketHeadInfo.WritePacketId(memoryPackPacket.Data, PACKETID.NTF_IN_CONNECT_CLIENT);
        }
        else
        {
            MemoryPackPacketHeadInfo.WritePacketId(memoryPackPacket.Data, PACKETID.NTF_IN_DISCONNECT_CLIENT);
        }

        memoryPackPacket.SessionID = sessionID;
        return memoryPackPacket;
    }

}
   

[MemoryPackable]
public partial class PKTInternalNtfRoomLeave : PkHeader
{
    public int RoomNumber { get; set; }
    public string UserID { get; set; }
}

[MemoryPackable]
public partial class PKTServerTimer : PkHeader
{
    public DateTime dateTime { get; set; }
}

