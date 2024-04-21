namespace HiveServer.Repository;

public class AdbUser
{
    public long AccountId { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string SaltValue { get; set; }
}