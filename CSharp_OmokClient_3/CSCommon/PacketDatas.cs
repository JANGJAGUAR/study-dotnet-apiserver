// using MessagePack;
// using System;
// using System.Collections.Generic;
// using System.Text;
//
// namespace CSCommon
// {
//     [MessagePackObject]
//     public class PKNtfMustClose : MsgPackPacketHead
//     {
//         [Key(1)]
//         public short Result;
//     }
//
//
//     // 로그인 요청
//     // done
//     [MessagePackObject]
//     public class PKTReqLogin : MsgPackPacketHead
//     {
//         [Key(1)]
//         public string UserID;
//         [Key(2)]
//         public string AuthToken;
//     }
//     // done
//     [MessagePackObject]
//     public class PKTResLogin : MsgPackPacketHead
//     {
//         [Key(1)]
//         public short Result;
//     }
//
//         
//     // done?
//     [MessagePackObject]
//     public class PKTReqLobbyEnter : MsgPackPacketHead
//     {
//         [Key(1)]
//         public Int16 LobbyNumber;
//     }
//     // done?
//     [MessagePackObject]
//     public class PKTResLobbyEnter : MsgPackPacketHead
//     {
//         [Key(1)]
//         public short Result;
//         [Key(2)]
//         public Int16 LobbyNumber;
//     }
//     // done?
//     [MessagePackObject]
//     public class PKTNtfLobbyEnterNewUser : MsgPackPacketHead
//     {
//         [Key(1)]
//         public string UserID;
//     }
//
//     // done?
//     [MessagePackObject]
//     public class PKTResLobbyLeave : MsgPackPacketHead
//     {
//         [Key(1)]
//         public short Result;
//     }
//     // done?
//     [MessagePackObject]
//     public class PKTNtfLobbyLeaveUser : MsgPackPacketHead
//     {
//         [Key(1)]
//         public string UserID;
//     }
//
//     // done?
//     [MessagePackObject]
//     public class PKTReqLobbyChat : MsgPackPacketHead
//     {
//         [Key(1)]
//         public string Message;
//     }
//
//     // done?
//     [MessagePackObject]
//     public class PKTNtfLobbyChat : MsgPackPacketHead
//     {
//         [Key(1)]
//         public string UserID;
//
//         [Key(2)]
//         public string Message;
//     }
//
//
//
//     // done
//     [MessagePackObject]
//     public class PKTReqRoomEnter : MsgPackPacketHead
//     {
//         [Key(1)]
//         public int RoomNumber;
//     }
//     // done
//     [MessagePackObject]
//     public class PKTResRoomEnter : MsgPackPacketHead
//     {
//         [Key(1)]
//         public short Result;
//     }
//     // done
//     [MessagePackObject]
//     public class PKTNtfRoomUserList : MsgPackPacketHead
//     {
//         [Key(1)]
//         public List<string> UserIDList = new List<string>();
//     }
//     // done
//     [MessagePackObject]
//     public class PKTNtfRoomNewUser : MsgPackPacketHead
//     {
//         [Key(1)]
//         public string UserID;
//     }
//
//     // done
//     [MessagePackObject]
//     public class PKTReqRoomLeave : MsgPackPacketHead
//     {
//     }
//     // done
//     [MessagePackObject]
//     public class PKTResRoomLeave : MsgPackPacketHead
//     {
//         [Key(1)]
//         public short Result;
//     }
//     // done
//     [MessagePackObject]
//     public class PKTNtfRoomLeaveUser : MsgPackPacketHead
//     {
//         [Key(1)]
//         public string UserID;
//     }
//
//     // done
//     [MessagePackObject]
//     public class PKTReqRoomChat : MsgPackPacketHead
//     {
//         [Key(1)]
//         public string ChatMessage;
//     }
//
//     // done
//     [MessagePackObject]
//     public class PKTNtfRoomChat : MsgPackPacketHead
//     {
//         [Key(1)]
//         public string UserID;
//
//         [Key(2)]
//         public string ChatMessage;
//     }
// }
