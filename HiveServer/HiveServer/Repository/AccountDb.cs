using System;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;
using SqlKata.Execution;
using Microsoft.Extensions.Options;
using ZLogger;

namespace HiveServer.Repository;
// MySQL DB 관련
public class AccountDb : IAccountDb
{
    private readonly IOptions<DbConfig> _dbConfig;
    private readonly ILogger<AccountDb> _logger;
    private IDbConnection _dbConn;
    private readonly SqlKata.Compilers.MySqlCompiler _compiler;
    private readonly QueryFactory _queryFactory;
    
    public AccountDb(ILogger<AccountDb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConn, _compiler);
    }
    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.AccountDb);

        _dbConn.Open();
    }
    private void Close()
    {
        _dbConn.Close();
    }
    
    public async Task<ErrorCode> CreateAccountAsync(string email, string pw)
    {
        try
        {
            string saltValue = Security.SaltString();
            string hashingPassword = Security.MakeHashingPassWord(saltValue, pw);
            _logger.ZLogDebug(
            $"[CreateAccount] Email: {email}, SaltValue : {saltValue}, HashingPassword:{hashingPassword}");
        
            int count = await _queryFactory.Query("account").InsertAsync(new
            {
                Email = email,
                SaltValue = saltValue,
                HashedPassword = hashingPassword
            });
        
            return count != 1 ? ErrorCode.CreateAccountFailInsert : ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
            $"[AccountDb.CreateAccount] ErrorCode: {ErrorCode.CreateAccountFailException}, Email: {email}");
            return ErrorCode.CreateAccountFailException;
        }
    }
    public async Task<Tuple<ErrorCode, long>> VerifyUser(string email, string pw)
    {
        // try
        // {
        //     Model.DAO.AdbUser userInfo = await _queryFactory.Query("user")
        //         .Where("Email", email)
        //         .FirstOrDefaultAsync<Model.DAO.AdbUser>();
        //
        //     if (userInfo is null || userInfo.user_id == 0)
        //     {
        //         return new Tuple<ErrorCode, long>(ErrorCode.LoginFailUserNotExist, 0);
        //     }
        //
        //     string hashingPassword = Security.MakeHashingPassWord(userInfo.salt_value, pw);
        //     if (userInfo.hashed_pw != hashingPassword)
        //     {
        //         _logger.ZLogError(
        //             $"[AccountDb.VerifyAccount] ErrorCode: {ErrorCode.LoginFailPwNotMatch}, Email: {email}");
        //         return new Tuple<ErrorCode, long>(ErrorCode.LoginFailPwNotMatch, 0);
        //     }
        //
        //     return new Tuple<ErrorCode, long>(ErrorCode.None, userInfo.user_id);
        // }
        // catch (Exception e)
        // {
        //     _logger.ZLogError(e,
        //         $"[AccountDb.VerifyAccount] ErrorCode: {ErrorCode.LoginFailException}, Email: {email}");
        return new Tuple<ErrorCode, long>(ErrorCode.LoginFailException, 0);
        // }
    }
}

public class DbConfig
{
    // public string MasterDb { get; set; }
    public string AccountDb { get; set; }
    // public string GameDb { get; set; }
    // public string Memcached { get; set; }
}