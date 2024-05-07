using MemoryPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PvPGameServer.CS;


namespace PvPGameServer;

public class PKHRoom : PKHandler
{
    // REQ / RES 처리
    List<Room> _roomList = null;
    int _startRoomNumber;

    private DateTime _nowTime;

    OmokRule OmokLogic = new OmokRule();
    
    public void SetRooomList(List<Room> roomList)
    {
        _roomList = roomList;
        _startRoomNumber = roomList[0].Number;
    }

    // 패킷id에 맞춰서 함수 실행
    public void RegistPacketHandler(Dictionary<int, Action<MemoryPackBinaryRequestInfo>> packetHandlerMap)
    {
        packetHandlerMap.Add((int)PACKETID.REQ_ROOM_ENTER, RequestRoomEnter);
        packetHandlerMap.Add((int)PACKETID.REQ_ROOM_LEAVE, RequestLeave);
        packetHandlerMap.Add((int)PACKETID.NTF_IN_ROOM_LEAVE, NotifyLeaveInternal);
        packetHandlerMap.Add((int)PACKETID.REQ_ROOM_CHAT, RequestChat);
        packetHandlerMap.Add((int)PACKETID.REQ_GAME_START, RequestGameStart);
        packetHandlerMap.Add((int)PACKETID.REQ_PUT_STONE, RequestPutstone);
        packetHandlerMap.Add((int)PACKETID.NTF_IN_SERVER_TIMER, NotifyServerTimer);
        packetHandlerMap.Add((int)PACKETID.REQ_HEART_BEAT, ReqHeartBeat);
    }


    Room GetRoom(int roomNumber)
    {
        var index = roomNumber - _startRoomNumber;

        if( index < 0 || index >= _roomList.Count())
        {
            return null;
        }

        return _roomList[index];
    }
            
    (bool, Room, RoomUser) CheckRoomAndRoomUser(string userNetSessionID)
    {
        var user = _userMgr.GetUser(userNetSessionID);
        if (user == null)
        {
            return (false, null, null);
        }

        var roomNumber = user.RoomNumber;
        var room = GetRoom(roomNumber);

        if(room == null)
        {
            return (false, null, null);
        }

        var roomUser = room.GetUserByNetSessionId(userNetSessionID);

        if (roomUser == null)
        {
            return (false, room, null);
        }

        return (true, room, roomUser);
    }
    
    
    // [Heart Beat]
    // 패킷이 매초 오기 때문에 매초 처리하는 함수
    public void NotifyServerTimer(MemoryPackBinaryRequestInfo packetData)
    {
        // 시간 갱신
        // var reqData = MemoryPackSerializer.Deserialize<PKTServerTimer>(packetData.Data);
        // _nowTime = reqData.dateTime;
        // Console.WriteLine("Timer: " + _nowTime);

        _roomMgr.CheckRoomList();
        _userMgr.CheckUserList();
    }
    
    // 클라한테 받은 HeartBeat를 처리하는 함수
    public void ReqHeartBeat(MemoryPackBinaryRequestInfo packetData)
    {
        // 해당 클라의 마지막 접속 시간 갱신
        var sessionID = packetData.SessionID;
        var reqData = MemoryPackSerializer.Deserialize<PKTClientHeartBeat>(packetData.Data);
        var user = _userMgr.GetUser(sessionID);
        user.LastAccessTime = reqData.dateTime;
        
        //TODO 이거 수정해서 1번만 가게 하면 테스트 가능
    }



    public void RequestRoomEnter(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        MainServer.MainLogger.Debug("RequestRoomEnter");

        try
        {
            var user = _userMgr.GetUser(sessionID);

            if (user == null || user.IsConfirm(sessionID) == false)
            {
                ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_INVALID_USER, sessionID);
                return;
            }

            if (user.IsStateRoom())
            {
                ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_INVALID_STATE, sessionID);
                return;
            }

            var reqData = MemoryPackSerializer.Deserialize<PKTReqRoomEnter>(packetData.Data);
            
            var room = GetRoom(reqData.RoomNumber);

            if (room == null)
            {
                ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_INVALID_ROOM_NUMBER, sessionID);
                return;
            }

            if (room.AddUser(user.ID(), sessionID) == false)
            {
                ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_FAIL_ADD_USER, sessionID);
                return;
            }


            user.EnteredRoom(reqData.RoomNumber);

            room.NotifyPacketUserList(sessionID);
            room.NofifyPacketNewUser(sessionID, user.ID());

            ResponseEnterRoomToClient(ERROR_CODE.NONE, sessionID);

            MainServer.MainLogger.Debug("RequestEnterInternal - Success");
        }
        catch (Exception ex)
        {
            MainServer.MainLogger.Error(ex.ToString());
        }
    }

    void ResponseEnterRoomToClient(ERROR_CODE errorCode, string sessionID)
    {
        var resRoomEnter = new PKTResRoomEnter()
        {
            Result = (short)errorCode
        };

        var sendPacket = MemoryPackSerializer.Serialize(resRoomEnter);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.RES_ROOM_ENTER);
        
        NetSendFunc(sessionID, sendPacket);
    }

    public void RequestLeave(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        MainServer.MainLogger.Debug("방나가기 요청 받음");

        try
        {
            var user = _userMgr.GetUser(sessionID);
            if(user == null)
            {
                return;
            }

            if(LeaveRoomUser(sessionID, user.RoomNumber) == false)
            {
                return;
            }

            user.LeaveRoom();

            ResponseLeaveRoomToClient(sessionID);

            MainServer.MainLogger.Debug("Room RequestLeave - Success");
        }
        catch (Exception ex)
        {
            MainServer.MainLogger.Error(ex.ToString());
        }
    }

    // 해당 번호 방에서 세션 id를 가진 클라를 빼겠다
    bool LeaveRoomUser(string sessionID, int roomNumber)
    {
        MainServer.MainLogger.Debug($"LeaveRoomUser. SessionID:{sessionID}");

        var room = GetRoom(roomNumber);
        if (room == null)
        {
            return false;
        }

        var roomUser = room.GetUserByNetSessionId(sessionID);
        if (roomUser == null)
        {
            return false;
        }
                    
        var userID = roomUser.UserID;
        room.RemoveUser(roomUser);

        room.NotifyPacketLeaveUser(userID);
        return true;
    }

    void ResponseLeaveRoomToClient(string sessionID)
    {
        var resRoomLeave = new PKTResRoomLeave()
        {
            Result = (short)ERROR_CODE.NONE
        };

        var sendPacket = MemoryPackSerializer.Serialize(resRoomLeave);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.RES_ROOM_LEAVE);
   
        NetSendFunc(sessionID, sendPacket);
    }

    //방에서 빼라는 id가 왔을때
    public void NotifyLeaveInternal(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        MainServer.MainLogger.Debug($"NotifyLeaveInternal. SessionID: {sessionID}");

        var reqData = MemoryPackSerializer.Deserialize<PKTInternalNtfRoomLeave>(packetData.Data);            
        LeaveRoomUser(sessionID, reqData.RoomNumber);
    }
            
    public void RequestChat(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        MainServer.MainLogger.Debug("Room RequestChat");

        try
        {
            var roomObject = CheckRoomAndRoomUser(sessionID);

            if(roomObject.Item1 == false)
            {
                return;
            }


            var reqData = MemoryPackSerializer.Deserialize<PKTReqRoomChat>(packetData.Data);

            var notifyPacket = new PKTNtfRoomChat()
            {
                UserID = roomObject.Item3.UserID,
                ChatMessage = reqData.ChatMessage
            };

            var sendPacket = MemoryPackSerializer.Serialize(notifyPacket);
            MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NTF_ROOM_CHAT);
            
            roomObject.Item2.Broadcast("", sendPacket);

            MainServer.MainLogger.Debug("Room RequestChat - Success");
        }
        catch (Exception ex)
        {
            MainServer.MainLogger.Error(ex.ToString());
        }
    }
    
    // [게임 시작]
    // 게임 시작 요청 받으면 실행
    public void RequestGameStart(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        MainServer.MainLogger.Debug("게임 시작 요청 받음");

        
        //1명이면 기다리고
        //2명이면 게임 시작(게임 시작해주는 패킷 보내주기)
        
        try
        {
            var user = _userMgr.GetUser(sessionID);

            var reqData = MemoryPackSerializer.Deserialize<PKTReqGameStart>(packetData.Data);
            //TODO 이거 안 쓰는 거 같은데 그럼 굳이 뭐 넘겨줄 필요 없지 않나
            
            //세션 ID를 패킷 전송을 위해 써야해서 유저id가 아닌 세션 id로 처리
            var room = GetRoom(user.RoomNumber);
            if (!room.IsWaiting )
            {
                room.IsWaiting = true;
                room.WaitingSID = sessionID;
            }
            else if(room.WaitingSID!=sessionID)
            {
                // 랜덤으로 선 정하기
                Random rand = new Random();
                string startSessionId = rand.Next(1, 3) == 1 ? sessionID : room.WaitingSID;
                
                // 세션 id로 처리했기에 유저 id로 넘겨줘야 클라가 처리 가능
                var startUserInfo = CheckRoomAndRoomUser(startSessionId);
                
                
                // 게임 진행을 위해 다음 순서 계속 기록
                if (startSessionId == sessionID)
                {
                    // 들어온 사람 (선), 기다린 사람 (후)
                    room.NowTurnSID = sessionID;
                    room.NextTurnSID = room.WaitingSID;
                }
                else
                {
                    // 들어온 사람 (후), 기다린 사람 (선)
                    // var secondUserInfo = CheckRoomAndRoomUser(sessionID);
                    room.NowTurnSID = room.WaitingSID;
                    room.NextTurnSID = sessionID;
                }

                ResponseGameStart(ERROR_CODE.NONE, sessionID, startUserInfo.Item3.UserID);
                ResponseGameStart(ERROR_CODE.NONE, room.WaitingSID, startUserInfo.Item3.UserID);
                
                // 게임 시작 시간 갱신
                room.IsGameStart = true;
                room.GameStartTime = DateTime.Now;
                MainServer.MainLogger.Debug("RequestGameStart - Success");
            }
        }
        catch (Exception ex)
        {
            MainServer.MainLogger.Error(ex.ToString());
        }
    }
    
    // 게임 시작 응답 전달
    void ResponseGameStart(ERROR_CODE errorCode, string sessionID, string startUserID)
    {
        var resGameStart = new PKTResGameStart();
        resGameStart.StartID = startUserID;
        
        var sendPacket = MemoryPackSerializer.Serialize(resGameStart);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.RES_GAME_START);
        
        NetSendFunc(sessionID, sendPacket);
    }
    
    // [돌 두기]
    // 돌 두기 요청 받으면 실행
    public void RequestPutstone(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        MainServer.MainLogger.Debug("돌 두기 요청 받음");

        try
        {
            var user = _userMgr.GetUser(sessionID);
            var room = GetRoom(user.RoomNumber);
            
            var reqData = MemoryPackSerializer.Deserialize<PKTReqPutStone>(packetData.Data);
            var putStoneResult = OmokLogic.돌두기(reqData.xPos, reqData.yPos);
            
            // 돌을 둔 클라한테는 결과 반환
            ResponsePutstone(putStoneResult, sessionID, reqData.xPos, reqData.yPos);
            
            // 둘 수 있었으면 다음에 둘 클라한테는 턴 넘어가는걸 반환
            if (putStoneResult == ERROR_CODE.NONE)
            {
                ResponseTurnChange(room.NextTurnSID, reqData.xPos, reqData.yPos);
                // 턴 교체
                (room.NowTurnSID, room.NextTurnSID) = (room.NextTurnSID, room.NowTurnSID);
            }
            
            
            MainServer.MainLogger.Debug("RequestGameStart - Success");
        }
        catch (Exception ex)
        {
            MainServer.MainLogger.Error(ex.ToString());
        }
    }
    // 지금 둔 클라에게 응답
    void ResponsePutstone(ERROR_CODE errorCode, string sessionID, int xpos, int ypos)
    {
        var iswin =  OmokLogic.오목확인(xpos, ypos);
        
        var resGameStart = new PKTResPutStone();
        
        resGameStart.xPos = xpos;
        resGameStart.yPos = ypos;
        resGameStart.isAble = errorCode;
        //TODO 아래랑 묶기 
        resGameStart.isWin = iswin;
        if (iswin)
        {
            // 함수 처리
            var user = _userMgr.GetUser(sessionID);
            var room = GetRoom(user.RoomNumber);
            room.IsGameStart = false;
            room.WaitingSID = "";
            //TODO 초기화 더 있는지 확인, 방 들어올때도 초기화, 그때랑 다름.
        }
        
        var sendPacket = MemoryPackSerializer.Serialize(resGameStart);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.RES_PUT_STONE);
        
        NetSendFunc(sessionID, sendPacket);
    }
    // 다음에 둘 클라에게 응답
    void ResponseTurnChange(string sessionID, int xpos, int ypos)
    {
        var islose =  OmokLogic.오목확인(xpos, ypos);
        
        //상대가 둔거 알려줘야 함
        var resGameStart = new PKTResTurnChange();
        resGameStart.xPos = xpos;
        resGameStart.yPos = ypos;
        resGameStart.isLose = islose;
        
        var sendPacket = MemoryPackSerializer.Serialize(resGameStart);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.RES_TURN_CHANGE);
        
        NetSendFunc(sessionID, sendPacket);
        
        // 착수 시간 갱신
        var user = _userMgr.GetUser(sessionID);
        var room = GetRoom(user.RoomNumber);
        room.LastPutStoneTime = DateTime.Now;
    }
}
