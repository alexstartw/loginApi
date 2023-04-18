using loginApi.Repos;

namespace loginApi.Services;

public class CheckInService
{
    private readonly CheckInRepo _checkInRepo;
    private readonly AccountRepo _accountRepo;

    public CheckInService(CheckInRepo checkInRepo, AccountRepo accountRepo)
    {
        _checkInRepo = checkInRepo;
        _accountRepo = accountRepo;
    }
    
    public async Task<bool> TodayCheckIn(string username)
    {
        var userExist = await _accountRepo.CheckUserExist(username);
        var hadCheckIn = _checkInRepo.GetTodayCheckInStatus(username);
        if (userExist && !hadCheckIn)
        {
            Console.WriteLine("CheckInService: TodayCheckIn");
            return _checkInRepo.TodayCheckIn(username);
        }

        return false;
    }
    
    public bool GetTodayCheckInStatus(string username)
    {
        return _checkInRepo.GetTodayCheckInStatus(username);
    }
    
    public int GetMonthCheckInCount(string username, DateTime dateTime)
    {
        return _checkInRepo.GetMonthCheckInCount(username, dateTime);
    }
    
    public int GetAbsentCount(string username, DateTime dateTime)
    {
        return _checkInRepo.GetAbsentCount(username, dateTime);
    }
}