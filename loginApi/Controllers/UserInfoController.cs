using System.Net;
using loginApi.Content;
using Microsoft.AspNetCore.Mvc;
using loginApi.Services;

namespace loginApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class LoginController : Controller
{
    private readonly UserInfoService _userInfoService;

    public LoginController(UserInfoService userInfoService)
    {
        _userInfoService = userInfoService;
    }

    [HttpPost]
    public async Task<HttpStatusCode> CreateNewUser(string username, string password)
    {
        return await _userInfoService.CreateNewUser(username, password);
    }
    
    [HttpPost]
    public async Task<bool> CheckUsernameExist(string username)
    {
        return await _userInfoService.CheckUsernameExist(username);
    }
    
    [HttpPost]
    public async Task<bool> CheckPassword(string username, string password)
    {
        return await _userInfoService.CheckPassword(username, password);
    }
    
    [HttpPost]
    public async Task<HttpStatusCode> ChangePassword(string username, string oldPassword, string newPassword)
    {
        return await _userInfoService.ChangePassword(username, oldPassword, newPassword);
    }
    
    [HttpPost]
    public async Task<bool> ChangeUserStatus(string loginUser, string changeUser, int status)
    {
        return await _userInfoService.ChangeUserStatus(loginUser, changeUser, status);
    }
    
    [HttpPost]
    public async Task<List<AllUserInfo>> GetAllUserInfo(string loginUser)
    {
        return await _userInfoService.GetAllUserInfo(loginUser);
    }

    [HttpGet]
    public string Test()
    {
        return "test";
    }
}