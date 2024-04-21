namespace HiveServer.Repository;

public class RdbAuthUserData
{
    public string Email { get; set; } = "";
    public string AuthToken { get; set; } = "";
    public long AccountId { get; set; } = 0;
    public string State { get; set; } = ""; // enum UserState    
}