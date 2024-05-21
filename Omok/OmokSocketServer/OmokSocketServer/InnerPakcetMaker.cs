using MemoryPack;
using OmokShareProject;

namespace OmokSocketServer;

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
    // TODO 이 패킷 받는 곳이 없는 것 같은데 지금
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
    
    // 방번호 적으면  (시간초과로 턴 넘겨주는 패킷id, 방번호) 담긴 패킷 만들어줌
    public static MemoryPackBinaryRequestInfo MakeNTFInnerTimeTurnChange(int roomNumber)
    {
        var packet = new PKTInternalNtfTimeTurnChange()
        {
            RoomNumber = roomNumber
        };

        var sendData = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendData, PACKETID.NTF_IN_TIME_TURN_CHANGE);

        var memoryPackPacket = new MemoryPackBinaryRequestInfo(null);
        memoryPackPacket.Data = sendData;
        memoryPackPacket.SessionID = "";
        // TODO 서버만 쓰는 건데 없으면 안되나
        return memoryPackPacket;
    }

}
   
// 서버에서만 쓰는 패킷들 

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
[MemoryPackable]
public partial class PKTInternalNtfTimeTurnChange : PkHeader
{
    public int RoomNumber { get; set; }
}
