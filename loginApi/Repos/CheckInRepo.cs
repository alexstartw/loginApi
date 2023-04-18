using System.Data;
using System.Globalization;
using System.Net;
using Dapper;
using loginApi.Enums;
using MySql.Data.MySqlClient;


namespace loginApi.Repos;

public class CheckInRepo : ICheckInRepo
{
    private const string DbHost = "35.201.224.32";
    private const string DbUser = "root";
    private const string DbName = "userinfo";
    private const string ConnStr = "server=" + DbHost + ";user=" + DbUser + ";database=" + DbName + ";port=3306;password=;";
    
    public bool TodayCheckIn(string username)
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            string sql = $"INSERT INTO checkin (username, checkin_time, enable) VALUES (@username, @checkin_time,@enable)";

            var tzi = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            var nowDatetime = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, tzi);

            var localTime = nowDatetime.LocalDateTime;
            var localDay = localTime.Date;
            if (localTime.TimeOfDay < new TimeSpan(5, 0, 0))
            {
                localDay = localDay.AddDays(-1);
            }
            
            var rowAffected = conn.Execute(sql, new { username , checkin_time = localDay, CheckInRecordStatus.Enable});
            Console.WriteLine(username + " add " + rowAffected + " checkin record.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false; 
        }
        conn.Close();
        return true;
    }

    public bool GetTodayCheckInStatus(string username)
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            string sql = $"Select count(*) from checkin where username = @username and CAST(checkin_time AS DATE) = CAST(NOW() AS DATE) and enable = 1";
            
            var rowAffected = conn.ExecuteScalar<int>(sql, new { username });
            Console.WriteLine(username + " had " + rowAffected + " checkin record today.");
            if (rowAffected == 0)
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false; 
        }
        conn.Close();
        return true;
    }

    public int GetMonthCheckInCount(string username, DateTime dateTime)
    {
        var workDays = GetMonthWorkdays(dateTime).ToArray();

        int checkInCount;

        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");

            string sql = $"Select count(*) from checkin where username = @username and CAST(checkin_time AS DATE) in @workdays and enable = 1";
            
            checkInCount = conn.ExecuteScalar<int>(sql, new { username, workdays = workDays });
            Console.WriteLine(username + " had " + checkInCount + " checkin record in " + dateTime.Month +".");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return 0; 
        }
        conn.Close();
        return checkInCount;
        
    }

    public int GetAbsentCount(string username, DateTime dateTime)
    {
        var workdays = GetMonthWorkdaysTilToday(dateTime).ToArray();

        int checkInCount;

        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");

            string sql = $"Select count(*) from checkin where username = @username and CAST(checkin_time AS DATE) in @workdays and enable = 1";
            
            checkInCount = conn.ExecuteScalar<int>(sql, new { username, workdays });
            Console.WriteLine(username + " had " + checkInCount + " checkin record in " + dateTime.Month +".");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return 0; 
        }
        conn.Close();
        return workdays.Length - checkInCount;
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
}