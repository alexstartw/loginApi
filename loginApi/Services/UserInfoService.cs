using System.Net;
using loginApi.Repos;

namespace loginApi.Services;

public class UserInfoService
{
    private readonly AccountRepo _accountRepo;

    public UserInfoService(AccountRepo accountRepo)
    {
        _accountRepo = accountRepo;
    }
    
    public async Task<HttpStatusCode> CreateAccount(string username, string password)
    {
        var checkUserExistStatus = await _accountRepo.CheckUserExist(username);
        if (checkUserExistStatus==HttpStatusCode.OK)
        {
            return _accountRepo.CreateAccount(username, password);
        }

        return checkUserExistStatus == HttpStatusCode.Conflict ? checkUserExistStatus : HttpStatusCode.InternalServerError;


    }
    
    public Task<bool> CheckPassword(string username, string password)
    {
        return _accountRepo.CheckPassword(username, password);
    }
    
}