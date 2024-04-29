using System;

namespace OmokClient.Packets;

// public class PacketCode
// {
//     
// }
// packet define걸 가져와서 더 가져오고 구분해야 함
public enum PACKET_ID : ushort
{
    PACKET_ID_ECHO = 101,

    REQ_LOGIN = 1002,
    RES_LOGIN = 1003,

    REQ_ROOM_ENTER = 1015,
    RES_ROOM_ENTER = 1016,
    NTF_ROOM_USER_LIST = 1017,
    NTF_ROOM_NEW_USER = 1018,

    REQ_ROOM_LEAVE = 1021,
    RES_ROOM_LEAVE = 1022,
    NTF_ROOM_LEAVE_USER = 1023,

    REQ_ROOM_CHAT = 1026,
    NTF_ROOM_CHAT = 1027,
    
    MATCH_USER_REQ = 1031,
    
    GAME_START_REQ = 1041,
}
public enum ERROR_CODE : Int16
{
    ERROR_NONE = 0,



    ERROR_CODE_USER_MGR_INVALID_USER_UNIQUEID = 112,

    ERROR_CODE_PUBLIC_CHANNEL_IN_USER = 114,

    ERROR_CODE_PUBLIC_CHANNEL_INVALIDE_NUMBER = 115,
}