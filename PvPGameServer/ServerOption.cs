
namespace PvPGameServer;

public class ServerOption
{
    public int ServerUniqueID { get; set; }

    public string Name { get; set; }

    public int MaxConnectionNumber { get; set; }

    public int Port { get; set; }

    public int MaxRequestLength { get; set; }

    public int ReceiveBufferSize { get; set; }

    public int SendBufferSize { get; set; }

    // 방 개수
    public int RoomMaxCount { get; set; } = 0;

    // 한 방의 최대 인원
    public int RoomMaxUserCount { get; set; } = 0;

    public int RoomStartNumber { get; set; } = 0;   
    
    public int UserCheckCycle { get; set; } = 0;

}    
