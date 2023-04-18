using System.Net;
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
    public async Task<HttpStatusCode> ChangePassword(string username, string password)
    {
        return await _userInfoService.ChangePassword(username, password);
    }
}