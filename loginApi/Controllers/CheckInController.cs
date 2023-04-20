using loginApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace loginApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CheckInController : Controller
{
    private readonly CheckInService _checkInService;

    public CheckInController(CheckInService checkInService)
    {
        _checkInService = checkInService;
    }

    [HttpPost]
    public Task<bool> TodayCheckIn(string username)
    {
        return _checkInService.TodayCheckIn(username);
    }
    
    [HttpPost]
    public bool GetTodayCheckInStatus(string username)
    {
        return _checkInService.GetTodayCheckInStatus(username);
    }
    
    [HttpPost]
    public int GetMonthCheckInCount(string username, DateTime dateTime)
    {
        return _checkInService.GetMonthCheckInCount(username, dateTime);
    }
    
    [HttpPost]
    public int GetAbsentCount(string username)
    {
        return _checkInService.GetAbsentCount(username);
    }
}