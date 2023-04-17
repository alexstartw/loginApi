using System.Data;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Dapper;
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
    public async Task<HttpStatusCode> NewUserInfo(string username, string password)
    {
        return await _userInfoService.CreateAccount(username, password);
    }
    
    [HttpPost]
    public string Test()
    {
        return "Hello World";
    }
    
    
    // [HttpPost]
    // public HttpStatusCode ChangePassword(string username, string password)
    // {
    //     IDbConnection conn = new MySqlConnection(_connStr);
    //     try
    //     {
    //         Console.WriteLine("Connecting to MySQL...");
    //         conn.Open();
    //         Console.WriteLine("Connected!");
    //         
    //         string sql = $"UPDATE userinfo SET password = @password WHERE username = @username";
    //         var rowAffected = conn.Execute(sql, new { username , password });
    //         Console.WriteLine(rowAffected);
    //         
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.ToString());
    //         return HttpStatusCode.InternalServerError; 
    //     }
    //     conn.Close();
    //     Console.WriteLine("Done.");
    //     return HttpStatusCode.OK;
    // }
}