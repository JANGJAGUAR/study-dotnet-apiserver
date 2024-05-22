namespace OmokMatchingServer.Model;

public class DbConfig
{
    public string RedisAddress { get; set; }
    public string MatchingToSocketKey { get; set; }
    public string SocketToMatchingKey { get; set; }
}