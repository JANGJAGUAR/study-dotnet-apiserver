using System.Data;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using ZLogger;

namespace APIServer.Repository;

public class UserDb : IUserDb
{
    private readonly IOptions<DbConfig> _dbConfig;
    private readonly ILogger<UserDb> _logger;
    private IDbConnection _dbConn;
    private readonly SqlKata.Compilers.MySqlCompiler _compiler;
    private readonly QueryFactory _queryFactory;
    
    public UserDb(ILogger<UserDb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;
    
        Open();
    
        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConn, _compiler);
    }
    
    //
    public async Task<ErrorCode> InitNewUserGameData(String email)
    {
        try
        {
            
            _logger.ZLogDebug($"[CreateAccount] Email: {email}");

            int count = await _queryFactory.Query("usergamedata").InsertAsync(new
            {
                Email = email,
                CreatedAt = DateTime.Now,
                Level = 0,
                Exp = 0,
                Win = 0,
                Lose = 0
            });

            return count != 1 ? ErrorCode.CreateUserFailInsert : ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[AccountDb.CreateAccount] ErrorCode: {ErrorCode.CreateUserFailException}, Email: {email}");
            return ErrorCode.CreateUserFailException;
        }
    }
    public async Task<UserGameData> GetUserByPlayerEmail(string email)
    {
        return await _queryFactory.Query("usergamedata")
            .Where("Email", email)
            .FirstOrDefaultAsync<UserGameData>();
    }
    // 
    public async Task<Tuple<ErrorCode, UserGameData>> LoadUserData(int accountid)
    {
        try
        {
            UserGameData gameInfo = await _queryFactory.Query("usergamedata")
                .Where("AccountId", accountid)
                .FirstOrDefaultAsync<UserGameData>();
            return new Tuple<ErrorCode, UserGameData>(ErrorCode.None,gameInfo);
        }
        catch (Exception e)
        {
            return new Tuple<ErrorCode, UserGameData>(ErrorCode.UserDb_Fail_LoadData, null);
        }
    }
    
    public void Dispose()
    {
        Close();
    }
    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.UserGameDataDb);
    
        _dbConn.Open();
    }
    
    private void Close()
    {
        _dbConn.Close();
    }
    
}