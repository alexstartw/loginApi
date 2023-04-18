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
    public bool TodayCheckIn(string username)
    {
        return _checkInService.TodayCheckIn(username);
    }
}