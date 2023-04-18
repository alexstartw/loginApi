namespace loginApi.Repos;

public interface ICheckInRepo
{
    public bool TodayCheckIn(string username, DateTime dateTime);
    public bool GetTodayCheckInStatus(string username);
    public int GetMonthCheckInCount(string username, DateTime dateTime, string[] workDays);
    public int GetAbsentCount(string username, DateTime dateTime, string[] workDays);
    
}