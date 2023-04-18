using System.Net;

namespace loginApi.Repos;

public interface IAccountRepo
{
    public HttpStatusCode CreateAccount(string username, string password);
    public Task<HttpStatusCode> CheckUserExist(string username);
    public Task<bool> CheckPassword(string username, string password);
    public HttpStatusCode ChangePassword(string username, string password);
    
}