using System;


namespace PvPGameServer;

public class PKHandler
{
    // 여기서 로그 찍자
    public static Func<string, byte[], bool> NetSendFunc;
    public static Action<MemoryPackBinaryRequestInfo> DistributeInnerPacket;

    protected UserManager _userMgr = null;
    
    


    public void Init(UserManager userMgr)
    {
        this._userMgr = userMgr;
    }           
            
    
}
