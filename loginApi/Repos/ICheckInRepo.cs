﻿namespace loginApi.Repos;

public interface ICheckInRepo
{
    public bool TodayCheckIn(string username);
    public Task<bool> GetTodayCheckInStatus(string username);
    public Task<int> GetCheckInCount(string username);
    public Task<int> GetAbsentCount(string username);
    
}