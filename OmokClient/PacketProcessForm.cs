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
            //TODO 밑에 보셈 2개짜리?
        }

        DevLog.Write($"방의 기존 유저 리스트 받음");
        
        /*var notifyPkt = new RoomUserListNtfPacket();
//         notifyPkt.FromBytes(bodyData);
//
//         for (int i = 0; i < notifyPkt.UserCount; ++i)
//         {
                // ?
//             AddRoomUserList(notifyPkt.UserUniqueIdList[i], notifyPkt.UserIDList[i]);
//         }
//
//         DevLog.Write($"방의 기존 유저 리스트 받음");*/
    }
    
    // 방에 새 유저를 받아 AddRoomUserList로 작성 후 기록
    void PacketProcess_RoomNewUserNotify(byte[] packetData)
    {
        var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfRoomNewUser>(packetData);

        AddRoomUserList(notifyPkt.UserID);
        //TODO 밑에 보셈 2개짜리?

        DevLog.Write($"방에 새로 들어온 유저 받음");
        
        /*var notifyPkt = new RoomNewUserNtfPacket();
        notifyPkt.FromBytes(bodyData);

        AddRoomUserList(notifyPkt.UserUniqueId, notifyPkt.UserID);
        
        DevLog.Write($"방에 새로 들어온 유저 받음");*/
    }
    
    // 방에서 나간 후 결과를 기록 (나간 사람 클라에)
    void PacketProcess_RoomLeaveResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResRoomLeave>(packetData);

        DevLog.Write($"방 나가기 결과:  {(ERROR_CODE)responsePkt.Result}");
    }
    
    // 방에서 나간 유저를 RemoveRoomUserList로 삭제 후 기록 (남은 사람 클라에)
    void PacketProcess_RoomLeaveUserNotify(byte[] packetData)
    {
        var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfRoomLeaveUser>(packetData);

        RemoveRoomUserList(notifyPkt.UserID);
        //TODO 밑에 보셈 2개짜리?

        DevLog.Write($"방에서 나간 유저 받음");
        
        /*var notifyPkt = new RoomLeaveUserNtfPacket();
        notifyPkt.FromBytes(bodyData);

        RemoveRoomUserList(notifyPkt.UserUniqueId);

        DevLog.Write($"방에서 나간 유저 받음");*/
    }
    
    // 방에서 채팅을 AddRoomChatMessage로 작성 후 기록
    void PacketProcess_RoomChatNotify(byte[] packetData)
    {
        var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfRoomChat>(packetData);
    
        AddRoomChatMessage(notifyPkt.UserID, notifyPkt.ChatMessage);
        //TODO 밑에 보셈 2개짜리?
        
        /*var responsePkt = new RoomChatNtfPacket();
       responsePkt.FromBytes(bodyData);

       AddRoomChatMessageList(responsePkt.UserUniqueId, responsePkt.Message);*/
    }
    

    // 로비 생성 후
    //TODO 로비 입장 관련(방처럼) 다 다시 만들기
    
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
        
        DevLog.Write($"게임시작 !!!!  '{responsePkt.StartID}' turn  ");
    }

    // 돌 두기 후 다음플레이어 응답 기록
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
        DevLog.Write($"다음 턴 :  {responsePkt.NextID}");

    }
    
    // 돌 두기 후 둔 자리 알림 응답 기록
    void PacketProcess_PutStoneInfoResponse(byte[] packetData)
    {
        var responsePkt = MemoryPackSerializer.Deserialize<PKTResPutStoneInfo>(packetData);
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
    
    //TODO (6주차) 릴레이..??
    void PacketProcess_RoomRelayNotify(byte[] packetData)
    {
        /*var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfRelay>(packetData);

        var stringData = Encoding.UTF8.GetString(notifyPkt.RelayData);
        DevLog.Write($"방에서 릴레이 받음. {notifyPkt.UserUniqueId} - {stringData}");*/
    }
    
    // 에러 기록
    //TODO 쓰는 상황 알아보기
    void PacketProcess_ErrorNotify(byte[] packetData)
    {
        var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfError>(packetData);
        DevLog.Write($"에러 통보 받음:  {notifyPkt.Error}");
    }
    
    // 에코 패킷 기록
    // void PacketProcess_Echo(byte[] bodyData)
    // {
    //     DevLog.Write($"Echo 받음:  {bodyData.Length}");
    // }
}