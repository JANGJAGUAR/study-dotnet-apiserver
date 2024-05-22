namespace OmokSocketServer.Manager;

public class User
{
    UInt64 SequenceNumber = 0;
    public string SessionID { get; set; } 
    
    public int RoomNumber { get; private set; } = -1;
    string UserID;

    public int UserListIdx { get; set; }

    public DateTime LastAccessTime { get; set; }
            
    public void Set(UInt64 sequence, string sessionID, string userID, DateTime lastAccessTime)
    {
        SequenceNumber = sequence;
        SessionID = sessionID;
        UserID = userID;
        LastAccessTime = lastAccessTime;
    }                   
    
    public bool IsConfirm(string netSessionID)
    {
        return SessionID == netSessionID;
    }

    public string ID()
    {
        return UserID;
    }

    public void EnteredRoom(int roomNumber)
    {
        RoomNumber = roomNumber;
    }

    public void LeaveRoom()
    {
        RoomNumber = -1;
    }

    public bool IsStateLogin() { return SequenceNumber != 0; }

    // 방 번호 -1이면 true
    public bool IsStateRoom() { return RoomNumber != -1; }

    public void UserConnectCheck(DateTime dateTime)
    {
        
    }
}