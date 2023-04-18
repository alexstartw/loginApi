using loginApi.Repos;

namespace loginApi.Services;

public class CheckInService
{
    private readonly CheckInRepo _checkInRepo;

    public CheckInService(CheckInRepo checkInRepo)
    {
        _checkInRepo = checkInRepo;
    }
    
    public async Task<bool> TodayCheckIn(string username)
    {
        return await _checkInRepo.TodayCheckIn(username);
    }
}