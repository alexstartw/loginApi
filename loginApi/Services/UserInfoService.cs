using System.Net;
using loginApi.Content;
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

        return HttpStatusCode.Conflict;

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

    public async Task<HttpStatusCode> ChangePassword(string username, string oldPassword, string newPassword)
    {
        var checkUserExistStatus = await _accountRepo.CheckUserExist(username);
        var checkPasswordStatus = await _accountRepo.CheckPassword(username, oldPassword);
        if (!checkUserExistStatus || !checkPasswordStatus )
        {
            return HttpStatusCode.BadRequest;
        }
        
        return _accountRepo.ChangeOwnPassword(username, newPassword) ;
    }

    public async Task<bool> ChangeUserStatus(string loginUser, string changeUser, int status)
    {
        if (!loginUser.Equals("admin"))
        {
            return false;
        }
        else
        {
            return await _accountRepo.ChangeUserStatus(changeUser, status);
        }
    }

    public async Task<List<AllUserInfo>> GetAllUserInfo(string loginUser)
    {
        if (!loginUser.Equals("admin"))
        {
            return new List<AllUserInfo>();
        }
        else
        {
            return await _accountRepo.GetAllUserInfo();
        }
    }
    
    public async Task<HttpStatusCode> ChangeUserPassword(string loginUser, string changeUser, string newPassword)
    {
        var checkUserExistStatus = await _accountRepo.CheckUserExist(changeUser);
        if (!checkUserExistStatus || !loginUser.Equals("admin") )
        {
            return HttpStatusCode.BadRequest;
        }
        
        return _accountRepo.ChangeOwnPassword(changeUser, newPassword) ;
    }
}