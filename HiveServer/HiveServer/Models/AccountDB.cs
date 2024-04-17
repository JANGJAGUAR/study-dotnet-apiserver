namespace HiveServer.Models;

public class AccountDB
{
    public long user_id { get; set; }
    public string email { get; set; }
    public string hashed_pw { get; set; }
    public string salt_value { get; set; }
}