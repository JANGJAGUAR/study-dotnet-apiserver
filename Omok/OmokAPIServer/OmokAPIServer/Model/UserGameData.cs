namespace OmokAPIServer.Model;

public class UserGameData
{
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastAccessAt { get; set; }
    public int Win { get; set; }
    public int Lose { get; set; }
    public int BackItem { get; set; }
}