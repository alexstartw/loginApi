namespace loginApi.Repos;

public interface ICheckInRepo
{
    public bool TodayCheckIn(string username);
    public bool GetTodayCheckInStatus(string username);
    public int GetMonthCheckInCount(string username, DateTime dateTime);
    public int GetAbsentCount(string username, DateTime dateTime);
    
}