namespace OmokSocketServer.CS;

using System;

public enum ErrorCode : Int16
{
    // ERROR_NONE = 0,
    
    ERROR_CODE_USER_MGR_INVALID_USER_UNIQUEID = 112,

    ERROR_CODE_PUBLIC_CHANNEL_IN_USER = 114,

    ERROR_CODE_PUBLIC_CHANNEL_INVALIDE_NUMBER = 115,
    
    NONE                        = 0, // 에러가 아니다

    // 서버 초기화 에라
    REDIS_INIT_FAIL             = 1,    // Redis 초기화 에러

    // 로그인 
    LOGIN_INVALID_AUTHTOKEN             = 1001, // 로그인 실패: 잘못된 인증 토큰
    ADD_USER_DUPLICATION                = 1002,
    REMOVE_USER_SEARCH_FAILURE_USER_ID  = 1003,
    USER_AUTH_SEARCH_FAILURE_USER_ID    = 1004,
    USER_AUTH_ALREADY_SET_AUTH          = 1005,
    LOGIN_ALREADY_WORKING = 1006,
    LOGIN_FULL_USER_COUNT = 1007,

    DB_LOGIN_INVALID_PASSWORD   = 1011,
    DB_LOGIN_EMPTY_USER         = 1012,
    DB_LOGIN_EXCEPTION          = 1013,

    ROOM_ENTER_INVALID_STATE = 1021,
    ROOM_ENTER_INVALID_USER = 1022,
    ROOM_ENTER_ERROR_SYSTEM = 1023,
    ROOM_ENTER_INVALID_ROOM_NUMBER = 1024,
    ROOM_ENTER_FAIL_ADD_USER = 1025,
    
    GAME_SAMSAM = 1333,
}