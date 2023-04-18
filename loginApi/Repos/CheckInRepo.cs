using System.Data;
using System.Net;
using Dapper;
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
            
            string sql = $"INSERT INTO checkin (username, checkin_time) VALUES (@username, @checkin_time)";

            var offset=new DateTimeOffset(DateTime.Now,new TimeSpan(8,0,0));
            
            var rowAffected = conn.Execute(sql, new { username , checkin_time = offset.ToUniversalTime()});
            Console.WriteLine(rowAffected+" checkin record had been added.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false; 
        }
        conn.Close();
        return true;
    }

    public Task<bool> GetTodayCheckInStatus(string username)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetCheckInCount(string username)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetAbsentCount(string username)
    {
        throw new NotImplementedException();
    }
}