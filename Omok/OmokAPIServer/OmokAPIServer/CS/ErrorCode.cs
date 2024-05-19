namespace OmokAPIServer.CS;

using System;

public enum ErrorCode : UInt16
{
    None = 0,
    
    
    
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
    
}