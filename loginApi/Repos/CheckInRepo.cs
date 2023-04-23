using System.Data;
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
    
    public virtual bool TodayCheckIn(string username, DateTime dateTime)
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            string sql = $"INSERT INTO checkin (username, checkin_time, enable) VALUES (@username, @checkin_time,@enable)";

            var rowAffected = conn.Execute(sql, new { username , checkin_time = dateTime, CheckInRecordStatus.Enable});
            Console.WriteLine(username + " add " + rowAffected + " checkin record. Time: " + dateTime.Date);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false; 
        }
        conn.Close();
        return true;
    }

    public virtual bool GetTodayCheckInStatus(string username, DateTime localDay)
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            string sql = $"Select count(*) from checkin where username = @username and CAST(checkin_time AS DATE) = CAST(@now_time AS DATE) and enable = 1";
            
            var rowAffected = conn.ExecuteScalar<int>(sql, new { username, now_time = localDay });
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

    public virtual int GetMonthCheckInCount(string username, DateTime dateTime, string[] workDays)
    {
        int checkInCount;

        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");

            string sql = $"Select count(*) from checkin where username = @username and CAST(checkin_time AS DATE) in @workDays and enable = 1";
            
            checkInCount = conn.ExecuteScalar<int>(sql, new { username, workDays });
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

    public virtual int GetAbsentCount(string username, string[] workDays)
    {

        int checkInCount;

        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");

            string sql = $"Select count(*) from checkin where username = @username and CAST(checkin_time AS DATE) in @workdays and enable = 1";
            
            checkInCount = conn.ExecuteScalar<int>(sql, new { username, workDays });
            Console.WriteLine(username + " had " + checkInCount + " checkin record" +".");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return 0; 
        }
        conn.Close();
        return workDays.Length - checkInCount;
    }
    
}