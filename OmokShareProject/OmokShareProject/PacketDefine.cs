namespace OmokShareProject;

public enum PACKETID : int               // ushort
{
    // 에코 요청                            // (안씀)
    // REQ_RES_PACKET_ID_ECHO = 101,
    
    // < 클라이언트 >                          
    CS_BEGIN        = 1001,    
    NTF_MUST_CLOSE       = 1005, 
    
    // 로그인 요청/응답 
    REQ_LOGIN       = 1002,
    RES_LOGIN       = 1003,
    
    // 로비 입장 요청/응답
    // 
    
    // 로비 채팅 알림
    NTF_LOBBY_CHAT = 1011,
    
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
    
    // 방 채팅 요청/알림
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
    
    // 돌 두기 요청/응답/알림
    REQ_PUT_STONE = 1051,
    RES_PUT_STONE = 1052,
    RES_PUT_STONE_INFO = 1053,
    
    RES_TURN_CHANGE = 1061,
    
    RES_TIME_TURN_CHANGE = 1063,

    RES_GAME_END = 1071,
    RES_TIME_GAME_END = 1072,
    
    RES_MATCH_USER = 1081,
   
    NTF_ROOM_RELAY = 1091,
    
    CS_END          = 1100,
    
    // 시스템, 서버 - 서버
    SS_START    = 8001,

    NTF_IN_CONNECT_CLIENT = 8011,
    NTF_IN_DISCONNECT_CLIENT = 8012,

    REQ_SS_SERVERINFO = 8021,
    RES_SS_SERVERINFO = 8023,

    REQ_IN_ROOM_ENTER = 8031,
    RES_IN_ROOM_ENTER = 8032,

    NTF_IN_ROOM_LEAVE = 8036,
    
    NTF_IN_TIME_TURN_CHANGE = 8062,
    
    NTF_IN_SERVER_TIMER = 8080,
    
    REQ_HEART_BEAT = 8088,


    // DB 8101 ~ 9000
    REQ_DB_LOGIN = 8101,
    RES_DB_LOGIN = 8102,
}


