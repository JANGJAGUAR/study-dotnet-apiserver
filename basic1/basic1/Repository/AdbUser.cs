namespace basic1.Repository;

public class AdbUser
{
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string SaltValue { get; set; }
}