using MemoryPack;
using OmokShareProject;

namespace OmokGameClient;

public partial class OmokClient : Form
{
    // RES들 처리 하는 곳
    
    // Dictionary<PACKETID, Action<byte[]>> _packetFuncDic = new Dictionary<PACKETID, Action<byte[]>>();
    
    //TODO: Login 응답 받을때 매칭 서버 주소도 같이 넘겨주기
    
    Dictionary<PACKETID, Action<byte[]>> _packetFuncDic = new Dictionary<PACKETID, Action<byte[]>>();

    void SetPacketHandler()
    {
        // 패킷에 쓰는 함수들 모아두기
        _packetFuncDic.Add(PACKETID.RES_ROOM_ENTER, PacketProcess_RoomEnterResponse);
        _packetFuncDic.Add(PACKETID.NTF_ROOM_USER_LIST, PacketProcess_RoomUserListNotify);
        _packetFuncDic.Add(PACKETID.NTF_ROOM_NEW_USER, PacketProcess_RoomNewUserNotify);
        _packetFuncDic.Add(PACKETID.RES_ROOM_LEAVE, PacketProcess_RoomLeaveResponse);
        _packetFuncDic.Add(PACKETID.NTF_ROOM_LEAVE_USER, PacketProcess_RoomLeaveUserNotify);
        _packetFuncDic.Add(PACKETID.NTF_ROOM_CHAT, PacketProcess_RoomChatNotify);
        _packetFuncDic.Add(PACKETID.RES_GAME_START, PacketProcess_GameStartResultResponse);
        _packetFuncDic.Add(PACKETID.RES_PUT_STONE, PacketProcess_PutStoneResponse);
        _packetFuncDic.Add(PACKETID.RES_TURN_CHANGE, PacketProcess_TurnChangeResponse);
        // _packetFuncDic.Add(PACKETID.RES_PUT_STONE_INFO, PacketProcess_PutStoneInfoResponse);
        // _packetFuncDic.Add(PACKETID.RES_GAME_END, PacketProcess_GameEndResultResponse);
        _packetFuncDic.Add(PACKETID.RES_TIME_TURN_CHANGE, PacketProcess_TimeTurnChangeResponse);
        _packetFuncDic.Add(PACKETID.RES_TIME_GAME_END, PacketProcess_TimeGameEndResponse);

    }
    void PacketProcess(byte[] packet)
    {
        var header = new MemoryPackPacketHeadInfo();
        header.Read(packet);

        var packetType = (PACKETID)header.Id;
        //DevLog.Write("Packet Error:  PacketID:{packet.PacketID.ToString()},  Error: {(ErrorCode)packet.Result}");
        //DevLog.Write("RawPacket: " + packet.PacketID.ToString() + ", " + PacketDump.Bytes(packet.BodyData));

        if (_packetFuncDic.ContainsKey(packetType))
        {
            _packetFuncDic[packetType](packet);
        }
        else
        {
            DevLog.Write("Unknown Packet Id: " + packetType);
        }
    }
    
    // 방 입장 결과 응답 기록
    void PacketProcess_RoomEnterResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResRoomEnter>(packetData);

        DevLog.Write($"방 입장 결과:  {(ErrorCode)responsePkt.Result}");
    }
    
    // 방 유저 리스트를 AddRoomUserList로 작성 후 기록
    void PacketProcess_RoomUserListNotify(byte[] packetData)
    {
        var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfRoomUserList>(packetData);

        for (int i = 0; i < notifyPkt.UserIDList.Count; ++i)
        {
            AddRoomUserList(notifyPkt.UserIDList[i]);
        }

        DevLog.Write($"방의 기존 유저 리스트 받음");
    }
    
    // 방에 새 유저를 받아 AddRoomUserList로 작성 후 기록
    void PacketProcess_RoomNewUserNotify(byte[] packetData)
    {
        var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfRoomNewUser>(packetData);

        AddRoomUserList(notifyPkt.UserID);

        DevLog.Write($"방에 새로 들어온 유저 받음");
    }
    
    // 방에서 나간 후 결과를 기록 (나간 사람 클라에)
    void PacketProcess_RoomLeaveResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResRoomLeave>(packetData);

        DevLog.Write($"방 나가기 결과:  {(ErrorCode)responsePkt.Result}");
    }
    
    // 방에서 나간 유저를 RemoveRoomUserList로 삭제 후 기록 (남은 사람 클라가 보게 방에)
    void PacketProcess_RoomLeaveUserNotify(byte[] packetData)
    {
        var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfRoomLeaveUser>(packetData);
        
        RemoveRoomUserList(notifyPkt.UserID);
        
        DevLog.Write($"방에서 나감: {notifyPkt.UserID}");
    }
    
    // 방에서 채팅을 AddRoomChatMessage로 작성 후 기록
    void PacketProcess_RoomChatNotify(byte[] packetData)
    {
        var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfRoomChat>(packetData);
    
        AddRoomChatMessage(notifyPkt.UserID, notifyPkt.ChatMessage);
    }
    
    // 게임 시작 시 선플레이어 응답 기록
    void PacketProcess_GameStartResultResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResGameStart>(packetData);
        
        DevLog.Write($"게임시작 !!!!");
        //TODO 여기선 적혀있는 ID랑 비교하지만 실제는 토큰으로 인증된 본인 id와 비교
        if (ID_Label.Text == responsePkt.StartID)
        {
            둘차례 = true;
            DevLog.Write($"당신은 흑돌입니다.");
        }
        else
        {
            플레이어흑돌 = false;
            DevLog.Write($"당신은 백돌입니다.");
        }
    }
    // 돌 두기 후 착수 가능한지 응답 기록
    void PacketProcess_PutStoneResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResPutStone>(packetData);

        //TODO 에러 코드 없이 턴 넘겨도 되나? + SAMSAM은 에러코드 안 쓰고 처리
        if (responsePkt.isAble==ErrorCode.GAME_SAMSAM)
        {
            DevLog.Write($"흑은 삼삼이라 둘 수 없는 자리입니다.");
        }
        else
        {
            착수(responsePkt.xPos, responsePkt.yPos);
            if (responsePkt.isWin)
            {
                var 승리색깔 = 플레이어흑돌?"흑":"백";
                DevLog.Write($"게임 종료 : 당신이 이겼습니다, "+승리색깔+" 승리");
                // 둘차례 = false;
            }
            else
            {
                DevLog.Write($"착수 완료");  
            }
        }
    }
    
    // 턴 넘어온 후 기록
    void PacketProcess_TurnChangeResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResTurnChange>(packetData);
        
        착수(responsePkt.xPos, responsePkt.yPos);
        
        if (responsePkt.isLose)
        {
            var 승리색깔 = 플레이어흑돌?"백":"흑";
            DevLog.Write($"게임 종료 : 당신이 졌습니다, "+승리색깔+" 승리");
            둘차례 = false;
        }
        else
        {
            DevLog.Write($"당신의 차례");   
        }

    }
    
    // 시간 초과로 턴 넘어온 것 기록
    void PacketProcess_TimeTurnChangeResponse(byte[] packetData)
    {
        // 턴 변경 알림 메시지 + 턴 변경 처리
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResTimeTurnChange>(packetData);
        둘차례 = !둘차례;
        DevLog.Write($"30초가 지나 턴을 바꿉니다, "+responsePkt.turnCount+"회 경고");
    }

    void PacketProcess_TimeGameEndResponse(byte[] packetData)
    {
        //게임 종료 알림 메시지
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResTimeEndGame>(packetData);
        if (responsePkt.IsWin)
        {
            DevLog.Write($"상대방의 시간 초과로 당신이 승리했습니다.");
        }
        else
        {
            DevLog.Write($"당신의 시간 초과로 패배했습니다.");
            둘차례 = false;
        }
        
    }
    

}