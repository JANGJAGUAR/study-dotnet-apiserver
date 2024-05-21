namespace OmokHiveServer.Model;

public class UserAccountData
{
    public string UserId { get; set; }
    public string HashedPassword { get; set; }
    public string SaltValue { get; set; }
}