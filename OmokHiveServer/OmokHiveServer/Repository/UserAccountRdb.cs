using System.Data;
using Microsoft.Extensions.Options;
using MySqlConnector;
using OmokHiveServer.Model;
using OmokHiveServer.Repository.Interface;
using OmokShareProject;
using SqlKata.Execution;
using ZLogger;

namespace OmokHiveServer.Repository;

public class UserAccountRdb : IUserAccountRdb
{
    private readonly ILogger<UserAccountRdb> _logger;
    private readonly IOptions<DbConfig> _dbConfig;
    private IDbConnection _dbConn;
    private readonly SqlKata.Compilers.MySqlCompiler _compiler;
    private readonly QueryFactory _queryFactory;
    
    public UserAccountRdb(ILogger<UserAccountRdb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConn, _compiler);
    }
    
    public void Dispose()
    {
        Close();
    }
    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.UserAccountRdb);

        _dbConn.Open();
    }

    private void Close()
    {
        _dbConn.Close();
    }
    
    public async Task<ErrorCode> CreateAccountData(string userid, string password)
    {
        //TODO: 중복 확인 후, 등록 후 None반환
        ErrorCode errorCode = ErrorCode.None;
        // try
        // {
        //     string saltValue = Security.SaltString();
        //     string hashingPassword = Security.MakeHashingPassWord(saltValue, pw);
        //     _logger.ZLogDebug(
        //         $"[CreateAccount] Email: {email}, SaltValue : {saltValue}, HashingPassword:{hashingPassword}");
        //
        //     int count = await _queryFactory.Query("account").InsertAsync(new
        //     {
        //         Email = email,
        //         SaltValue = saltValue,
        //         HashedPassword = hashingPassword
        //     });
        //
        //     return count != 1 ? ErrorCode.CreateAccountFailInsert : ErrorCode.None;
        // }
        // catch (Exception e)
        // {
        //     _logger.ZLogError(e,
        //         $"[AccountDb.CreateAccount] ErrorCode: {ErrorCode.CreateAccountFailException}, Email: {email}");
        //     return ErrorCode.CreateAccountFailException;
        // }
        return errorCode;
    }

    public async Task<ErrorCode> VerifyAccountData(string userid, string password)
    {
        //TODO: 로그인 정보 있으면 None, 없으면 ErrorCode.LoginFail 반환;
        ErrorCode errorCode = ErrorCode.None;
        // try
        // {
        //     var userInfo = await _queryFactory.Query("account")
        //                             .Where("Email", email)
        //                             .FirstOrDefaultAsync<AdbUser>();
        //
        //     // var user_Info = await _queryFactory.Query("account").Where("Email", email).FirstOrDefaultAsync<DBUserInfo>();
        //     if (userInfo.AccountId == 0)
        //     {
        //         return new Tuple<ErrorCode, long>(ErrorCode.LoginFailUserNotExist, 0);
        //     }
        //
        //     string hashingPassword = Security.MakeHashingPassWord(userInfo.SaltValue, pw);
        //     if (userInfo.HashedPassword != hashingPassword)
        //     {
        //         _logger.ZLogError(
        //             $"[AccountDb.VerifyAccount] ErrorCode: {ErrorCode.LoginFailPwNotMatch}, Email: {email}");
        //         return new Tuple<ErrorCode, long>(ErrorCode.LoginFailPwNotMatch, 0);
        //     }
        //
        //     return new Tuple<ErrorCode, long>(ErrorCode.None, userInfo.AccountId);
        //     
        // }
        // catch (Exception e)
        // {
        //     _logger.ZLogError(e,
        //         $"[AccountDb.VerifyAccount] ErrorCode: {ErrorCode.LoginFailException}, Email: {email}");
        //     return new Tuple<ErrorCode, long>(ErrorCode.LoginFailException, 0);
        // }
        return errorCode;
    }
    
}