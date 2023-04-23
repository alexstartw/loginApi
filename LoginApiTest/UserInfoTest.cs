using System.Net;
using loginApi.Repos;
using loginApi.Services;
using NSubstitute;

namespace LoginApiTest;

public class Tests
{
    private UserInfoService _userInfoService = null;
    private AccountRepo _accountRepo = null;

    [SetUp]
    public void Setup()
    {
        _accountRepo = Substitute.For<AccountRepo>();
        _userInfoService = new UserInfoService(_accountRepo);
    }

    [Test]
    public async Task CheckPasswordRightShouldReturnTrue()
    {
        
        _accountRepo.CheckPassword("username", "password").Returns(true);
        var result = await _userInfoService.CheckPassword("username", "password");
        
        Assert.IsTrue(result);
    }

    [Test]
    public async Task WhenCreateAccountUserNotExistShouldOk()
    {

        _accountRepo.CheckUserExist("username").Returns(false);
        _accountRepo.CreateAccount("username", "password").Returns(HttpStatusCode.OK);
        var result = await _userInfoService.CreateNewUser("username", "password");
        
        Assert.That(result, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task WhenCreateAccountUserExistShouldConflict()
    {

        _accountRepo.CheckUserExist("username").Returns(true);
        
        var result = await _userInfoService.CreateNewUser("username", "password");
        
        Assert.That(result, Is.EqualTo(HttpStatusCode.Conflict));
    }
    
    [Test]
    public async Task WhenChangePasswordUserNotExistShouldBadRequest()
    {

        _accountRepo.CheckUserExist("username").Returns(false);
        
        var result = await _userInfoService.ChangePassword("username", "oldPassword", "newPassword");
        
        Assert.That(result, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test]
    public async Task WhenChangePasswordUserExistButPasswordWrongShouldBadRequest()
    {

        _accountRepo.CheckUserExist("username").Returns(true);
        _accountRepo.CheckPassword("username", "oldPassword").Returns(false);
        
        var result = await _userInfoService.ChangePassword("username", "oldPassword", "newPassword");
        
        Assert.That(result, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}