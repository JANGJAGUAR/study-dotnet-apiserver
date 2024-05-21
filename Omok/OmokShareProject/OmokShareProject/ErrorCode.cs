namespace OmokShareProject;

using System;

public enum ErrorCode : Int16
{
    None = 0,
    
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
    
    MatchingFind_Fail_InvalidResponse = 3302,
    MatchingCheck_Fail_InvalidResponse = 3302,
    
    
    // Auth 2000 ~
    CreateNewUserFailException = 2001,
    CreateUserFailInsert = 2002,
    LoginFailException = 2004,
    LoginFailUserNotExist = 2005,
    LoginFailAddRedis = 2017,
    CheckAuthFailNotExist = 2018,
    CheckAuthFailNotMatch = 2019,
    CheckAuthFailException = 2020,
    GetTokenFailNotExist = 2021,
    GetTokenFailException = 2022,
    SetLoginTimeFailExceoption = 2023,
    
    // Request/Response 3000 ~
    Hive_Fail_InvalidResponse = 3010,
    Hive_Fail_Exception = 3013,
    Hive_No_Response = 3014,
    
    
    
    //UserDb 4000~ 
    UserDb_Fail_LoadData = 4001,
    
    
    
    
    Login_Fail_InvalidResponse = 3400,
    Signup_Fail_InvalidResponse = 3400,
    
    
    
    MatchingFailNotExistCompleteMatchingDic = 11001,
    MatchingFailReqQueueNotExist = 11002,
    MatchingFailRedisNotExist = 11003,
    MatchingFailException = 11004,
    
    Matching_Fail_Notqueued = 11005,
    
    GAME_SAMSAM = 3333,

}