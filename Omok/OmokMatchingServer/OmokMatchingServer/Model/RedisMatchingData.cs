namespace OmokMatchingServer.Model;

//TODO: 공유(서버만)
public class RedisMatchingData
{
    public string UserId1 { get; set; }
    public string UserId2 { get; set; }
    public string ServerAddress { get; set; }
    public int RoomNumber { get; set; }
}