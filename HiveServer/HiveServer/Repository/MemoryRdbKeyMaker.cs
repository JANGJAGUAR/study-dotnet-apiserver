namespace HiveServer.Repository;

public class MemoryRdbKeyMaker
{
    const string loginUID = "HID_";
    const string userLockKey = "HLock_";

    public static string MakeUIDKey(string id)
    {
        return loginUID + id;
    }

    public static string MakeUserLockKey(string id)
    {
        return userLockKey + id;
    }
}