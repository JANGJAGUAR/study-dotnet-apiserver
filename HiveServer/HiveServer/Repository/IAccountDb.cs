using System;
using System.Threading.Tasks;
namespace HiveServer.Repository;
// MySQL DB 관련
public interface IAccountDb
    // *** 이거 사용 이유 : IDisposable 
{
    //계정 생성 메서드
    public Task<ErrorCode> CreateAccountAsync(String email, String pw);
    //사용자 확인 메서드
    public Task<Tuple<ErrorCode, Int64>> VerifyUser(String email, String pw);
    
}