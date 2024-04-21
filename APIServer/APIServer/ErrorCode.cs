namespace APIServer;

using System;

// 1000 ~ 19999
public enum ErrorCode : UInt16
{
    None = 0,

    // Common 1000 ~
    UnhandleException = 1001,
    RedisFailException = 1002,
    InValidRequestHttpBody = 1003,
    TokenDoesNotExist = 1004,
    UidDoesNotExist = 1005,
    AuthTokenFailWrongAuthToken = 1006,
    Hive_Fail_InvalidResponse = 1010,
    InValidAppVersion = 1011,
    InvalidMasterDataVersion = 1012,
    Hive_Fail_Exception = 1013,
    Hive_No_Response = 1014,

    // Auth 2000 ~
    CreateUserFailException = 2001,
    CreateUserFailNoNickname = 2002,
    CreateUserFailDuplicateNickname = 2003,
    LoginFailException = 2004,
    LoginFailUserNotExist = 2005,
    LoginFailPwNotMatch = 2006,
    LoginFailSetAuthToken = 2007,
    LoginUpdateRecentLoginFail = 2008,
    LoginUpdateRecentLoginFailException = 2009,
    AuthTokenMismatch = 2010,
    AuthTokenKeyNotFound = 2011,
    AuthTokenFailWrongKeyword = 2012,
    AuthTokenFailSetNx = 2013,
    AccountIdMismatch = 2014,
    DuplicatedLogin = 2015,
    CreateUserFailInsert = 2016,
    LoginFailAddRedis = 2017,
    CheckAuthFailNotExist = 2018,
    CheckAuthFailNotMatch = 2019,
    CheckAuthFailException = 2020,
    LogoutRedisDelFail = 2021,
    LogoutRedisDelFailException= 2022,
    DeleteAccountFail = 2023,
    DeleteAccountFailException = 2024,
    InitNewUserGameDataFailException = 2025,
    InitNewUserGameDataFailCharacter = 2026,
    InitNewUserGameDataFailGameList = 2027,
    InitNewUserGameDataFailMoney = 2028,
    InitNewUserGameDataFailAttendance = 2029,

    // Friend 2100
    FriendSendReqFailUserNotExist = 2101,
    FriendSendReqFailInsert = 2102,
    FriendSendReqFailException = 2103,
    FriendSendReqFailAlreadyExist = 2104,
    SendFriendReqFailSameUid = 2105,
    FriendGetListFailOrderby = 2106,
    FriendGetListFailException = 2107,
    FriendGetRequestListFailException = 2108,
    FriendDeleteFailNotFriend = 2109,
    FriendDeleteFailDelete = 2110,
    FriendDeleteFailException = 2111,
    FriendDeleteFailSameUid = 2112,
    FriendDeleteReqFailNotFriend = 2113,
    FriendDeleteReqFailDelete = 2114,
    FriendDeleteReqFailException = 2115,
    FriendAcceptFailException = 2116,
    FriendAcceptFailSameUid = 2117,
    AcceptFriendRequestFailUserNotExist = 2118,
    AcceptFriendRequestFailAlreadyFriend = 2119,
    AcceptFriendRequestFailException = 2120,
    FriendSendReqFailNeedAccept = 2121,

    // Game 2200
    MiniGameListFailException = 2201,
    GameSetNewUserListFailException = 2202,
    GameSetNewUserListFailInsert = 2203,
    MiniGameUnlockFailInsert = 2204,
    MiniGameUnlockFailException = 2205,
    MiniGameInfoFailException = 2206,
    MiniGameSaveFailException = 2207,
    MiniGameSaveFailGameLocked = 2208,
    MiniGameUnlockFailAlreadyUnlocked = 2209,
    MiniGameSetPlayCharFailUpdate = 2210,
    MiniGameSetPlayCharFailException = 2211,
    MiniGameSaveFailFoodDecrement = 2212,

    SetUserScoreFailException = 2301,
    GetRankingFailException = 2302,
    GetUserRankFailException = 2303,

    // Item 3000 ~
    CharReceiveFailInsert = 3011,
    CharReceiveFailLevelUP = 3012,
    CharReceiveFailIncrementCharCnt = 3013,
    CharReceiveFailException= 3014,
    CharListFailException = 3015,
    CharNotExist = 3016,
    CharSetCostumeFailUpdate = 3017,
    CharSetCostumeFailException = 3018,

    SkinReceiveFailAlreadyOwn = 3021,
    SkinReceiveFailInsert = 3022,
    SkinReceiveFailException = 3023,
    SkinListFailException = 3024,

    CostumeReceiveFailInsert = 3031,
    CostumeReceiveFailLevelUP = 3032,
    CostumeReceiveFailIncrementCharCnt = 3033,
    CostumeReceiveFailException = 3034,
    CostumeListFailException = 3035,
    CharSetCostumeFailHeadNotExist= 3036,
    CharSetCostumeFailFaceNotExist = 3037,
    CharSetCostumeFailHandNotExist = 3038,

    FoodReceiveFailInsert = 3041,
    FoodReceiveFailIncrementFoodQty = 3042,
    FoodReceiveFailException = 3043,
    FoodListFailException = 3044,
    FoodGearReceiveFailInsert = 3045,
    FoodGearReceiveFailIncrementFoodGear = 3046,
    FoodGearReceiveFailException = 3047,

    GachaReceiveFailException= 3051,


    //UserDb 4000~ 
    UserDb_Fail_LoadData = 4001,
    GetUserDbConnectionFail = 4002,
    


    // MasterDb 5000 ~
    MasterDB_Fail_LoadData = 5001,
    MasterDB_Fail_InvalidData = 5002,

    // User
    UserInfoFailException = 6001,
    UserMoneyInfoFailException = 6002,
    UserUpdateJewelryFailIncremnet = 6003,
    SetMainCharFailException = 6004,
    GetOtherUserInfoFailException = 6005,
    UserNotExist = 6006,

    // Mail
    MailListFailException = 8001,
    MailReceiveFailException = 8002,
    MailReceiveFailAlreadyReceived = 8003,
    MailReceiveFailMailNotExist = 8004,
    MailReceiveFailUpdateReceiveDt = 8005,
    MailRewardListFailException = 8006,
    MailDeleteFailDeleteMail = 8007,
    MailDeleteFailDeleteMailReward = 8008,
    MailDeleteFailException = 8009,
    MailReceiveFailNotMailOwner = 8010,
    MailReceiveRewardsFailException = 8011,

    // Attendance
    AttendanceInfoFailException = 9001,
    AttendanceCheckFailAlreadyChecked = 9002,
    AttendanceCheckFailException = 9003,

    GetRewardFailException = 9004,
    
    // 예전
    // Common 1000 ~
    // UnhandleException = 1001,
    // RedisFailException = 1002,
    // InValidRequestHttpBody = 1003,
    // AuthTokenFailWrongAuthToken = 1006,
    //
    // // Account 2000 ~
    // CreateAccountFailException = 2001,
    // LoginFailException = 2002,
    // LoginFailUserNotExist = 2003,
    // LoginFailPwNotMatch = 2004,
    // LoginFailSetAuthToken = 2005,
    // AuthTokenMismatch = 2006,
    // AuthTokenNotFound = 2007,
    // AuthTokenFailWrongKeyword = 2008,
    // AuthTokenFailSetNx = 2009,
    // AccountIdMismatch = 2010,
    // DuplicatedLogin = 2011,
    // CreateAccountFailInsert = 2012,
    // LoginFailAddRedis = 2014,
    // CheckAuthFailNotExist = 2015,
    // CheckAuthFailNotMatch = 2016,
    // CheckAuthFailException = 2017,
    //
    // // Character 3000 ~
    // CreateCharacterRollbackFail = 3001,
    // CreateCharacterFailNoSlot = 3002,
    // CreateCharacterFailException = 3003,
    // CharacterNotExist = 3004,
    // CountCharactersFail = 3005,
    // DeleteCharacterFail = 3006,
    // GetCharacterInfoFail = 3007,
    // InvalidCharacterInfo = 3008,
    // GetCharacterItemsFail = 3009,
    // CharacterCountOver = 3010,
    // CharacterArmorTypeMisMatch = 3011,
    // CharacterHelmetTypeMisMatch = 3012,
    // CharacterCloakTypeMisMatch = 3012,
    // CharacterDressTypeMisMatch = 3013,
    // CharacterPantsTypeMisMatch = 3012,
    // CharacterMustacheTypeMisMatch = 3012,
    // CharacterArmorCodeMisMatch = 3013,
    // CharacterHelmetCodeMisMatch = 3014,
    // CharacterCloakCodeMisMatch = 3015,
    // CharacterDressCodeMisMatch = 3016,
    // CharacterPantsCodeMisMatch = 3017,
    // CharacterMustacheCodeMisMatch = 3018,
    // CharacterHairCodeMisMatch = 3019,
    // CharacterCheckCodeError = 3010,
    // CharacterLookTypeError = 3011,
    //
    // CharacterStatusChangeFail = 3012,
    // CharacterIsExistGame = 3013,
    // GetCharacterListFail = 3014,
    //
    // //GameDb 4000~ 
    // GetGameDbConnectionFail = 4002
}
