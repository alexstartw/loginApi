using System.Net;
using loginApi.Content;

namespace loginApi.Repos;

public interface IAccountRepo
{
    public HttpStatusCode CreateAccount(string username, string password);
    public Task<bool> CheckUserExist(string username);
    public Task<bool> CheckPassword(string username, string password);
    public HttpStatusCode ChangeOwnPassword(string username, string oldPassword);
    public HttpStatusCode ChangeUserPassword(string username, string oldPassword);
    public Task<bool> ChangeUserStatus(string username, int status);
    public Task<List<AllUserInfo>> GetAllUserInfo();

}