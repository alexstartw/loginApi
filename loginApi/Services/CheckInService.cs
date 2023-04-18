using loginApi.Repos;

namespace loginApi.Services;

public class CheckInService
{
    private readonly CheckInRepo _checkInRepo;

    public CheckInService(CheckInRepo checkInRepo)
    {
        _checkInRepo = checkInRepo;
    }
    
    public bool TodayCheckIn(string username)
    {
        return _checkInRepo.TodayCheckIn(username);
    }
}