using System.Data;
using System.Net;
using Dapper;
using MySql.Data.MySqlClient;

namespace loginApi.Repos;

public class AccountRepo : IAccountRepo
{
    private const string DbHost = "35.201.224.32";
    private const string DbUser = "root";
    private const string DbName = "userinfo";
    private const string ConnStr = "server=" + DbHost + ";user=" + DbUser + ";database=" + DbName + ";port=3306;password=;";

    public HttpStatusCode CreateAccount(string username, string password)
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            string sql = $"INSERT INTO userinfo (username, password, created_time, update_time, enable) VALUES (@username, @password, @created_time, @update_time, @enable)";
            var createdTime = DateTime.Now;
            var updateTime = DateTime.Now;
            var enable = AccountStatus.Enable;
            var rowAffected = conn.Execute(sql, new { username , password , created_time = createdTime, update_time = updateTime, enable });
            Console.WriteLine(rowAffected+" account had been created.");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return HttpStatusCode.InternalServerError; 
        }
        conn.Close();
        return HttpStatusCode.OK;
    }
    
    public async Task<HttpStatusCode> CheckUserExist(string username)
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            string sql = $"SELECT COUNT(*) FROM userinfo WHERE username = @username";
            
            var rowAffected = await conn.ExecuteScalarAsync<int>(sql, new { username });
            Console.WriteLine(rowAffected);
            if (rowAffected>0)
            {
                return HttpStatusCode.Conflict;
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return HttpStatusCode.InternalServerError; 
        }
        conn.Close();
        Console.WriteLine("Done.");
        return HttpStatusCode.OK;
    }

    public async Task<bool> CheckPassword(string username, string password)
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            string sql = $"SELECT password from userinfo WHERE username = @username";
            var dbPw = await conn.ExecuteScalarAsync<string>(sql, new { username });
            
            Console.WriteLine(dbPw);

            if (!password.Equals(dbPw))
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
        Console.WriteLine("Done.");
        return true;
    }
    
    public HttpStatusCode ChangePassword(string username, string password)
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            string sql = $"UPDATE userinfo SET password = @password WHERE username = @username";
            var rowAffected = conn.Execute(sql, new { username , password });
            Console.WriteLine(rowAffected);
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return HttpStatusCode.InternalServerError; 
        }
        conn.Close();
        Console.WriteLine("Done.");
        return HttpStatusCode.OK;
    }
}