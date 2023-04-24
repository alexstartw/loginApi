using System.Net;
using loginApi.Repos;
using loginApi.Services;
using NSubstitute;

namespace LoginApiTest;

public class CheckInTest
{
    private UserInfoService _userInfoService = null;
    private CheckInService _checkInService = null;
    private AccountRepo _accountRepo = null;
    private CheckInRepo _checkInRepo = null;
    
    [SetUp]
    public void Setup()
    {
        _accountRepo = Substitute.For<AccountRepo>();
        _checkInRepo = Substitute.For<CheckInRepo>();
        _userInfoService = new UserInfoService(_accountRepo);
        _checkInService = new CheckInService(_checkInRepo, _accountRepo);
    }
    
    [Test]
    public async Task TodayCheckInShouldReturnTrue()
    {
        var tzi = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
        var nowDatetime = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, tzi);

        var localTime = nowDatetime.LocalDateTime;
        var localDay = localTime;
        if (localTime.TimeOfDay < new TimeSpan(5, 0, 0))
        {
            localDay = localDay.AddDays(-1);
        }

        _accountRepo.CheckUserExist("username").Returns(true);
        _checkInRepo.GetTodayCheckInStatus("username", localDay).Returns(false);
        _checkInRepo.TodayCheckIn("username", localDay).Returns(true);
        
        var result = await _checkInService.TodayCheckIn("username");
        
        
        Assert.That(result, Is.EqualTo(true));
    }
    
}