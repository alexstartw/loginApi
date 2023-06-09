﻿using System.Data;
using System.Net;
using Dapper;
using loginApi.Content;
using loginApi.Enums;
using MySql.Data.MySqlClient;

namespace loginApi.Repos;

public class AccountRepo : IAccountRepo
{
    private const string DbHost = "35.201.224.32";
    private const string DbUser = "root";
    private const string DbName = "userinfo";
    private const string ConnStr = "server=" + DbHost + ";user=" + DbUser + ";database=" + DbName + ";port=3306;password=zxcvbnm;";

    public virtual HttpStatusCode CreateAccount(string username, string password)
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            string sql = $"INSERT INTO userinfo (username, password, created_time, update_time, enable) VALUES (@username, @password, @created_time, @update_time, @enable)";
            var rowAffected = conn.Execute(sql, new { username , password , created_time = DateTime.Now, update_time = DateTime.Now,
                enable = AccountStatus.Enable });
            if (rowAffected==1)
            {
                Console.WriteLine(rowAffected+" account had been created.");
            }
            else
            {
                throw new Exception(username + " : Create account failed.");
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return HttpStatusCode.InternalServerError; 
        }
        conn.Close();
        return HttpStatusCode.OK;
    }
    
    public virtual async Task<bool> CheckUserExist(string username)
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
                return true;
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false; 
        }
        conn.Close();
        Console.WriteLine("Done.");
        return false;
    }

    public virtual async Task<bool> CheckPassword(string username, string password)
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            string sql = $"SELECT password from userinfo WHERE username = @username";
            var dbPw = await conn.ExecuteScalarAsync<string>(sql, new { username });

            Console.WriteLine("username: " + username);
            Console.WriteLine("password: " + password);
            
            Console.WriteLine("dbPW" + dbPw);

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
    
    public virtual HttpStatusCode ChangeOwnPassword(string username, string newPassword)
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            string sql = $"UPDATE userinfo SET password = @password WHERE username = @username";
            var rowAffected = conn.Execute(sql, new { username , password = newPassword });
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
    
    public virtual HttpStatusCode ChangeUserPassword(string username, string newPassword)
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            string sql = $"UPDATE userinfo SET password = @password WHERE username = @username";
            var rowAffected = conn.Execute(sql, new { username , password = newPassword });
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

    public async Task<bool> ChangeUserStatus(string username, int status)
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            string sql = $"UPDATE userinfo SET enable = @status WHERE username = @username";
            var rowAffected = await conn.ExecuteAsync(sql, new { status, username});
            Console.WriteLine(rowAffected); 
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

    public async Task<List<AllUserInfo>> GetAllUserInfo()
    {
        IDbConnection conn = new MySqlConnection(ConnStr);
        var result = new List<AllUserInfo>();
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();
            Console.WriteLine("Connected!");
            
            IDbCommand dbCommand = conn.CreateCommand();
            dbCommand.CommandText = 
                $"Select u.username, c.checkin_time, u.enable FROM userinfo.userinfo as u LEFT join userinfo.checkin as c on u.username = c.username";
            using (IDataReader reader = dbCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    var userdata = new AllUserInfo();
                    userdata.username = reader.GetString(0);
                    userdata.checkin_time = reader.IsDBNull(1) ? null : reader.GetDateTime(1).ToString();
                    userdata.enable = reader.GetInt32(2);
                    
                    result.Add(userdata);
                }
            }

            Console.WriteLine("Get all user info success.");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new List<AllUserInfo>{};
        }
        
        conn.Close();
        Console.WriteLine("Done.");
        return result;
    }
}