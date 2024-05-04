namespace OmokClient.CS;

// 솔루션 하나 파서 서버랑 연동 예정
public enum PACKETID : int                 // ushort
{
    // 에코 요청                            // (안씀)
    // REQ_RES_PACKET_ID_ECHO = 101,
    
    // < 클라이언트 >                          
    CS_BEGIN        = 1001,                 // CS가 뭐지
    // NTF_MUST_CLOSE       = 1005,         // (안쓸듯?)
    
    // 로그인 요청/응답 
    REQ_LOGIN       = 1002,
    RES_LOGIN       = 1003,
    
    // 로비 입장 요청/응답
    //
    
    // 방 입장 요청/응답
    REQ_ROOM_ENTER = 1015,
    RES_ROOM_ENTER = 1016,
    
    // 방에 유저 목록 알림
    NTF_ROOM_USER_LIST = 1017,
    
    // 방에 새 유저 알림
    NTF_ROOM_NEW_USER = 1018,
    
    // 방 떠나기 요청/응답/알림
    REQ_ROOM_LEAVE = 1021,
    RES_ROOM_LEAVE = 1022,
    NTF_ROOM_LEAVE_USER = 1023,
    
    // 채팅 요청/알림
    REQ_ROOM_CHAT = 1026,
    NTF_ROOM_CHAT = 1027,
    
    // 매치 요청/응답/알림                  // 6주차 매칭 서버 
    // REQ_MATCH_USER = 1031,
    // RES_MATCH_USER = 1032,
    // NTF_MATCH_USER = 1033,
    
    // 게임 시작 요청/응답/알림             // (클라에서는 버튼 더 안 눌리게)  
    REQ_GAME_START = 1041,
    RES_GAME_START = 1042,              // 게임 시켜주기
    // NTF_GAME_START = 1043,           // 필요없나
    
    CS_END          = 1100,
    
}



