namespace APIServer.Repository;

public class UserGameData
{
    public int Accountid { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int Win { get; set; }
    public int Lose { get; set; }
}