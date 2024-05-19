using System.Data;
using Microsoft.Extensions.Options;
using MySqlConnector;
using OmokAPIServer.CS;
using OmokAPIServer.Model;
using OmokAPIServer.Repository.Interface;
using SqlKata.Execution;
using ZLogger;

namespace OmokAPIServer.Repository;

public class UserInfoRdb : IUserInfoRdb
{
    private readonly IOptions<DbConfig> _dbConfig;
    private readonly ILogger<UserInfoRdb> _logger;
    private IDbConnection _dbConn;
    private readonly SqlKata.Compilers.MySqlCompiler _compiler;
    private readonly QueryFactory _queryFactory;
    
    public UserInfoRdb(ILogger<UserInfoRdb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;
    
        Open();
    
        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConn, _compiler);
    }
    
    public async Task<ErrorCode> CreateNewUser(string id)
    {
        try
        {
            _logger.ZLogDebug($"[CreateAccount] ID: {id}");

            //TODO : DB 이름 변경
            int count = await _queryFactory.Query("usergamedata").InsertAsync(new
            {
                UserId = id,
                CreatedAt = DateTime.Now,
                LastAccessAt = DateTime.Now,
                Win = 0,
                Lose = 0,
                BackItem = 0
            });

            return count != 1 ? ErrorCode.CreateUserFailInsert : ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[AccountDb.CreateAccount] ErrorCode: {ErrorCode.CreateNewUserFailException}, ID: {id}");
            return ErrorCode.CreateNewUserFailException;
        }
    }
    
    
    public async Task<(ErrorCode, UserGameData)> GetUserDataById(string id)
    {
        try
        {
            UserGameData gameData = await _queryFactory.Query("usergamedata")
                .Where("UserID", id)
                .FirstOrDefaultAsync<UserGameData>();
            return (ErrorCode.None, gameData);
        }
        catch (Exception e)
        {
            return (ErrorCode.UserDb_Fail_LoadData, null);
        }
    }
    
    public async Task<ErrorCode> UpdateLastLoginTime(string id)
    {
        try
        {
            UserGameData gameData = await _queryFactory.Query("usergamedata")
                .Where("UserID", id)
                .FirstOrDefaultAsync<UserGameData>();
            gameData.LastAccessAt = DateTime.Now;
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            return ErrorCode.SetLoginTimeFailExceoption;
        }
    }
    
    public void Dispose()
    {
        Close();
    }
    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.UserInfoRdb);
    
        _dbConn.Open();
    }
    
    private void Close()
    {
        _dbConn.Close();
    }
    
}