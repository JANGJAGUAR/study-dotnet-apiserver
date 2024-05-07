
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
    
    // 총 인원수 = 방 개수 * 한 방의 최대 인원 (1000 x 2 = 2000)

    // 방 시작 번호
    public int RoomStartNumber { get; set; } = 0;
    
    // 주기적으로 방 체크 시 한번에 체크할 수량 (1000 / 4 = 250)
    public int RoomCheckMaxCount { get; set; } = 0;
    // 주기적으로 유저 체크 시 한번에 체크할 수량 (2000 / 4 = 500)
    public int UserCheckMaxCount { get; set; } = 0;
    
    // 하트비트 주기 (ms)
    public int HeartbeatInterval { get; set; } = 0;
    
    

}    
