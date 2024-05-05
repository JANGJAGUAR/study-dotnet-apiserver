using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Windows.Forms;
using MemoryPack;
using OmokClient.CS;  //클서

namespace OmokClient;

[SupportedOSPlatform("windows10.0.177630")]
public partial class mainForm : Form
{
    // RES들 처리 하는 곳
    
    Dictionary<PACKETID, Action<byte[]>> _packetFuncDic = new Dictionary<PACKETID, Action<byte[]>>();

    void SetPacketHandler()
    {
        // 패킷에 쓰는 함수들 모아두기
        //TODO 더 있음
        _packetFuncDic.Add(PACKETID.RES_LOGIN, PacketProcess_LoginResponse);
        _packetFuncDic.Add(PACKETID.RES_ROOM_ENTER, PacketProcess_RoomEnterResponse);
        _packetFuncDic.Add(PACKETID.NTF_ROOM_USER_LIST, PacketProcess_RoomUserListNotify);
        _packetFuncDic.Add(PACKETID.NTF_ROOM_NEW_USER, PacketProcess_RoomNewUserNotify);
        _packetFuncDic.Add(PACKETID.RES_ROOM_LEAVE, PacketProcess_RoomLeaveResponse);
        _packetFuncDic.Add(PACKETID.NTF_ROOM_LEAVE_USER, PacketProcess_RoomLeaveUserNotify);
        _packetFuncDic.Add(PACKETID.NTF_ROOM_CHAT, PacketProcess_RoomChatNotify);
        
        _packetFuncDic.Add(PACKETID.NTF_LOBBY_CHAT, PacketProcess_LobbyChat);
        _packetFuncDic.Add(PACKETID.RES_GAME_START, PacketProcess_GameStartResultResponse);
        _packetFuncDic.Add(PACKETID.RES_PUT_STONE, PacketProcess_PutStoneResponse);
        _packetFuncDic.Add(PACKETID.RES_TURN_CHANGE, PacketProcess_TurnChangeResponse);
        _packetFuncDic.Add(PACKETID.RES_PUT_STONE_INFO, PacketProcess_PutStoneInfoResponse);
        _packetFuncDic.Add(PACKETID.RES_GAME_END, PacketProcess_GameEndResultResponse);
        _packetFuncDic.Add(PACKETID.RES_MATCH_USER, PacketProcess_MatchUserResponse);
        _packetFuncDic.Add(PACKETID.NTF_ROOM_RELAY, PacketProcess_RoomRelayNotify);

    }

    //TODO 여기 이해 못함
    void PacketProcess(byte[] packet)
    {
        var header = new MemoryPackPacketHeadInfo();
        header.Read(packet);

        var packetType = (PACKETID)header.Id;
        //DevLog.Write("Packet Error:  PacketID:{packet.PacketID.ToString()},  Error: {(ERROR_CODE)packet.Result}");
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

    // 로그인 결과 응답 기록
    void PacketProcess_LoginResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResLogin>(packetData);

        DevLog.Write($"로그인 결과:  {(ERROR_CODE)responsePkt.Result}");
    }


    // 방 입장 결과 응답 기록
    void PacketProcess_RoomEnterResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResRoomEnter>(packetData);

        DevLog.Write($"방 입장 결과:  {(ERROR_CODE)responsePkt.Result}");
    }
    
    // 방 유저 리스트를  AddRoomUserList로 작성 후 기록
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

        DevLog.Write($"방 나가기 결과:  {(ERROR_CODE)responsePkt.Result}");
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
    

    // TODO 로비 생성 후
    // TODO 로비 입장 관련(방처럼) 다 다시 만들기
    
    // 로비에서 채팅을 AddRoomChatMessage로 작성 후 기록
    void PacketProcess_LobbyChat(byte[] packetData)
    {
        var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfLobbyChat>(packetData);
        AddLobbyChatMessage(notifyPkt.UserID, notifyPkt.ChatMessage);
    }

    // 게임 시작 시 선플레이어 응답 기록
    void PacketProcess_GameStartResultResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResGameStart>(packetData);
        
        //TODO 2명 들어왔는지 어디서 처리???
        //TODO 랜덤은??
        /*
        if ((ERROR_CODE)responsePkt.Result == ERROR_CODE.NOT_READY_EXIST)
        {
            DevLog.Write($"모두 레디상태여야 시작합니다.");
        }
        else
        {
            DevLog.Write($"게임시작 !!!! '{responsePkt.UserID}' turn  ");
        }*/
        
        DevLog.Write($"게임시작 !!!!");
        //TODO 여기선 적혀있는 ID랑 비교하지만 실제는 토큰으로 인증된 본인 id와 비교
        if (textBoxUserID.Text == responsePkt.StartID)
        {
            둘차례 = true;
            DevLog.Write($"당신은 흑돌입니다.");
        }
        else
        {
            플레이어흑돌 = false;
            DevLog.Write($"당신은 백돌입니다.");
        }
        // DevLog.Write($"{responsePkt.StartID}가 선입니다.");
    }

    // 돌 두기 후 착수 가능한지 응답 기록
    void PacketProcess_PutStoneResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResPutStone>(packetData);

        //TODO 에러 코드 없이 턴 넘겨도 되나??
        /*
        if((ERROR_CODE)responsePkt.Result != ERROR_CODE.ERROR_NONE)
        {
            DevLog.Write($"Put Stone Error : {(ERROR_CODE)responsePkt.Result}");
        }

        DevLog.Write($"다음 턴 :  {(ERROR_CODE)responsePkt.Result}");*/

        if (responsePkt.isAble==ERROR_CODE.GAME_SAMSAM)
        {
            DevLog.Write($"흑은 삼삼이라 둘 수 없는 자리입니다.");
        }
        else
        {
            착수(responsePkt.xPos, responsePkt.yPos);
            DevLog.Write($"착수 완료"); //TODO 자연스러워지면 빼기
        }
        
        

    }
    
    // 턴 넘어온 후 기록
    void PacketProcess_TurnChangeResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResTurnChange>(packetData);
        
        착수(responsePkt.xPos, responsePkt.yPos);
        DevLog.Write($"당신의 차례");

    }
    
    // 돌 두기 후 둔 자리 알림 응답 기록
    void PacketProcess_PutStoneInfoResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResPutStoneInfo>(packetData);
        돌존재(responsePkt.xPos, responsePkt.yPos);
        DevLog.Write($"'{responsePkt.userID}' Put Stone  : [{responsePkt.xPos}] , [{responsePkt.yPos}] ");

    }
    
    // 게임 종료 후 승리플레이어 응답 기록
    void PacketProcess_GameEndResultResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResGameEnd>(packetData);
        DevLog.Write($"'{responsePkt.WinID}' WIN , END GAME ");

    }
    
    //TODO (6주차) 매칭서버
    void PacketProcess_MatchUserResponse(byte[] packetData)
    {
        /*responsePkt = MemoryPackSerializer.Deserialize<PKTResMatchUser>(packetData);

        DevLog.Write($"매칭 결과:  {(ERROR_CODE)responsePkt.Result} ");*/

    }
    
    //TODO 릴레이..??
    void PacketProcess_RoomRelayNotify(byte[] packetData)
    {
        /*var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfRelay>(packetData);

        var stringData = Encoding.UTF8.GetString(notifyPkt.RelayData);
        DevLog.Write($"방에서 릴레이 받음. {notifyPkt.UserUniqueId} - {stringData}");*/
    }
    
    // 에러 기록
    //TODO 쓰는 상황 알아보기
    // void PacketProcess_ErrorNotify(byte[] packetData)
    // {
    //     var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfError>(packetData);
    //     DevLog.Write($"에러 통보 받음:  {notifyPkt.Error}");
    // }
    
    // 에코 패킷 기록
    // void PacketProcess_Echo(byte[] bodyData)
    // {
    //     DevLog.Write($"Echo 받음:  {bodyData.Length}");
    // }
}