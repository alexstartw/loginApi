using System.Globalization;
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
        Console.WriteLine("CheckInService: userExist: " + userExist);
        Console.WriteLine("CheckInService: hadCheckIn: " + hadCheckIn);
        if (userExist && !hadCheckIn)
        {
            Console.WriteLine("CheckInService: TodayCheckIn");
            var tzi = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            var nowDatetime = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, tzi);

            var localTime = nowDatetime.LocalDateTime;
            var localDay = localTime;
            if (localTime.TimeOfDay < new TimeSpan(5, 0, 0))
            {
                localDay = localDay.AddDays(-1);
            }
            return _checkInRepo.TodayCheckIn(username, localDay);
        }

        return false;
    }
    
    public bool GetTodayCheckInStatus(string username)
    {
        return _checkInRepo.GetTodayCheckInStatus(username);
    }
    
    public int GetMonthCheckInCount(string username, DateTime dateTime)
    {
        var workDays = GetMonthWorkdays(dateTime).ToArray();
        return _checkInRepo.GetMonthCheckInCount(username, dateTime, workDays);
    }
    
    public int GetAbsentCount(string username, DateTime dateTime)
    {
        var workDays = GetMonthWorkdaysTilToday(dateTime).ToArray();
        return _checkInRepo.GetAbsentCount(username, dateTime, workDays);
    }
    
    private IEnumerable<string> GetMonthWorkdays(DateTime dateTime1)
    {
        TimeZoneInfo taipeiZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
        DateTimeOffset taipeiTime = TimeZoneInfo.ConvertTime(dateTime1, taipeiZone);

        CultureInfo taiwanCulture = new CultureInfo("zh-TW");
        DateTimeFormatInfo taiwanDateTimeFormat = taiwanCulture.DateTimeFormat;


        var startDate = new DateTime(taipeiTime.Year, taipeiTime.Month, 1);
        var endDate = new DateTime(taipeiTime.Year, taipeiTime.Month,
            DateTime.DaysInMonth(taipeiTime.Year, taipeiTime.Month));
        var workDays = new List<string>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (taiwanDateTimeFormat.Calendar.GetDayOfWeek(date) != DayOfWeek.Saturday
                && taiwanDateTimeFormat.Calendar.GetDayOfWeek(date) != DayOfWeek.Sunday)
            {
                workDays.Add(date.ToString("yyyy-MM-dd"));
            }
        }

        return workDays;
    }
    private IEnumerable<string> GetMonthWorkdaysTilToday(DateTime today)
    {
        TimeZoneInfo taipeiZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
        DateTimeOffset taipeiTime = TimeZoneInfo.ConvertTime(today, taipeiZone);

        CultureInfo taiwanCulture = new CultureInfo("zh-TW");
        DateTimeFormatInfo taiwanDateTimeFormat = taiwanCulture.DateTimeFormat;


        var startDate = new DateTime(taipeiTime.Year, taipeiTime.Month, 1);
        var endDate = new DateTime(taipeiTime.Year, taipeiTime.Month,taipeiTime.Day);
        var workDays = new List<string>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (taiwanDateTimeFormat.Calendar.GetDayOfWeek(date) != DayOfWeek.Saturday
                && taiwanDateTimeFormat.Calendar.GetDayOfWeek(date) != DayOfWeek.Sunday)
            {
                workDays.Add(date.ToString("yyyy-MM-dd"));
            }
        }

        return workDays;
    }

}