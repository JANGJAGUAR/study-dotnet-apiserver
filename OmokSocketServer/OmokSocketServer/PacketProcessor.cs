using System.Threading.Tasks.Dataflow;
using OmokShareProject;
using OmokSocketServer.Manager;
using OmokSocketServer.Model;
using OmokSocketServer.Packet;
using SuperSocket.SocketBase.Logging;

namespace OmokSocketServer;

class PacketProcessor
{
    bool _isThreadRunning = false;
    System.Threading.Thread _processThread = null;

    public Func<string, byte[], bool> NetSendFunc;
    
    //receive쪽에서 처리하지 않아도 Post에서 블럭킹 되지 않는다. 
    //BufferBlock<T>(DataflowBlockOptions) 에서 DataflowBlockOptions의 BoundedCapacity로 버퍼 가능 수 지정. BoundedCapacity 보다 크게 쌓이면 블럭킹 된다
    BufferBlock<MemoryPackBinaryRequestInfo> _msgBuffer = new BufferBlock<MemoryPackBinaryRequestInfo>();

    UserManager _userMgr = new UserManager();

    RoomManager _roomMgr = new RoomManager();

    List<Room> _roomList = new List<Room>();

    Dictionary<int, Action<MemoryPackBinaryRequestInfo>> _packetHandlerMap = new Dictionary<int, Action<MemoryPackBinaryRequestInfo>>();
    PKHCommon _commonPacketHandler = new PKHCommon();
    PKHRoom _roomPacketHandler = new PKHRoom();
            

    public void CreateAndStart(RoomManager roomMgr, ServerOption serverOpt) // List<Room> roomList
    {
        var maxUserCount = serverOpt.RoomMaxCount * serverOpt.RoomMaxUserCount;
        _userMgr.Init(maxUserCount, serverOpt.UserCheckMaxCount);
        
        // _roomList = roomList;
        _roomMgr = roomMgr;
        _roomList = roomMgr.GetRoomsList();
        var minRoomNum = _roomList[0].Number;
        var maxRoomNum = _roomList[0].Number + _roomList.Count() - 1;
        
        RegistPacketHandler();

        _isThreadRunning = true;
        _processThread = new System.Threading.Thread(this.Process);
        _processThread.Start();
    }
    
    public void Destory()
    {
        MainServer.MainLogger.Info("PacketProcessor::Destory - begin");

        _isThreadRunning = false;
        _msgBuffer.Complete();

        _processThread.Join();

        MainServer.MainLogger.Info("PacketProcessor::Destory - end");
    }
  
    public static ILog MainLogger;
          
    public void InsertPacket(MemoryPackBinaryRequestInfo data)
    {
        _msgBuffer.Post(data);

    }

    
    void RegistPacketHandler()
    {
        PKHandler.NetSendFunc = NetSendFunc;
        PKHandler.DistributeInnerPacket = InsertPacket;
        
        
        _commonPacketHandler.Init(_userMgr, _roomMgr);
        _commonPacketHandler.RegistPacketHandler(_packetHandlerMap);                
        
        _roomPacketHandler.Init(_userMgr, _roomMgr);
        _roomPacketHandler.SetRooomList(_roomList);
        _roomPacketHandler.RegistPacketHandler(_packetHandlerMap);
    }

    void Process()
    {
        while (_isThreadRunning)
        {
            //System.Threading.Thread.Sleep(64); //테스트 용
            try
            {
                var packet = _msgBuffer.Receive();

                var header = new MemoryPackPacketHeadInfo();
                header.Read(packet.Data);

                if (_packetHandlerMap.ContainsKey(header.Id))
                {
                    _packetHandlerMap[header.Id](packet);
                }
                /*else
                {
                    System.Diagnostics.Debug.WriteLine("세션 번호 {0}, PacketID {1}, 받은 데이터 크기: {2}", packet.SessionID, packet.PacketID, packet.BodyData.Length);
                }*/
            }
            catch (Exception ex)
            {
                if (_isThreadRunning)
                {
                    MainServer.MainLogger.Error(ex.ToString());
                }
            }
        }
    }


}
