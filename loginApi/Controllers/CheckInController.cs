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
    public async Task<bool> TodayCheckIn(string username)
    {
        return await _checkInService.TodayCheckIn(username);
    }
}