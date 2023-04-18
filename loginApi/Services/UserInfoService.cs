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
    
    public async Task<HttpStatusCode> CreateNewUser(string username, string password)
    {
        var checkUserExistStatus = await _accountRepo.CheckUserExist(username);
        if (!checkUserExistStatus)
        {
            return _accountRepo.CreateAccount(username, password);
        }

        return checkUserExistStatus ? HttpStatusCode.Conflict : HttpStatusCode.InternalServerError;

    }
    
    public Task<bool> CheckPassword(string username, string password)
    {
        return _accountRepo.CheckPassword(username, password);
    }

    public async Task<bool> CheckUsernameExist(string username)
    {
        var checkUserExistStatus = await _accountRepo.CheckUserExist(username);
        return checkUserExistStatus;
    }

    public async Task<HttpStatusCode> ChangePassword(string username, string password)
    {
        var checkUserExistStatus = await _accountRepo.CheckUserExist(username);
        return checkUserExistStatus ? _accountRepo.ChangePassword(username, password) : HttpStatusCode.InternalServerError;
    }
}