using OmokSocketServer.Manager;

namespace OmokSocketServer.Packet;

public class PKHandler
{
    // 여기서 로그 찍자
    public static Func<string, byte[], bool> NetSendFunc;
    public static Action<MemoryPackBinaryRequestInfo> DistributeInnerPacket;

    protected UserManager _userMgr = null;

    protected RoomManager _roomMgr = null;
    


    public void Init(UserManager userMgr, RoomManager roomManager)
    {
        this._userMgr = userMgr;
        this._roomMgr = roomManager;
    }           
            
    
}
