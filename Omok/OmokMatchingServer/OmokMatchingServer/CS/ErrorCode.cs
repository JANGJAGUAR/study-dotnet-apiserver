namespace OmokMatchingServer.CS;

using System;

public enum ErrorCode : UInt16
{
    None = 0,
    MatchingFailNotExistCompleteMatchingDic = 11001,
    MatchingFailReqQueueNotExist = 11002,
    MatchingFailRedisNotExist = 11003,
    MatchingFailException = 11004,
}